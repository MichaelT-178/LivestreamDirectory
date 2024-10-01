"""
Count the number of videos in 
each list of the blocked.json file.

should add up to a total of 166
"""

import json
from termcolor import colored as c

with open("data/blocked.json", 'r') as file:
    content = json.load(file)
    
    blocked = content["blocked"]
    not_blocked = content["not_blocked"]
    previously_blocked = content["previously_blocked"]

    print(c("\nStats", 'magenta'))
    print(f"blocked: {c(len(blocked), 'blue')}")
    print(f"not_blocked: {c(len(not_blocked), 'blue')}")
    print(f"previously_blocked: {c(len(previously_blocked), 'blue')}")

    print(f"\nTotal: {c(len(blocked) + len(not_blocked) + len(previously_blocked), 'blue')}\n")