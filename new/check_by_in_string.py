excluded_keywords = (
    "https", 
    "Livestream", 
    "Solo Video", 
    " by "
    "Acoustic Guitar"
    "Electric Guitar"
    "Acoustic Guitar"
    "Classical Guitar"
    "Acoustic Guitar"
)

# like saying if 'https' not in line and ' by ' not in line

with open("../db_manager/timestamps/all-timestamps.txt", 'r') as file:
    for line in file:
        stripped = line.strip()
        if stripped and all(keyword not in stripped for keyword in excluded_keywords):
            print(stripped)
