import re

def extract_keys(s):
    match = re.search(r'\(([^)]+)\)', s)
    if match:
        contents = match.group(1)
        return [part.strip() for part in contents.split('/')]
    return []



all_keys = []


with open("instrument_results.txt", "r") as file:
    for line in file:
        if "Livestream " in line:
            key_list = extract_keys(line)
            
            for key in key_list:
                if key not in all_keys:
                    all_keys.append(key)


print("KEYS HERE")

for key in sorted(all_keys):
    print(key)