import urllib, json

def generate_first_row():
  url = "https://iss-program.herokuapp.com/api/suit/recent"
  response = urllib.urlopen(url)
  data = json.loads(response.read())
  return data[0]
