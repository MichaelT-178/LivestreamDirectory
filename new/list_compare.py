import json
from termcolor import colored as c


with open("list1.json", 'r') as file1:
    file1_content = json.load(file1)


with open("list2.json", 'r') as file2:
    pics = json.load(file2)

print(c("not in pics", "green"))
for title in file1_content:
    if title not in pics:
        print(title)

print(c("\n\n\nnot in file 1", "red"))
for artist in pics:
    if artist not in file1_content:
        print(artist)