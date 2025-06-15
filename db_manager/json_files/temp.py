import json

# List of U.S. states
states = [
    "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut",
    "Delaware", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa",
    "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan",
    "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada",
    "New Hampshire", "New Jersey", "New Mexico", "New York", "North Carolina",
    "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island",
    "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont",
    "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming"
]

# Load the original JSON
with open("artists.json", 'r', encoding='utf-8') as file:
    content = json.load(file)

# Update each artist with Country and Emoji
for artist in content.values():
    location = artist.get("Location", "")

    if any(state in location for state in states):
        artist["Country"] = "United States"
        artist["Emoji"] = "🇺🇸"
    elif ", England" in location:
        artist["Country"] = "England"
        artist["Emoji"] = "🇬🇧"
    else:
        artist["Country"] = ""
        artist["Emoji"] = ""

# Save updated JSON
with open("artists.json", 'w', encoding='utf-8') as file:
    json.dump(content, file, indent=4, ensure_ascii=False)
