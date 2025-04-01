import json
import re
from collections import defaultdict

def clean_text(text):
    # If the entire title is in parentheses (e.g., "(Nice Dream) by Radiohead"), leave it alone
    if re.match(r'^\([^)]+\)\s+by\s+', text, flags=re.IGNORECASE):
        return text

    no_parens = re.sub(r'\(([^)]*)\)', '', text)
    no_parens = ' '.join(no_parens.split())

    parts = re.split(r'\bby\b', no_parens, flags=re.IGNORECASE)

    if len(parts) <= 1:
        return no_parens

    cleaned = ' by'.join(parts[:-1]).strip()

    return cleaned


# Load repertoire
with open("repertoire.json", "r") as file:
    content = json.load(file)

# Load whitelist
with open("whitelist_duplicates.json", "r") as whitelist_file:
    whitelist_data = json.load(whitelist_file)

    whitelist_map = {
        k.lower(): set(vv.lower() for vv in v)
        for k, v in whitelist_data.get("whitelist_duplicates", {}).items()
    }

# Build cleaned map
cleaned_map = defaultdict(list)

for original in content:
    cleaned = clean_text(original).strip()

    if not cleaned:
        continue

    normalized = cleaned.lower()
    cleaned_map[normalized].append(original)

# Find duplicates excluding exact whitelist matches
duplicates = {}

for cleaned, originals in cleaned_map.items():
    if len(originals) <= 1:
        continue

    norm_originals = set(o.lower() for o in originals)

    if cleaned in whitelist_map:
        whitelist_originals = whitelist_map[cleaned]
        if norm_originals == whitelist_originals:
            continue  # Exact match to whitelist, skip it

    duplicates[cleaned] = originals

# Print results
if duplicates:
    print("Duplicates found (ignoring case):")
    for norm, originals in duplicates.items():
        print(f"\nCleaned form: {norm}")
        for orig in originals:
            print(f"  - {orig}")
else:
    print("No duplicates found.")
