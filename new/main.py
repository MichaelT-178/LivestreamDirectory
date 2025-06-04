from music_data import MusicData
from spotify_images import CLIENT_ID, CLIENT_SECRET, SpotifyApi

artists = MusicData.get_artists()
songs = MusicData.get_songs()





access_token = SpotifyApi.get_access_token(CLIENT_ID, CLIENT_SECRET)


for song in songs:
    SpotifyApi.process_song_image(song, access_token)