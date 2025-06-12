from spotify_images import SpotifyApi
from clean_text import clean_text
from push_to_vue import push_to_vue

artist = input("Enter the artist name: ")

clean_artist_name = clean_text(artist)


artist_dict = {
    "name": artist.strip(),
    "cleanedName": clean_artist_name,
    "id": 1
}

SpotifyApi.process_artist_image(artist_dict)


push_to_vue()

