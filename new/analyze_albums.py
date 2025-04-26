# import json


# with open("../db_manager/json_files/albums.json", 'r') as file:
#     content = json.load(file)
#     albums = content['albums']


import json
from collections import OrderedDict

# Load the existing JSON file
with open("../db_manager/json_files/albums.json", 'r', encoding='utf-8') as file:
    content = json.load(file)
    albums = content['albums']

# Add an "id" as the first attribute
new_albums = []
for idx, album in enumerate(albums, start=1):
    ordered_album = OrderedDict()
    ordered_album['id'] = idx
    for key, value in album.items():
        ordered_album[key] = value
    new_albums.append(ordered_album)

# Save the updated JSON back to the same file
with open("../db_manager/json_files/albums.json", 'w', encoding='utf-8') as file:
    json.dump({"albums": new_albums}, file, indent=4, ensure_ascii=False)

print("IDs added as the first attribute successfully!")




