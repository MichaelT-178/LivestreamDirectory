import json
import re
from collections import OrderedDict
import unicodedata

def normalize_to_ascii(text):
    """
    Turn ascii chars into standard English chars
    
    Ex. 
    original -> Pronounced 'Lĕh-'nérd 'Skin-'nérd
    normalized -> Pronounced 'Leh-'nerd 'Skin-'nerd

    return: String with standard english chars instead of ascii chars.
    """
    normalized = unicodedata.normalize('NFKD', text)
    ascii_text = normalized.encode('ascii', 'ignore').decode('ascii')
    return ascii_text

def prepare_text(text):
    """
    Replace ' with "" so you don't get weird names like 
    """
    return text.replace("'", "")

def remove_non_alphanumeric(text):
    """
    Replace any sequence of non-alphanumeric characters with a single space.

    original -> Pronounced 'Leh-'nerd 'Skin-'nerd
    normalized -> Pronounced Leh nerd Skin nerd
    """
    cleaned = re.sub(r'[^a-zA-Z0-9]+', ' ', text)
    return cleaned.strip()

def collapse_spaces(text):
    """
    Replace multiple spaces with a single dash.

    """
    return re.sub(r'\s+', '-', text).strip()

def lowercase_text(text):
    """
    Convert all characters in the text to lowercase.
    """
    return text.lower()

def clean_text(text):
    # Starts -> Pronounced 'Lĕh-'nérd 'Skin-'nérd
    
    # Can't Stop -> Cant Stop
    prepared_text = prepare_text(text)

    # Pronounced 'Le'h-'nerd 'Skin-'nerd
    ascii_text = normalize_to_ascii(prepared_text)

    # Pronounced 'Leh-'nerd 'Skin-'nerd
    alphanumeric = remove_non_alphanumeric(ascii_text)

    # Pronounced-Leh-nerd-Skin-nerd
    spaced_text = collapse_spaces(alphanumeric)

    # pronounced-leh-nerd-skin-nerd
    clean_text = lowercase_text(spaced_text)

    return clean_text



"""
Main Script
"""

# Load existing albums.json and preserve key order
with open('albums.json', 'r', encoding='utf-8') as f:
    raw = json.load(f, object_pairs_hook=OrderedDict)

updated_albums = []
for album in raw["albums"]:
    new_album = OrderedDict()
    for key, value in album.items():
        new_album[key] = value
        if key == "Song":
            cleaned = clean_text(value)
            new_album["CleanedSong"] = cleaned
    updated_albums.append(new_album)

# Overwrite albums.json with new structure
with open('albums.json', 'w', encoding='utf-8') as f:
    json.dump({"albums": updated_albums}, f, indent=4, ensure_ascii=False)

print("Inserted CleanedSong after Song in albums.json.")
