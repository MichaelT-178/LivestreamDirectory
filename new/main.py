from music_data import MusicData
from spotify_images import SpotifyApi

artists = MusicData.get_artists()
songs = MusicData.get_songs()



song_obj =         {
            "id": 618,
            "title": "Strange Meeting II",
            "cleanedTitle": "strange-meeting-ii",
            "artist": "Nick Drake",
            "cleanedArtist": "nick-drake",
            "album": "Family Tree",
            "cleanedAlbum": "family-tree",
            "otherArtists": "",
            "instruments": "Acoustic Guitar, (M15M) - Martin 00-15m",
            "search": "",
            "CleanedPicture": "family-tree",
            "Type": "Song"
        },

SpotifyApi.fallback_album_image_url_by_album_only(song_obj)
# # # SpotifyApi.fallback_album_image_by_album_and_artist_exact(song_obj, "spirit", 1968)

# album_link = "https://open.spotify.com/album/4jCpSNa8dV40aMrHOshRdv?si=YUwrazE_ReWnLEEOs5lzyw"
# SpotifyApi.get_album_image_by_url(album_link, "It's Only Rock'N'Roll")
exit(0)





for song in songs:
    SpotifyApi.process_album_image(song)


cool = input("Done? : ")


for artist in artists:
    SpotifyApi.process_artist_image(artist)



