from music_data import MusicData
from spotify_images import SpotifyApi

artists = MusicData.get_artists()
songs = MusicData.get_songs()



# # song_obj =        {
# #             "id": 561,
# #             "title": "Like a Rolling Stone",
# #             "cleanedTitle": "like-a-rolling-stone",
# #             "artist": "Bob Dylan",
# #             "cleanedArtist": "bob-dylan",
# #             "album": "Highway 61 Revisted",
# #             "cleanedAlbum": "highway-61-revisted",
# #             "otherArtists": "",
# #             "instruments": "Acoustic Guitar, (main) - Stonebridge (Furch) OM32SM",
# #             "search": "",
# #             "CleanedPicture": "highway-61-revisted",
# #             "Type": "Song"
# #         }
 
# # SpotifyApi.fallback_album_image_url_by_album_only(song_obj)
# # # SpotifyApi.fallback_album_image_by_album_and_artist_exact(song_obj, "spirit", 1968)

# album_link = "https://open.spotify.com/album/6YabPKtZAjxwyWbuO9p4ZD?si=_-WBJOh3Sq-nA9sDq8Qpvg"
# SpotifyApi.get_album_image_by_url(album_link, "Highway 61 Revisted")
# exit(0)





for song in songs:
    SpotifyApi.process_album_image(song)


cool = input("Done? : ")


for artist in artists:
    SpotifyApi.process_artist_image(artist)



