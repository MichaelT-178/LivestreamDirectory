import json


with open("instruments.json", "r") as file:
    content = json.load(file)
    instruments = content['Instruments']

    for instrument in instruments:
        print(instrument['name'])