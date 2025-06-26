# Original Livestream Directory
## [Link to Website](https://www.coreyheuvelsearch.com/)
This is a search tool I made for the Youtuber "Corey Heuvel" back in Summer 2022. Allows users to easily navigate thousands of livestream music performances. This website is currently being hosted on GitHub pages.

Here's Corey Heuvel talking about the Livestream Directory during one of his livestreams: [Youtube Link](https://www.youtube.com/live/bEBVkT9SWFY?feature=share&t=2937)

Note: I'm the "Michael L" being referred to, that's my Youtube username. The L doesn't stand for anything.

## Technologies Used
- Vanilla HTML, CSS, and JavaScript
- C# 
- JSON
- GitHub Pages
- Cloudflare
- Python
- SQLite

## Explanation of how this Works
- The frontend was built using **HTML**, **CSS**, and **JavaScript**. I did not use any framework for this project

- I used **C#** to make the algorithm. It was originally written in *[Python](https://github.com/MichaelT-178/Modern-Python-Files/blob/main/og_main_algorithm.py)* but I rewrote it in C# to improve code clarity and add new features. The purpose of the algorithm is to convert the db_manager/timestamps/[all-timestamps.txt](https://github.com/MichaelT-178/LivestreamDirectory/blob/main/db_manager/timestamps/all-timestamps.txt) file into the database/[song_list.json](https://github.com/MichaelT-178/LivestreamDirectory/blob/main/database/song_list.json) file. It basically turns the timestamps into a list of JSON song objects.

- **JSON** is the data format used in this project.

- **GitHub Pages** is where the website is being hosted.

- **Cloudflare** is how I configured the domain link for this site.

- **Python** is NOT directly used in the website. But I use it to convert the song_list.json file into a **SQLite** database. The **SQLite** database is just for viewing purposes and to query it using SQL. It's NOT being used by the website in anyway.

## This is what the output of running the algorithm looks like
<img src="pics/run-algorithm.jpg" width="350" height="350">

## Reviews
This website was met with a very positive response when posted on Corey Heuvel's [Patreon](https://www.patreon.com/posts/livestream-70578990?utm_medium=clipboard_copy&utm_source=copyLink&utm_campaign=postshare_fan&utm_content=web_share) page. Below are some of the reviews.

- *"That is purely amazing!!! Thanks Lord Michael!"*
- *"Really immense; thanks so much."*
- *"Holy wow, Michael, that is SUPER cool, thank you!"*

And one of my personal favorites from a Lenovo hiring manager:

- *"I use the S#%t out of that thing."*