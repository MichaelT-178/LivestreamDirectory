import base64
import os
from dotenv import load_dotenv
import requests
from termcolor import colored as c
import tkinter as tk
from PIL import Image, ImageTk
from io import BytesIO

load_dotenv()

CLIENT_ID = os.getenv('SPOTIFY_CLIENT_ID')
CLIENT_SECRET = os.getenv('SPOTIFY_CLIENT_SECRET')

class SpotifyApi:

    @staticmethod
    def get_access_token(client_id, client_secret):
        client_creds = f"{client_id}:{client_secret}"
        client_creds_b64 = base64.b64encode(client_creds.encode())

        token_url = "https://accounts.spotify.com/api/token"
        token_data = { "grant_type": "client_credentials" }
        token_headers = { "Authorization": f"Basic {client_creds_b64.decode()}" }

        r = requests.post(token_url, data=token_data, headers=token_headers)
        token_response_data = r.json()
        return token_response_data.get("access_token")
    
    @staticmethod
    def get_album_image_url(title, album, access_token):
        search_url = "https://api.spotify.com/v1/search"
        headers = {"Authorization": f"Bearer {access_token}"}

        # Try to find track within the specified album first
        query = f'track:"{title}" album:"{album}"'
        params = {"q": query, "type": "track", "limit": 1}
        r = requests.get(search_url, headers=headers, params=params)
        result = r.json()
        tracks = result.get('tracks', {}).get('items', [])

        if tracks:
            images = tracks[0]['album']['images']
            return images[0]['url'] if images else None

        # If no match, fallback to album search directly
        print(c(f"Track+album search failed. Trying album search only for '{album}'...", 'yellow'))

        album_params = {"q": f'album:"{album}"', "type": "album", "limit": 1}
        r = requests.get(search_url, headers=headers, params=album_params)
        album_result = r.json()
        albums = album_result.get('albums', {}).get('items', [])

        if not albums:
            return None

        images = albums[0]['images']
        return images[0]['url'] if images else None


    @staticmethod
    def get_artist_image_url(artist_name: str, access_token: str) -> str | None:
        search_url = "https://api.spotify.com/v1/search"
        headers = {"Authorization": f"Bearer {access_token}"}
        params = {
            "q": artist_name,
            "type": "artist",
            "limit": 1
        }

        r = requests.get(search_url, headers=headers, params=params)
        result = r.json()
        artists = result.get('artists', {}).get('items', [])

        if not artists:
            return None

        images = artists[0]['images']

        # Default to None if no images are available
        large_image = images[0]['url'] if len(images) > 0 else None

        return large_image




    @staticmethod
    def download_image(image_url: str, image_name: str) -> None:
        response = requests.get(image_url)

        if response.status_code == 200:
            with open(f"../../VueLivestreamDirectory/src/assets/AlbumPics/{image_name}.jpg", "wb") as f:
                f.write(response.content)

            print(c("Image downloaded successfully!", 'green'))
        else:
            print(f"Failed to download image. Status code: {response.status_code}")



    @staticmethod
    def decide_image(image_id: str, 
                    image_url: str, 
                    song_name: str, 
                    album_name: str, 
                    album_saved_image_name: str,
                    all_good: bool
                    ) -> None:
        
        def download_and_close():
            if not all_good:
                SpotifyApi.download_image(image_url, album_saved_image_name)

            root.destroy()

        def skip_and_close():
            print(f"Skipped downloading. Image ID: {image_id}")
            root.destroy()

        response = requests.get(image_url)
        
        if response.status_code != 200:
            print(f"Failed to load image. Status code: {response.status_code}")
            return

        # Create window
        root = tk.Tk()
        root.title("Download this image?")

        # Load the image from URL
        img_data = BytesIO(response.content)
        pil_image = Image.open(img_data)

        # Resize the image if it's too big
        max_size = (500, 500)
        pil_image.thumbnail(max_size)

        tk_image = ImageTk.PhotoImage(pil_image)

        # Display the image ID first
        id_label = tk.Label(root, text=f"Image ID: {image_id}", font=("Helvetica", 14, "bold"))
        id_label.pack(pady=(10, 5))

        # Display the image
        label = tk.Label(root, image=tk_image)
        label.pack(pady=(5, 5))

        # Display song, album, and saved image name
        song_label = tk.Label(root, text=f"Song: {song_name}", font=("Helvetica", 14))
        song_label.pack(pady=(2, 2))

        album_label = tk.Label(root, text=f"Album: {album_name}", font=("Helvetica", 14))
        album_label.pack(pady=(2, 2))

        saved_label = tk.Label(root, text=f"Saved As: {album_saved_image_name}.jpg", font=("Helvetica", 14))
        saved_label.pack(pady=(2, 10))

        # Button frame (buttons side-by-side)
        button_frame = tk.Frame(root)
        button_frame.pack(pady=20)

        # YES button
        yes_button = tk.Button(button_frame, text="YES", command=download_and_close,
                            font=("Helvetica", 14), fg="green", width=10, height=2)
        yes_button.pack(side=tk.LEFT, padx=20)

        # NO button
        no_button = tk.Button(button_frame, text="NO", command=skip_and_close,
                            font=("Helvetica", 14), fg="red", width=10, height=2)
        no_button.pack(side=tk.RIGHT, padx=20)

        root.mainloop()
        
    
    @staticmethod
    def process_album_image(song: dict) -> None:

        access_token = SpotifyApi.get_access_token(CLIENT_ID, CLIENT_SECRET)

        title = song.get("title")
        album = song.get("album")
        cleaned_album = song.get("cleanedAlbum")
        song_id = song.get("id")

        if not title or not album or not cleaned_album:
            return

        image_path = f"../../VueLivestreamDirectory/src/assets/AlbumPics/{cleaned_album}.jpg"

        if os.path.exists(image_path):
            print(c(f"Image already exists: {cleaned_album}.jpg", 'blue'))
            return

        print(c(f"Searching image for '{title}' from album '{album}'...", 'cyan'))

        image_url = SpotifyApi.get_album_image_url(title, album, access_token)

        if not image_url:
            print(c("No image found for this song.", 'yellow'))
            return
        
        SpotifyApi.decide_image(
            image_id=str(song_id),
            image_url=image_url,
            song_name=title,
            album_name=album,
            album_saved_image_name=cleaned_album,
            all_good=False
        )
        

    @staticmethod
    def process_artist_image(artist: dict) -> None:
        access_token = SpotifyApi.get_access_token(CLIENT_ID, CLIENT_SECRET)

        name = artist.get("name")
        cleaned_name = artist.get("cleanedName")
        artist_id = artist.get("id")

        if not name or not cleaned_name:
            return

        image_path = f"../../VueLivestreamDirectory/src/assets/ArtistPics/{cleaned_name}.jpg"

        if os.path.exists(image_path):
            print(c(f"Image already exists: {cleaned_name}.jpg", 'blue'))
            return

        print(c(f"Searching image for artist '{name}'...", 'cyan'))

        image_url = SpotifyApi.get_artist_image_url(name, access_token)

        if not image_url:
            print(c("No image found for this artist.", 'yellow'))
            return

        SpotifyApi.decide_image(
            image_id=str(artist_id),
            image_url=image_url,
            song_name=name,  # Reusing song_name for label. It's actually the artists name
            album_name="(Artist)",  # Just a placeholder since we're not using albums here
            album_saved_image_name=cleaned_name,
            all_good=False
        )
