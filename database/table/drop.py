"""
Drops the song list table in the database
"""

import sqlite3

conn = sqlite3.connect('Songs.db')
cursor = conn.cursor()

try:
    cursor.execute("DROP TABLE Song;")
except sqlite3.OperationalError:
    pass # Table might not exist. Just pass.

conn.commit()
conn.close()