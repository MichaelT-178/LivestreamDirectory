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

country_emoji_map = {
    "England": ("England", "🇬🇧"),
    "Germany": ("Germany", "🇩🇪"),
    "Spain": ("Spain", "🇪🇸"),
    "Australia": ("Australia", "🇦🇺"),
    "Canada": ("Canada", "🇨🇦"),
    "Israel": ("Israel", "🇮🇱"),
    "France": ("France", "🇫🇷"),
    "Hungary": ("Hungary", "🇭🇺"),
    "Italy": ("Italy", "🇮🇹"),
    "Earth": ("Earth", "🌎"),
    "Burma": ("Burma", "🇲🇲"),
    "Ireland": ("Ireland", "🇮🇪"),
    "Scotland": ("Scotland", "🏴󠁧󠁢󠁳󠁣󠁴󠁿"),
    "Wales": ("Wales", "🏴󠁧󠁢󠁷󠁬󠁳󠁿"),
    "North Ireland": ("Ireland", "🇮🇪"),
    "Jamaica": ("Jamaica", "🇯🇲"),
    "Belgium": ("Belgium", "🇧🇪"),
    "Ukraine": ("Ukraine", "🇺🇦"),
    "The Netherlands": ("The Netherlands", "🇳🇱"),
    "Norway": ("Norway", "🇳🇴"),
    "Paraguay": ("Paraguay", "🇵🇾"),
    "Japan": ("Japan", "🇯🇵"),
    "Egypt": ("Egypt", "🇪🇬"),
    "Puerto Rico": ("Puerto Rico", "🇵🇷"),
    "Cyprus": ("Cyprus", "🇨🇾")
}



with open("artists.json", 'r', encoding='utf-8') as file:
    content = json.load(file)



for artist in content.values():
    location = artist.get("Location", "")

    if any(state in location for state in states):
        artist["Country"] = "United States"
        artist["Emoji"] = "🇺🇸"
    else:
        matched = False
        for key, (country, emoji) in country_emoji_map.items():
            if f", {key}" in location:
                artist["Country"] = country
                artist["Emoji"] = emoji
                matched = True
                break
        if not matched:
            artist["Country"] = ""
            artist["Emoji"] = ""



with open("artists.json", 'w', encoding='utf-8') as file:
    json.dump(content, file, indent=4, ensure_ascii=False)
