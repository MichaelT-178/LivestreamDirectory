import os
import subprocess
import webbrowser
from spotify_images import SpotifyApi
from clean_text import clean_text
from push_to_vue import push_to_vue
from termcolor import colored as c

def write_to_clipboard(output: str):
	process = subprocess.Popen('pbcopy', env={'LANG': 'en_US.UTF-8'}, stdin=subprocess.PIPE)
	process.communicate(output.encode('utf-8'))


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
    
    non_number_line = ''
    
    if artist.strip() and artist.strip()[0].isdigit():
        non_number_line = '        "NonNumberCleanedArtist": "",\n'
        

    artist_output = f'''"{clean_artist_name}": {{
        "Artist": "{artist.strip()}",
        "CleanedArtist": "{clean_artist_name}",
{non_number_line}        "Location": "",
        "YearFormed": 0,
        "Genre": "",
        "Country": "",
        "CleanedCountry": "",
        "Emoji": ""
    }}'''


    print("\n")
    print(artist_output)
    print()

    print(c("Artist object copied to clipboard!", 'green'))
    print(c("Go paste this in 'db_manager/json_files/artists.json'", 'green'))

    os.system('python3 ../../VueLivestreamDirectory/python/shrink_spotify_images.py')

    write_to_clipboard(artist_output)
else:
    print(c('\n\nThe image already exists in the pics folder!', 'green'))



webbrowser.open("https://music.apple.com/us/new")

push_to_vue()


