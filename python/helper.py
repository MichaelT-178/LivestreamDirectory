import os
from PIL import Image

directory = '../pics'

for filename in os.listdir(directory):
    if filename.endswith(".jpg"):
        with Image.open(os.path.join(directory, filename)) as img:
            width, height = img.size
            print(f"Image: {filename} | Width: {width}px | Height: {height}px")
