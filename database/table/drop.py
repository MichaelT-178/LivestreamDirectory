"""
Drops the song list table in the database
"""

import sqlite3

conn = sqlite3.connect('Songs.db')

cursor = conn.cursor()

#SQL COMMAND HERE 
cursor.execute("DROP TABLE Song;")

conn.commit()
conn.close()