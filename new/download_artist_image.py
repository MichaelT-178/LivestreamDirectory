import os
from spotify_images import SpotifyApi
from clean_text import clean_text
from push_to_vue import push_to_vue
from termcolor import colored as c

artist = input("Enter the artist name: ")

clean_artist_name = clean_text(artist)


artist_dict = {
    "name": artist.strip(),
    "cleanedName": clean_artist_name,
    "id": 1
}

SpotifyApi.process_artist_image(artist_dict)


file_path = f'../pics/{clean_artist_name}.jpg'

if not os.path.isfile(file_path):
    os.system(f'cp ../../VueLivestreamDirectory/src/assets/ArtistPics/{clean_artist_name}.jpg ../pics/')
    print(c('\n\nCopied new image to the pics folder!', 'green'))
else:
    print(c('\n\nThe image already exists in the pics folder!', 'green'))


push_to_vue()

