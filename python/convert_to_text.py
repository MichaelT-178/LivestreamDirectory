import json

"""
Converts the blocked.json file into the affected.txt file
"""

with open("data/blocked.json", 'r') as file:
    content = json.load(file)


with open("data/affected.txt", "w") as file:
    file.write("Blocked\n")

    for video in content["blocked"]:
        file.write(f'{video["title"]} - {video["link"]}\n')




    file.write("\n\nNot Blocked\n")

    for video in content["not_blocked"]:
        file.write(f'{video["title"]} - {video["link"]}\n')




    file.write("\n\nPreviously Blocked\n")

    for video in content["previously_blocked"]:
        file.write(f'{video["title"]} - {video["link"]}\n')