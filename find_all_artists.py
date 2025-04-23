import json

# with open("./database/song_list.json", 'r') as file:

#     content = json.load(file)

#     songs = content['songs']

#     for song in songs:
#         if "/" in song['Artist']:
#             print(song['Artist'])


with open("./db_manager/timestamps/all-timestamps.txt", 'r') as file:
    for line in file:
        line_data = line.rsplit(" by ", 1)

        if len(line_data) > 1:
            artists = line_data[1]
            
            if "+" in artists:
                print(artists)


        # if "," in song['Other_Artists']:
        #     print(song['Other_Artists'])




# AC/DC
# AC/DC
# Yusuf / Cat Stevens
