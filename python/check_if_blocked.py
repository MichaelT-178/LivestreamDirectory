import json
from pytube import YouTube
from googleapiclient.discovery import build
from termcolor import colored as c


"""
Countries where the videos are blocked

['AS', 'GU', 'MP', 'PR', 'UM', 'US', 'VI']

AS: American Samoa
GU: Guam
MP: Northern Mariana Islands
PR: Puerto Rico
UM: U.S. Minor Outlying Islands
US: United States
VI: U.S. Virgin Islands
"""


def get_key():
    with open('creds.json', 'r') as file:
        content = json.load(file)
        return content["key"]
    

def get_youtube_title(url):
    yt = YouTube(url)
    return yt.title

API_KEY = get_key()



def get_video_id_from_url(url):
    try:
        yt = YouTube(url)
        return yt.video_id
    except Exception as e:
        return None



def check_video_availability_by_url(url):

    video_id = get_video_id_from_url(url)

    if not video_id:
        return { "msg" : "Invalid YouTube URL or video not found", "blocked": True }
    
    youtube = build('youtube', 'v3', developerKey=API_KEY)
    
    request = youtube.videos().list(
        part="contentDetails",
        id=video_id
    )

    response = request.execute()
    
    if 'items' in response and len(response['items']) > 0:

        content_details = response['items'][0]['contentDetails']

        if 'regionRestriction' in content_details:
            restrictions = content_details['regionRestriction']


            if 'blocked' in restrictions:

                if len(restrictions['blocked']) == 1 and "RU" in restrictions['blocked']:
                    return { "msg": f"Video is only blocked in Russia", "blocked": False }
                
                return { "msg": f"Video is blocked in these countries: {restrictions['blocked']}", "blocked": True }
            
            else:
                return { "msg": "Video available in your country", "blocked": False }
            

        else:
            return { "msg": "No regional restrictions", "blocked": False }
        
    else:
        return { "msg": "Video not found", "blocked": True }





with open("blocked.json", 'r') as file:
    content = json.load(file)


    blocked = content["blocked"]
    not_blocked = content["not_blocked"]
    
    
    print("Should be blocked")
    print("{:<70} | {:<55}".format("Status", "Real Title"))

    for video in blocked:
        response = check_video_availability_by_url(video["link"])
        message = response["msg"]
        is_blocked = response["blocked"]
        
        if is_blocked:
            status = "{:<70}".format(f"{video['title']} is blocked and in the right place!")
            real_title = "{:<55}".format(get_youtube_title(video["link"]))

            status_colored = c(status, 'green')
            real_title_colored = c(real_title, 'blue')

            print(f"{status_colored} | {real_title_colored}")
        else:
            status = "{:<70}".format(f"{video['title']} is NOT blocked and in the wrong place!")
            real_title = "{:<55}".format(get_youtube_title(video["link"]))

            print(c(status, 'red'), end="")
            print(f" ({c(real_title, 'blue')})")
            print(message)

    print("\n\nShould NOT be blocked")
    print("{:<70} | {:<55}".format("Status", "Real Title"))


    for video in not_blocked:
        response = check_video_availability_by_url(video["link"])
        message = response["msg"]
        is_blocked = response["blocked"]

        if not is_blocked:
            status = "{:<70}".format(f"{video['title']} is NOT blocked and in the right place!")
            real_title = "{:<55}".format(get_youtube_title(video["link"]))

            status_colored = c(status, 'green')
            real_title_colored = c(real_title, 'blue')

            print(f"{status_colored} | {real_title_colored}")
        else:
            status = "{:<70}".format(f"{video['title']} is blocked and in the wrong place!")
            real_title = "{:<55}".format(get_youtube_title(video["link"]))

            print()
            print(c(status, 'red'), end="")
            print(f" ({c(real_title, 'blue')})")
            print(message)
            print()

