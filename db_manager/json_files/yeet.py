import json


with open("albums.json", 'r') as file:
    content = json.load(file)
    albums = content['albums']
    
    for album in albums:
        if album['Song'] == album['AlbumTitle']:
            print(album['Song'])