import json


with open("instruments.json", "r") as file:
    content = json.load(file)
    instruments = content['Instruments']

    for instrument in instruments:
        if "appears" in instrument:
            if instrument['appears'] == 1:
                ins_key = instrument['name'].split("-", 1)[0].strip()
                print(f"{instrument['id']}", end="")
                print(f".Replace(\"{ins_key}\", \"\")")