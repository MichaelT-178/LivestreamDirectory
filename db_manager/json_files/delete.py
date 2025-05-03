import json

# Load the existing albums from the JSON file
with open('albums.json', 'r', encoding='utf-8') as file:
    data = json.load(file)

# Rebuild each album with "AlbumImage" before "Year"
updated_albums = []
for album in data.get("albums", []):
    updated_album = {}
    for key in album:
        if key == "Year":
            updated_album["CleanedAlbumTitle"] = None
        updated_album[key] = album[key]
    updated_albums.append(updated_album)

# Write the updated data back to the file
with open('albums.json', 'w', encoding='utf-8') as file:
    json.dump({"albums": updated_albums}, file, indent=4, ensure_ascii=False)
