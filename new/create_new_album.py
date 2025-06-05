import subprocess
from clean_text import clean_text
from termcolor import colored as c 
from spotify_images import SpotifyApi

def write_to_clipboard(output: str):
	process = subprocess.Popen('pbcopy', env={'LANG': 'en_US.UTF-8'}, stdin=subprocess.PIPE)
	process.communicate(output.encode('utf-8'))



song = input("\nWhat's the song title? : ")

album = input("\nWhat's the album title? : ")

album_spotify_link = input("\nWhat's the album's spotify link? : ")


# Download album image
SpotifyApi.get_album_image_by_url(album_spotify_link, album)
print(c('\nSuccessfully downloaded album image!', 'green'))


artist = input("\nWho's the artist? : ")



cleaned_song = clean_text(song)
cleaned_album = clean_text(album)
cleaned_artist = clean_text(artist)

year = int(input("\nWhat year was in released? : "))


print("\n")

album_str = ""

album_str +=  '  {\n'
album_str += f'      "id": 1000,\n'
album_str += f'      "Song": "{song}",\n'
album_str += f'      "CleanedSong": "{cleaned_song}",\n'
album_str += f'      "AlbumTitle": "{album}",\n'
album_str += f'      "CleanedAlbumTitle": "{cleaned_album}",\n'
album_str += f'      "Artist": "{artist}",\n'
album_str += f'      "CleanedArtist": "{cleaned_artist}",\n'
album_str += f'      "Year": {year}\n'
album_str +=  '    },'


print(album_str)

write_to_clipboard(album_str)

print(c("\nSuccessfully copied to clipboard! Paste in albums.json", 'green'))