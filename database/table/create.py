"""
Creates the song list table in the database
"""

import sqlite3

conn = sqlite3.connect('Songs.db')

cursor = conn.cursor()

cursor.execute('''
    CREATE TABLE Song (
        Title VARCHAR(100),
        Artist VARCHAR(50),
        Other_Artists VARCHAR(100),
        Instruments VARCHAR(255),
        Image VARCHAR(50),
        Search VARCHAR(255),
        Appearances TEXT(3000),
        Links TEXT(7000)
    );
''')

conn.commit()
conn.close()

# Attribute and max string length
# Title - 66
# Artist - 33
# Other Artist - 38
# Instruments - 188
# Image - 37
# Search - 151
# Appearances - 2725
# Links - 6704