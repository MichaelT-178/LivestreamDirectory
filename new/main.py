from spotify_images import (
    CLIENT_ID,
    CLIENT_SECRET,
    get_access_token, 
    get_album_image_url,
    download_image,
    decide_image,
    get_artist_image_url
)

from c_sharp_methods import CSharpMethods



csharp_methods = CSharpMethods()

artist, count = csharp_methods.get_all_artists()
# artist, count = csharp_methods.list_image_files()

for artist in artist:
    print(artist)

print(count)
exit(0)








access_token = get_access_token(CLIENT_ID, CLIENT_SECRET)
image_url = get_album_image_url("(Don't Fear) The Reaper", "Agents Of Fortune", access_token)
# image_url = get_artist_image_url("Led Zeppelin", access_token)
print(image_url)




# starting id 
# start at image id 1

#CHANGE THIS 
image_id = 1

# if access-of-fortune.jpg already exists in album folder, all_good is true
all_good = False


decide_image(
    image_id,
    image_url, 
    "(Don't Fear) The Reaper",
    "Agents Of Fortune",
    "agents-of-fortune",
    all_good
)




# download_image(image, "blue lagoon")