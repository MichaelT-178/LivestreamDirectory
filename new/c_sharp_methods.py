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
                    name, ext = os.path.splitext(filename)
                    cleaned_name = clean_text(name)
                    new_filename = cleaned_name + ext
                    new_path = os.path.join(directory, new_filename)

                    # Always rename if the name is different, even on case-insensitive systems
                    if filename != new_filename:
                        # Rename via a temporary file to force case change on case-insensitive systems
                        temp_path = os.path.join(directory, f"__tmp__{new_filename}")
                        os.rename(full_path, temp_path)
                        os.rename(temp_path, new_path)
                        print(f"Renamed: {filename} -> {new_filename}")
                        count += 1
                    else:
                        print(f"Skipped (already clean): {filename}")

        print(f"Total files renamed: {count}")





