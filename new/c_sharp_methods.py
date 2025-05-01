import json
import os
from clean_text import clean_text


class CSharpMethods:
    def get_all_artists(self) -> list[str]:
        with open("../database/song_list.json", 'r') as file:
            content = json.load(file)
            songs = content['songs']
            
            artists = []

            count = 0

            for song in songs:
                # print(json.dumps(song, indent=4))
                artist = song.get("Artist")
                artist = clean_text(artist)
                
                if artist and artist not in artists:
                    artists.append(artist)
                    count += 1

                other_artists = song.get("Other_Artists")

                if other_artists.strip():
                    for other_artist in other_artists.split("+ "):
                        clean_other_artist = clean_text(other_artist)

                        if clean_other_artist not in artists:
                            artists.append(clean_other_artist)
                            count += 1

            return artists, count
        
    
    def list_image_files(self):
        directory = '../pics'

        count = 0 

        artists = []

        for filename in os.listdir(directory):
            full_path = os.path.join(directory, filename)
            if os.path.isfile(full_path) and ".DS_Store" not in full_path:
                artists.append(filename.replace(".jpg", ""))
                count += 1

        return artists, count
    

    def rename_images_temp(self):
        directory = '../pics'
        count = 0
        artists = []

        for filename in os.listdir(directory):
            full_path = os.path.join(directory, filename)

            if os.path.isfile(full_path) and filename.lower().endswith('.jpg') and ".DS_Store" not in filename:
                name, ext = os.path.splitext(filename)
                new_filename = f"{name}_TEMP{ext}"
                new_path = os.path.join(directory, new_filename)

                os.rename(full_path, new_path)
                artists.append(name)
                count += 1
                print(f"Renamed: {filename} -> {new_filename}")

        print(f"Total renamed: {count}")
        return artists, count

