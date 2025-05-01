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
                
                if artist and artist not in artists:
                    artists.append(artist)
                    count += 1

                other_artists = song.get("Other_Artists")

                if other_artists.strip():
                    for other_artist in other_artists.split("+ "):
                        if other_artist not in artists:
                            artists.append(other_artist)

            return artists, count
    
    # def list_image_files(self):
    #     directory = '../pics'

    #     count = 0 
    #     for filename in os.listdir(directory):
    #         full_path = os.path.join(directory, filename)
    #         if os.path.isfile(full_path):
    #             print(filename)
    #             count += 1
    #     print(count)

    def list_image_files(self):
        directory = '../pics'
        count = 0 

        for filename in os.listdir(directory):
            if filename.endswith(".jpg"):
                full_path = os.path.join(directory, filename)

                if os.path.isfile(full_path):
                    name, ext = os.path.splitext(filename)  # Split name and extension
                    cleaned_name = clean_text(name)
                    new_filename = cleaned_name + ext
                    new_path = os.path.join(directory, new_filename)

                    # Avoid overwriting existing files
                    if not os.path.exists(new_path):
                        os.rename(full_path, new_path)
                        print(f"Renamed: {filename} -> {new_filename}")
                    else:
                        print(f"Skipped (name exists): {filename} -> {new_filename}")

                    count += 1

        print(f"Total files renamed: {count}")



