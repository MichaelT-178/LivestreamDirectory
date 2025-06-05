from music_data import MusicData
from spotify_images import SpotifyApi

artists = MusicData.get_artists()
songs = MusicData.get_songs()



# song_obj = {
#     "id": 164,
#     "title": "Dance Monkey",
#     "cleanedTitle": "dance-monkey",
#     "artist": "Tones And I",
#     "cleanedArtist": "tones-and-i",
#     "album": "The Kids Are Coming",
#     "cleanedAlbum": "the-kids-are-coming-ep",
#     "otherArtists": "",
#     "instruments": "Acoustic Guitar, (main) - Stonebridge (Furch) OM32SM",
#     "search": "",
#     "CleanedPicture": "the-kids-are-coming-ep",
#     "Type": "Song"
# }

# SpotifyApi.fallback_album_image_url_by_album_only(song_obj)
# exit(0)





for song in songs:
    SpotifyApi.process_album_image(song)


cool = input("Done? : ")


for artist in artists:
    SpotifyApi.process_artist_image(artist)



