import json

with open("albums.json", 'r') as file:
    content = json.load(file)
    albums = content['albums']
    
    for album in albums:
        title = album.get('CleanedAlbumTitle')
        non_number = album.get('NonNumberCleanedAlbumTitle')

        if title and title[0].isdigit() and non_number:
            print(title)



# Make sure if the title starts with a letter it has a NonNumberCleanedAlbumTitle

# NonNumberCleanedAlbumTitle should be camel case

#   