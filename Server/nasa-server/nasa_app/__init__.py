import os
from flask import Flask, jsonify
from nasa_app.db import get_db
import random
from datetime import datetime
from . import generate_data_suit

# app factory
def create_app():
    # create and configure the app
    app = Flask(__name__, instance_relative_config=True)
    app.config.from_mapping(
        SECRET_KEY='dev',
        DATABASE=os.path.join(app.instance_path, 'nasa_app.sqlite'),
    )

    # ensure the instance folder exists
    try:
        os.makedirs(app.instance_path)
    except OSError:
        pass

    # creating the database
    from . import db
    db.init_app(app)

    # creating page to GET telemetry suit data from 
    @app.route('/api/suit/recent')
    def suit():
        db = get_db()
        # if this is the first GET, generate data from the most recent NASA data
        if db.execute("SELECT id FROM suitstatus WHERE id = 1").fetchone() is None:
            dd = generate_data_suit.generate_first_row()
            dd_tuple = (dd["p_h2o_g"], dd["heart_bpm"], dd["p_sub"], dd["t_water"],
                        dd["p_sop"], dd["p_h2o_l"], dd["rate_o2"], dd["p_o2"],
                        dd["cap_battery"], dd["rate_sop"], dd["t_sub"], dd["t_oxygen"],
                        dd["v_fan"], dd["p_suit"], dd["t_battery"])
            sql = '''INSERT INTO suitstatus (p_h2o_g, heart_bpm, p_sub, t_water, p_sop,
                p_h2o_l, rate_o2, p_o2, cap_battery, rate_sop, t_sub, t_oxygen, v_fan,
                p_suit, t_battery) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)'''
            db.execute(sql, dd_tuple)
            db.commit()
        # otherwise, take the last row, add variance, and commit it to the database
        else:
            prev_row = get_latest_row(db, "suitstatus")
            new_row = generate_variance(prev_row)
            new_row = new_row[0]
            new_row_tuple = (new_row["p_h2o_g"], new_row["heart_bpm"], new_row["p_sub"],
                            new_row["t_water"], new_row["p_sop"], new_row["p_h2o_l"],
                            new_row["rate_o2"], new_row["p_o2"], new_row["cap_battery"],
                            new_row["rate_sop"], new_row["t_sub"], new_row["t_oxygen"],
                            new_row["v_fan"], new_row["p_suit"], new_row["t_battery"])
            sql = '''INSERT INTO suitstatus (p_h2o_g, heart_bpm, p_sub, t_water, p_sop,
                p_h2o_l, rate_o2, p_o2, cap_battery, rate_sop, t_sub, t_oxygen, v_fan,
                p_suit, t_battery) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)'''
            db.execute(sql, new_row_tuple)
            db.commit()
            decrement_time(db)

        # cleans up the latest row to match NASA's JSON format, and return it
        latest_row = get_latest_row(db, "suitstatus")
        latest_row = clean_row(latest_row)
        return jsonify(latest_row)


    return app

# gets the most recent NASA data
def generate_first_row():
    url = "https://iss-program.herokuapp.com/api/suit/recent"
    response = urllib.urlopen(url)
    data = json.loads(response.read())
    return data[0]

# returns selected row from table
# ex: diff=1 means the second to last row
def get_latest_row(db, table, diff=0):
    cursor = db.cursor()
    sql = '''SELECT * FROM tablename WHERE id=((SELECT max(id) FROM tablename) - diffnum)'''
    # aka, SQL injection attack
    sql = sql.replace("tablename", table).replace("diffnum", str(diff))
    latest_row = cursor.execute(sql).fetchall()
    latest_row = [dict(zip([key[0] for key in cursor.description], row)) 
                    for row in latest_row]
    return latest_row

# returns "var" from row in "table"
# diff is a positive integer specifying a row
# ex: diff=1 means the second to last row
def get_latest_variable(db, table, var, diff=0):
    return get_latest_row(db, table, diff)[0][var]

# modifies a row (list) to match NASA's formats
def clean_row(row):
    del row[0]["id"]
    row[0]["create_date"] = row[0]["create_date"].strftime("%Y-%m-%dT%H:%M:%S.%f")[:-3]
    row[0]["create_date"] = row[0]["create_date"] + 'Z'
    for key in row[0]:
        row[0][key] = str(row[0][key])
    return row

# modifies a row (list) variables to add variance
# control the variance in the "delta" variables
# new number will always be positive
def generate_variance(row):
    row_dict = row[0]
    for key in row_dict:
        if type(row_dict[key]) == type(1):
            up_down = random.randint(0,2)
            delta = random.randint(0,5)
            if up_down == 0:
                row_dict[key] = row_dict[key] + delta
            else:
                row_dict[key] = row_dict[key] - delta
            row_dict[key] = abs(row_dict[key])

        elif type(row_dict[key]) == type(1.0):
            up_down = random.randint(0,2)
            delta = round(random.uniform(0,5), 2)
            if up_down == 0:
                row_dict[key] = row_dict[key] + delta
            else:
                row_dict[key] = row_dict[key] - delta
            row_dict[key] = abs(row_dict[key])

        # elif key[:2] == "t_":
        #     curr_time = datetime.now()
        #     print curr_time
        #     prev_time = row_dict["create_date"]
        #     delta_time = curr_time - prev_time
        #     curr_t_ = datetime.strptime(row_dict[key], "%H:%M:%S")
        #     print delta_time
    return row

# updates latest row's "var" in "table" to have "new_val".
def update_variable(db, table, var, new_val):
    sql = '''UPDATE tablename SET varname="newvalue" 
            WHERE id=(SELECT max(id) FROM tablename)'''
    sql = sql.replace("tablename", table).replace("varname", var).replace("newvalue", new_val)
    db.execute(sql)
    db.commit()

# run after adding a new row to the database!
# modifies the "t_..." variables by calculating time difference between latest row's
# creation time and previous row's creation time
# if time ends up being negative, resets to first row times
def decrement_time(db):
    curr_time = get_latest_variable(db, "suitstatus", "create_date", 0)
    prev_time = get_latest_variable(db, "suitstatus", "create_date", 1)
    delta_time = curr_time - prev_time

    variables = ["t_battery", "t_oxygen", "t_water"]
    for t_ in variables:
        curr_t_ = get_latest_variable(db, "suitstatus", t_)
        curr_t_ = datetime.strptime(curr_t_, "%H:%M:%S")
        try:
            new_t_ = curr_t_ - delta_time
            new_t_ = unicode(new_t_.strftime("%H:%M:%S"))
            update_variable(db, "suitstatus", t_, new_t_)
        except:
            new_t_ = generate_data_suit.generate_first_row()[t_]
            update_variable(db, "suitstatus", t_, new_t_)
