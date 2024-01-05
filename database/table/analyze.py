import json

"""
This file is meant to analyze the contents of the song_list.json data base
"""



with open("../song_list.json", 'r') as file:
    contents = file.read()
    data = json.loads(contents)


longest_title_line = ""
biggest_title_len = 0

longest_artist_line = ""
biggest_artist_len = 0

longest_other_artist_line = ""
biggest_other_artist_len = 0

longest_instruments_line = ""
biggest_instruments_len = 0

longest_image_line = ""
biggest_image_len = 0

longest_search_line = ""
biggest_search_len = 0

longest_appearances_line = ""
biggest_appearances_len = 0

longest_links_line = ""
biggest_links_len = 0


for song in data['songs']:
    if song["Instruments"].count(",") == 2:
        print(song["Instruments"])
        print(song["Title"])

    # if len(song["Title"]) > biggest_title_len:
    #     longest_title_line = song["Title"]
    #     biggest_title_len = len(song["Title"])

    # if len(song["Artist"]) > biggest_artist_len:
    #     longest_artist_line = song["Artist"]
    #     biggest_artist_len = len(song["Artist"])

    # if len(song["Other_Artists"]) > biggest_other_artist_len:
    #     longest_other_artist_line = song["Other_Artists"]
    #     biggest_other_artist_len = len(song["Other_Artists"])

    # if len(song["Instruments"]) > biggest_instruments_len:
    #     longest_instruments_line = song["Instruments"]
    #     biggest_instruments_len = len(song["Instruments"])

    # if len(song["Image"]) > biggest_image_len:
    #     longest_image_line = song["Image"]
    #     biggest_image_len = len(song["Image"])

    # if len(song["Search"]) > biggest_search_len:
    #     longest_search_line = song["Search"]
    #     biggest_search_len = len(song["Search"])

    # if len(song["Appearances"]) > biggest_appearances_len:
    #     longest_appearances_line = song["Appearances"]
    #     biggest_appearances_len = len(song["Appearances"])

    # if len(song["Links"]) > biggest_links_len:
    #     longest_links_line = song["Links"]
    #     biggest_links_len = len(song["Links"])


# print("Title")
# print(longest_title_line)
# print(biggest_title_len)

# print("\nArtist")
# print(longest_artist_line)
# print(biggest_artist_len)

# print("\nOther Artist")
# print(longest_other_artist_line)
# print(biggest_other_artist_len)

# print("\nInstruments")
# print(longest_instruments_line)
# print(biggest_instruments_len)

# print("\nImage")
# print(longest_image_line)
# print(biggest_image_len)

# print("\nSearch")
# # print(longest_search_line)
# print(biggest_search_len)

# print("\nAppearances")
# # print(longest_appearances_line)
# print(biggest_appearances_len)

# print("\nLinks")
# # print(longest_links_line)
# print(biggest_links_len)


    # print()
    # print(song["Title"])
    # print(song["Artist"])
    # print(song["Other_Artists"])
    # print(song["Instruments"])
    # print(song["Image"])
    # print(song["Search"])
    # print(song["Appearances"])
    # print(song["Links"])
    # print()