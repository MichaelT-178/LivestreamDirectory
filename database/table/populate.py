"""
Populates the table in the databse using the song_list.json file
Just meant to see the contents of the json file as a table.
Use SQLite Viewer to view table.
"""



import sqlite3
import json

with open("../song_list.json", 'r') as file:
    contents = file.read()
    data = json.loads(contents)



conn = sqlite3.connect('example.db')

cursor = conn.cursor()

# SQL COMMAND HERE 

for song in data['songs']:

    title = song["Title"]
    artist = song["Artist"]
    other_artists = song["Other_Artists"]
    instruments = song["Instruments"]
    artist_pic = song["Image"]
    search = song["Search"]
    appearances = song["Appearances"]
    links = song["Links"]

    sql = '''INSERT INTO Song (Title, Artist, Other_Artists, Instruments, 
                               Image, Search, Appearances, Links)
             VALUES (?, ?, ?, ?, ?, ?, ?, ?);
          '''
    
    cursor.execute(sql, (title, artist, other_artists, instruments, artist_pic, search, appearances, links))


conn.commit()
conn.close()