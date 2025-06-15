import json


with open("old_artists.json", "r") as file:
    original_data = json.load(file)


simplified_data = {}

for key, artist in original_data.items():
    simplified_data[key] = {
        "Artist": artist.get("Artist", ""),
        "CleanedArtist": artist.get("CleanedArtist", ""),
        "Location": "",
        "YearFormed": 0,
        "Genre": ""
    }

with open("artists.json", "w", encoding="utf-8") as outfile:
    json.dump(simplified_data, outfile, indent=4, ensure_ascii=False)
