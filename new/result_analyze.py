import json
import re





def clean_line(line: str) -> str:
    
    REMOVE_KEYS = {
        "12-String", "BDC", "BH", "BHG", "BSG", "BSGI", "Blues Slide", "Classical Guitar",
        "DM75", "DX1R", "Electric Song", "Electric riff", "FBD", "FBG", "FG", "FOB", "FOSG",
        "FSD", "FV2", "FVD", "GFCHN", "GLTC", "GMLHB", "GMLN", "GMNC", "GPPCB", "GPRG", "GRSG",
        "GSDG", "LPE", "M15M", "MDT", "MFF", "MHD", "Mandolin", "NBBU", "NBDA", "NSCG", "NSPBU",
        "NST", "NSTCW", "OOM", "OOMV1", "SAS", "SD22", "SGI", "SOM"
    }

    match = re.search(r'\(([^)]+)\)', line)

    if not match:
        return ''
    
    content = match.group(1)
    parts = [part.strip() for part in content.split('/')]
    filtered = [p for p in parts if p not in REMOVE_KEYS]
    
    return f"({ '/'.join(filtered) })" if filtered else ''



def remove_all_keys_from_song(line):
    with open("../db_manager/json_files/keys_to_keep.json", 'r') as file:
        content = json.load(file)
        song_keys = set(content["song_keys"])

    def replacer(match):
        full_match = match.group(0)
        return full_match if full_match in song_keys else ''

    return re.sub(r'\([^()]+\)', replacer, line).replace('  ', ' ').strip()



with open("instrument_results.txt", "r") as file:
    for line in file:
        if "- " in line:
            # print(line.strip())
            print(remove_all_keys_from_song(line).strip())
            print("\n")






# unique_keys = set()

# with open("instrument_results.txt", "r") as file:
#     for line in file:
#         if "CARDBOARD" in line:
#             start = line.find("(")
#             end = line.find(")")
#             if start != -1 and end != -1:
#                 content = line[start+1:end]
#                 parts = content.split("/")
#                 for part in parts:
#                     part = part.strip()
#                     if part:
#                         unique_keys.add(part)

# # Convert to list if needed
# key_list = list(unique_keys)

# # Print the result
# print("Key List:")
# for key in sorted(key_list):
#     print(key)


# print(len(key_list))



# Key List:
# 12-String
# BDC
# BH
# BHG
# BSG
# BSGI
# Blues Slide
# Classical Guitar
# DM75
# DX1R
# Electric Song
# Electric riff
# FBD
# FBG
# FG
# FOB
# FOSG
# FSD
# FV2
# FVD
# GFCHN
# GLTC
# GMLHB
# GMLN
# GMNC
# GPPCB
# GPRG
# GRSG
# GSDG
# LPE
# M15M
# MDT
# MFF
# MHD
# Mandolin
# NBBU
# NBDA
# NSCG
# NSPBU
# NST
# NSTCW
# OOM
# OOMV1
# SAS
# SD22
# SGI
# SOM