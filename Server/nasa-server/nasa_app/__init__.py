import os

from flask import Flask, jsonify
from nasa_app.db import get_db
import json
import random

def create_app(test_config=None):
    # create and configure the app
    app = Flask(__name__, instance_relative_config=True)
    app.config.from_mapping(
        SECRET_KEY='dev',
        DATABASE=os.path.join(app.instance_path, 'nasa_app.sqlite'),
    )

    if test_config is None:
        # load the instance config, if it exists, when not testing
        app.config.from_pyfile('config.py', silent=True)
    else:
        # load the test config if passed in
        app.config.from_mapping(test_config)

    # ensure the instance folder exists
    try:
        os.makedirs(app.instance_path)
    except OSError:
        pass


    from . import generate_data_suit
    @app.route('/suit')
    def suit():
        db = get_db()

        if db.execute("SELECT id FROM suitstatus WHERE id = 1").fetchone() is None:
            dd = generate_data_suit.generate_first_row()
            dd_tuple = (dd["p_h2o_g"], dd["heart_bpm"], dd["p_sub"], dd["t_water"], dd["p_sop"], dd["p_h2o_l"], dd["rate_o2"], dd["p_o2"], dd["cap_battery"], dd["rate_sop"], dd["t_sub"], dd["t_oxygen"], dd["v_fan"], dd["p_suit"], dd["t_battery"])
            db.execute("INSERT INTO suitstatus (p_h2o_g, heart_bpm, p_sub, t_water, p_sop, p_h2o_l, rate_o2, p_o2, cap_battery, rate_sop, t_sub, t_oxygen, v_fan, p_suit, t_battery) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", dd_tuple)
            db.commit()

        else:
            prev_row = get_latest_row(db)
            new_row = generate_variance(prev_row)
            new_row = new_row[0]
            new_row_tuple = (new_row["p_h2o_g"], new_row["heart_bpm"], new_row["p_sub"], new_row["t_water"], new_row["p_sop"], new_row["p_h2o_l"], new_row["rate_o2"], new_row["p_o2"], new_row["cap_battery"], new_row["rate_sop"], new_row["t_sub"], new_row["t_oxygen"], new_row["v_fan"], new_row["p_suit"], new_row["t_battery"])
            db.execute("INSERT INTO suitstatus (p_h2o_g, heart_bpm, p_sub, t_water, p_sop, p_h2o_l, rate_o2, p_o2, cap_battery, rate_sop, t_sub, t_oxygen, v_fan, p_suit, t_battery) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", new_row_tuple)
            db.commit()


        latest_row = get_latest_row(db)
        latest_row = clean_row(latest_row)
        return jsonify(latest_row)


    from . import db
    db.init_app(app)

    return app

def get_latest_row(db):
    cursor = db.cursor()
    latest_row = cursor.execute("SELECT * FROM suitstatus WHERE id=(SELECT max(id) FROM suitstatus)").fetchall()
    latest_row = [dict(zip([key[0] for key in cursor.description], row)) for row in latest_row]
    return latest_row

def clean_row(row):
    del row[0]["id"]
    row[0]["create_date"] = row[0]["create_date"].strftime("%Y-%m-%dT%H:%M:%S.%f")[:-3]
    row[0]["create_date"] = row[0]["create_date"] + 'Z'
    for key in row[0]:
        row[0][key] = str(row[0][key])
    return row

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


        # TODO modify because type(row_dict[key]) is "unicode"
        # if type(row_dict[key]) == type("string"):
        #     prev_time = row_dict["create_date"]
        #     curr_time = datetime.datetime.now()
        #     delta_time = curr_time - prev_time
        #     print delta_time
    return row