#mport os; os.system('clear')
import json

with open("/Users/michaeltotaro/LivestreamDirectory/database/song_list.json") as file:
    data = json.load(file) 

    for song in data["songs"]:
        if len(song['Title'].split(" ")) == 1:
            print(song['Title'])
             