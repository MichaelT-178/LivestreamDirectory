import json

"""
This file is meant to analyze the contents of the song_list.json data base
"""

with open("../song_list.json", 'r') as file:
    contents = file.read()
    data = json.loads(contents)

for song in data['songs']:
    print()
    print(song["Id"])
    print(song["Title"])
    print(song["Artist"])
    print(song["CleanedArtist"])
    print(song["Other_Artists"])
    print(song["Instruments"])
    print(song["Search"])
    print(song["Appearances"])
    print(song["Links"])
    print()
