import json

class AlbumSongData:
    def get_album_data(self):
        """
        Get albums.json data as an object list 

        Three attributes: id, Title, Album, Year
        """
        with open("../db_manager/json_files/albums.json", 'r') as album_file:
            content = json.load(album_file)
            albums = content['albums']

            return [album for album in albums]

    def get_album_songs(self):
        """
        Return titles from albums.json as a string list.
        """
        album_data = self.get_album_data() 
        return [album['Title'] for album in album_data]

    def get_repertoire_data(self):
        """
        Return song data from repertoire.json as an object list

        Two attributes: Title and Artist.
        
        """
        with open("../db_manager/json_files/repertoire.json", 'r') as repertoire_file:
            repertoire = json.load(repertoire_file)

            all_song_data = []

            for song in repertoire:
                if song.strip():
                    title, artist = song.rsplit(' by ', 1)

                    song_data = {
                        "Title": title.strip(),
                        "Artist": artist.strip()
                    }

                    all_song_data.append(song_data)
                    # print(json.dumps(song_data, indent=4))

            return all_song_data

    def get_repertoire_songs(self):
        """
        Return titles from repertoire.json as a string list.
        """
        repertoire_data = self.get_repertoire_data()
        return [song['Title'] for song in repertoire_data]


