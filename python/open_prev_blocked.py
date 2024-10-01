import json
import webbrowser

"""
Open livestreams that were supposedly unblocked in the 
browser to confirm they were actually unblocked
"""

with open("data/blocked.json", 'r') as file:
    content = json.load(file)
    prev_blocked_list = content["previously_blocked"]

    for i in range(len(prev_blocked_list)):
        if i % 10 == 0:
            pause = input("YEET: ")

        webbrowser.open(prev_blocked_list[i]["link"])