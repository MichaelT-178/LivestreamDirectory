from music_data import MusicData
from spotify_images import SpotifyApi

artists = MusicData.get_artists()
songs = MusicData.get_songs()


for song in songs:
    SpotifyApi.process_album_image(song)


cool = input("Done? : ")


for artist in artists:
    SpotifyApi.process_artist_image(artist)