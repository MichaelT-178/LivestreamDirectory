import json


def load_data():

    path_to_data = "../../VueLivestreamDirectory/src/assets/Data/SearchData.json"

    with open(path_to_data, 'r') as file:
        content = json.load(file)
        return content["SearchData"]
    



class MusicData:

    @staticmethod
    def get_artists():
        return [item for item in load_data() if item['Type'] == 'Artist']

    @staticmethod
    def get_songs():
        return [item for item in load_data() if item['Type'] == 'Song']





# Artist
# {
#     "id": 2,
#     "name": "Corey Heuvel",
#     "cleanedName": "corey-heuvel",
#     "Type": "Artist"
# },





# Song
# {
#     "id": 817,
#     "title": "Takin' Care Of Business (Audio Issues)",
#     "cleanedTitle": "takin-care-of-business-audio-issues",
#     "artist": "Bachman-Turner Overdrive",
#     "cleanedArtist": "bachman-turner-overdrive",
#     "album": "Bachman-Turner Overdrive II",
#     "cleanedAlbum": "bachman-turner-overdrive-ii",
#     "otherArtists": "",
#     "instruments": "Acoustic Guitar, (main) - Stonebridge (Furch) OM32SM",
#     "search": "Takin Care Of Business, Takin’ Care Of Business, Bachman Turner Overdrive, Takin‘ Care Of Business",
#     "CleanedPicture": "bachman-turner-overdrive-ii",
#     "Type": "Song"
# }