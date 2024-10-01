import os
from PIL import Image


directory = '../../pics'
output_directory = '../../pics'

os.makedirs(output_directory, exist_ok=True)


def resize_image(image, max_width, max_height):
    width, height = image.size
    if width > max_width or height > max_height:
       
        scaling_factor = min(max_width / width, max_height / height)
        new_width = int(width * scaling_factor)
        new_height = int(height * scaling_factor)
        return image.resize((new_width, new_height), Image.Resampling.LANCZOS)
    return image

for filename in os.listdir(directory):
    if filename.endswith(".jpg"):
        with Image.open(os.path.join(directory, filename)) as img:
            if img.mode == 'RGBA':
                img = img.convert('RGB')
            resized_img = resize_image(img, 1000, 1000)
            resized_img.save(os.path.join(output_directory, filename))
            print(f"Resized and saved: {filename}")

print("All images resized and saved successfully.")
