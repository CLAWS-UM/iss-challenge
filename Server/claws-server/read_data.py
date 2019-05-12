import urllib, json
# url = "https://iss-program.herokuapp.com/api/suit/recent" # suit status
url = "https://iss-program.herokuapp.com/api/suitswitch/recent" # warnings
response = urllib.urlopen(url)
data = json.loads(response.read())

for key in data[0]:
  print key + ": " + str(data[0][key])