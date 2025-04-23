import json

with open("./db_manager/json_files/repertoire.json", 'r') as file:
    content = json.load(file)

    songs = []

    for line in content:
        if line.strip():
            if " by " in line:
                title = line.rsplit(" by ", 1)[0]
                songs.append({
                    "Title": title,
                    "Album": "",
                    "Year": 0
                })

    output = {
        "songs": songs
    }


with open("./db_manager/json_files/albums.json", 'w') as wfile:
    wfile.write(json.dumps(output, indent=4))