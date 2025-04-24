import json


with open("./db_manager/json_files/albums.json", 'r') as file:
    content = json.load(file)
    albums = content['albums']

    for album in albums:
        if album['Album'] is None:
            print(album['Title'])