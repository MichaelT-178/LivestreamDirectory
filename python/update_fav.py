import json
from collections import OrderedDict


with open("../db_manager/json_files/fav_covers.json", "r") as file:
    content = json.load(file)


updated_covers = []

for idx, cover in enumerate(content["FavoriteCovers"], start=1):
    updated_cover = OrderedDict()

    updated_cover["id"] = idx
    updated_cover["Song"] = cover["Song"]
    updated_cover["Artist"] = cover["Artist"]
    updated_cover["Appearance"] = cover["Appearance"]
    updated_cover["Link"] = cover["Link"]
    updated_cover["AlbumImage"] = cover["AlbumImage"]
    updated_covers.append(updated_cover)


content["FavoriteCovers"] = updated_covers


with open("../db_manager/json_files/fav_covers.json", "w") as outfile:
    json.dump(content, outfile, indent=4, ensure_ascii=False)
