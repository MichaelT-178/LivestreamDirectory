import json
import re
from clean_text import clean_text


def remove_instrument_key_prefix(instrument_name):
    if not instrument_name:
        return instrument_name
    pattern = r'^\([^)]*\)\s*-\s*'
    return re.sub(pattern, '', instrument_name)

all_names = []

with open("../db_manager/json_files/instruments.json", "r") as file:
    content = json.load(file)
    instruments = content['Instruments']

    for instrument in instruments:
        if instrument['name'] not in all_names:
            all_names.append(instrument['name'])
            noPrefix = remove_instrument_key_prefix(instrument['name'])

            if clean_text(noPrefix) != instrument['cleaned']:
                print(instrument['cleaned'])


# with open("../database/song_list.json", 'r') as file:
#     content = json.load(file)
#     songs = content['songs']

#     final_ins = []

#     for song in songs:
#         instruments = song['Instruments'].split(", ")
#         for instrument in instruments:
#             if instrument not in final_ins:
#                 final_ins.append(instrument)

#     # for cool in final_ins:
#     #     # print(cool)
#     #     if cool not in all_names:
#     #         # print(cool)
