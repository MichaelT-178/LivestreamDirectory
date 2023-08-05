"""
This program reads the all-timestamps.txt file and writes
the song information to the song_list.json file in json format.
Performs error and format checking before writing to file.
"""
import threading as thread
import json
from time import time as the_time
from termcolor import colored # https://pypi.org/project/termcolor/
import webbrowser as Google
import os
from os.path import exists

print(colored("REMEBER TO ADD THE YOUTUBE LINK", "magenta"))
open_timestamps = input("Do you want to open the \"all-timestamps.txt\" file? : ")

if open_timestamps.upper() in ['Y', "YES"]:
    os.system(f'open -a "Visual Studio Code" ../timestamps/all-timestamps.txt')
    exit()
    
start_time = the_time()
print("\nPlease be patient. This will take a couple of seconds...")

def write():
    print("Writing to file...")

def wait():
    print("Python for loops am I right? 😅")

#Had to put the print statements in a function for some reason and leave out parentheses when calling?
t = thread.Timer(1.0, write)
t2 = thread.Timer(5.5, wait)
t.start()
t2.start()

def youtube_links(video_id, time):
    t = [int(x) for x in time.split(":")]
    seconds = t[1] + (t[0] * 60) if (len(t) == 2) else t[2] + (t[1] * 60) + (t[0] * 3600)

    if "share" in video_id: # Youtube changed their livestream links. 2/3/23
        return f"https://www.youtube.com/live/{video_id}&t={seconds} , " 

    return f"https://youtu.be/{video_id}?t={seconds} , " 

json_repeat = open("../json_files/no_repeats.json", 'r')
json_keys = open("../json_files/only_with_keys.json", 'r')
all_the_artists = open("../json_files/artists.json", 'r')

no_repeats = json.load(json_repeat)
only_keys = json.load(json_keys)
artists_played = json.load(all_the_artists)

def format_list(the_list):
    string = '['

    for item in the_list:
        string += "\n\t\"" + item + "\","

    string = string[:-1]
    string += '\n]'

    return string

def remove_from_no_repeats(no_repeats_list, item):
    no_repeats_list.remove(item)

    with open("../json_files/no_repeats.json", 'w') as f:
        f.write(format_list(no_repeats_list))

    return no_repeats_list

def remove_no_repeat_add_keys(no_repeats_list, item, only_keys_list, line):
    
    remove_from_no_repeats(no_repeats_list, item)
    key_list = line.split("%!")
    only_keys_list.append(key_list[0])
    only_keys_list.append(key_list[1])

    with open("../json_files/only_with_keys.json", 'w') as f:
        f.write(format_list(only_keys_list))

    return only_keys_list

def add_no_repeats(no_repeats_list, item):
    no_repeats_list.append(item)

    with open("../json_files/no_repeats.json", 'w') as f:
        f.write(format_list(no_repeats_list))

    return no_repeats_list

songs = []
all_songs = []
lower_songs = []

cap_error_found = False
line_num = 0
with open("../timestamps/all-timestamps.txt", "r") as afile:
    for aline in afile:
        if (len(aline.rsplit(" by ", 1)) == 2):
            time_split = aline.split()
            line = aline.replace(time_split[0], "")
            split_line = line.rsplit(" by ", 1)
            
            #Checks for capitalization error
            if (split_line[0].strip().replace("’", "'").replace("’", "'").replace("‘", "'") not in songs and 
                split_line[0].strip().replace("’", "'").replace("’", "'").replace("‘", "'").lower() in lower_songs):
                print(colored("CAPITALIZATION ERROR: ", 'red') + "\"" + split_line[0].strip().replace("’", "'").replace("‘", "'"), end="")
                print("\" on Line " + str(line_num + 1))
                cap_error_found = True
    

            if split_line[0].strip().replace("’", "'").replace("‘", "'") not in songs:
                songs.append(split_line[0].strip().replace("’", "'").replace("‘", "'"))
                lower_songs.append(split_line[0].strip().replace("’", "'").replace("‘", "'").lower())

            all_songs.append(split_line[0].strip())
        line_num += 1

if cap_error_found:
    print("\nGo fix capitalization and rerun\n")
    os.system(f'open -a "Visual Studio Code" ../timestamps/all-timestamps.txt')
    os._exit(0)

song_info = "{"
song_info += '\n	"songs":['

###############################################################################
matches = []

for i in range(len(all_songs)):
    for j in range(i + 1, len(all_songs)):

        s1 = all_songs[i].replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "")
        s2 = all_songs[j].replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "")

        if (s1.strip() == s2.strip()):
            matches.append(s1.strip())

for the_song in all_songs:
    if ("(Electric riff)" in the_song or "(Classical Guitar)" in the_song or
       "(Mandolin)" in the_song or "(Electric Song)" in the_song or
       "(12/twelve String)" in the_song or "(Partial)" in the_song):
       the_temp = the_song.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "")

       if the_temp.strip() not in matches and the_song not in no_repeats and the_song not in only_keys:
           print("\n" + colored("Add to No_Repeats", 'red') + ": " + the_song.strip() + "\n")
           no_repeats = add_no_repeats(no_repeats, the_song.strip())


###############################################################################
the_duplicates = []
for repeat_song in no_repeats:
    for th_song in all_songs:
        song1 = th_song.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "")
        r1 = repeat_song.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "")

        if ("(Electric riff)" in th_song or "(Classical Guitar)" in th_song or
           "(Mandolin)" in th_song or "(Electric Song)" in th_song or
           "(12/twelve String)" in th_song or  "(Partial)" in th_song):
           if r1.strip() == song1.strip():
               if repeat_song.strip() in the_duplicates:
                   both = repeat_song.strip() + "%!" + th_song.strip()
                   print("\n" + colored("Add to only_with_keys", 'red') + ": " + repeat_song.strip())
                   print(colored("Add to only_with_keys", 'red') + ": " + th_song.strip())

                   only_keys = remove_no_repeat_add_keys(no_repeats, repeat_song.strip(), only_keys, both)
               the_duplicates.append(repeat_song.strip())
###############################################################################          

for a_song in songs:
    a_song = a_song.strip()

    if a_song + " (Electric riff)" in no_repeats:
        full_song = a_song + " (Electric riff)"
        print("\n" + colored("Remove from no_repeats", 'red') + ": " + full_song)
        no_repeats = remove_from_no_repeats(no_repeats, full_song)

    elif a_song + " (Classical Guitar)" in no_repeats:
        full_song = a_song + " (Classical Guitar)"
        print("\n" + colored("Remove from no_repeats", 'red') + ": " + full_song)
        no_repeats = remove_from_no_repeats(no_repeats, full_song)

    elif a_song + " (Mandolin)" in no_repeats:
        full_song = a_song + " (Mandolin)"
        print("\n" + colored("Remove from no_repeats", 'red') + ": " + full_song)
        no_repeats = remove_from_no_repeats(no_repeats, full_song)

    elif a_song + " (Electric Song)" in no_repeats:
        full_song = a_song + " (Electric Song)"
        print("\n" + colored("Remove from no_repeats", 'red') + ": " + full_song)
        no_repeats = remove_from_no_repeats(no_repeats, full_song)

    elif a_song + " (12/twelve String)" in no_repeats:
        full_song = a_song + " (12/twelve String)"
        print("\n" + colored("Remove from no_repeats", 'red') + ": " + full_song)
        no_repeats = remove_from_no_repeats(no_repeats, full_song)

    elif a_song.strip() + " (Partial)" in no_repeats:
        full_song = a_song + " (Partial)"
        print("\n" + colored("Remove from no_repeats", 'red') + ": " + full_song)
        no_repeats = remove_from_no_repeats(no_repeats, full_song)

    else:
        pass

def replace_nth(full_str, old_str, new_str, occurence):
    arr = full_str.split(old_str)
    part1 = old_str.join(arr[:occurence])
    part2 = old_str.join(arr[occurence:])
    
    return part1 + new_str + part2

"""
Main part of the program
"""
for song in songs:
    title = song.replace("’", "'").replace("‘", "'")
    appearances = ""
    instruments = ""
    artist = ""
    time = ""
    links = ""

    the_songs = []
    the_artists = []

    with open("../timestamps/all-timestamps.txt", "r") as file:
        #file.readline() #Ignores first line of file
        for line in file:

            line = line.replace("’", "'").replace("‘", "'")

            if "Livestream" in line:
                ls_num = line.strip().replace("Livestream ", "")

            if "solo video" in line.lower():
                ls_num = line.strip()

            if "https" in line:
                link = line.strip().split("/")[-1] #was 3


            if (len(line.rsplit(" by ", 1)) == 2):
                split_line = line.rsplit(" by ", 1)

                time_split = line.split()

                real_title = split_line[0].replace(time_split[0], "").strip()

                if title not in no_repeats and title not in only_keys:
                    real_title = real_title.replace("(Electric riff)", "").replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").replace("(Partial)", "")
                
                title = title.replace(" (Classical Guitar)", "") if ("Fugue" in title) else title
                title = title.replace(" (Classical Guitar)", "") if ("1006a" in title) else title
                
                if title.lower().strip() == real_title.lower().strip(): 
                    artist = split_line[1].strip() if (len(split_line[1]) > len(artist)) else artist

                    if ("(Electric riff)" in line): 
                        appearances += ("Livestream " + ls_num + " (Electric riff),") if ("so" not in ls_num.lower()) else (ls_num.strip() + " (Electric riff),")

                    elif ("(Electric Song)" in line):
                        appearances += ("Livestream " + ls_num + " (Electric Song),") if ("so" not in ls_num.lower()) else (ls_num.strip() + " (Electric Song),")

                    elif ("(Classical Guitar)" in line):
                        appearances += ("Livestream " + ls_num + " (Classical Guitar),") if ("so" not in ls_num.lower()) else (ls_num.strip() + " (Classical Guitar),")

                    elif ("(Mandolin)" in line):
                        appearances += ("Livestream " + ls_num + " (Mandolin),") if ("so" not in ls_num.lower()) else (ls_num.strip() + " (Mandolin),")

                    elif ("(Partial)" in line):
                        appearances += ("Livestream " + ls_num + " (Partial),") if ("so" not in ls_num.lower()) else (ls_num.strip() + " (Partial),")

                    else:
                        appearances += ("Livestream " + ls_num + ",") if ("solo video" not in ls_num.lower()) else (ls_num.strip() + ",")
                    
                    try:
                        if (int(ls_num) == 136 and "Chris Whitley" == artist):
                            appearances = appearances.replace("Livestream 136,", "Livestream 136 (w/ Blues Slide),")
                    except ValueError:
                        pass

                    try:
                        links += youtube_links(link, time_split[0])
                    except ValueError:
                        print(colored("YOUTUBE LINK DIDN'T WORK RIGHT",'red'))

                    instruments += "Electric Guitar, " if ("(Electric riff)" in line and instruments == "") else ""
                    instruments += " Electric Guitar, " if ("(Electric riff)" in line and "Electric Guitar" not in instruments) else ""
                    
                    instruments += "Electric Guitar, " if ("Electric Riff Session #" in line and instruments == "") else ""
                    instruments += " Electric Guitar, " if ("Electric Riff Session #" in line and "Electric Guitar" not in instruments) else ""
                    
                    instruments += "Electric Guitar, " if ("(Electric Song)" in line and instruments == "") else ""
                    instruments += " Electric Guitar, " if ("(Electric Song)" in line and "Electric Guitar" not in instruments) else ""
                   
                    instruments += "Classical Guitar, " if ("(Classical Guitar)" in line and instruments == "") else ""
                    instruments += " Classical Guitar, " if ("(Classical Guitar)" in line and "Classical Guitar" not in instruments) else ""

                    instruments += "Mandolin, " if ("(Mandolin)"  in line and instruments == "") else ""
                    instruments += " Mandolin, " if ("(Mandolin)"  in line and "Mandolin" not in instruments) else ""

                    instruments += "Harmonica, " if ("Rein"  in line and instruments == "") else ""
                    instruments += " Harmonica, " if ("Rein"  in line and "Harmonica" not in instruments) else ""
            
                    if ("(Electric riff)" not in line and "(Electric Song)" not in line
                        and "(Classical Guitar)" not in line and "(Mandolin)" not in line
                        and "Acoustic Guitar" not in instruments and "(H)" not in line
                        and "Electric Riff Session #" not in line):
                        instruments += "Acoustic Guitar, "

                    if ("Forget Her" in line): instruments += "Electric Guitar, " 
                    instruments += " Blues Slide " if ("Blues Slide" in title or "Blues Slide" in appearances) else ""
 
        if (appearances.strip() != ""):
            song_info += "\n		{"
            title = title.replace("(I)", "").replace("(H)", "").strip() #For Incubus wish you were Here and Harmonica only
            title = "Intro “Out of the Mist”" if ("intro “out of the mist”" in title.lower()) else title
            other = ""
            other += title + ", " if ("“" in title or "”" in title) else ""

            r_slash = r"\""
            title = title.replace("“", r_slash).replace("”", r_slash)

            if not title.isascii() and "”" not in title:
                title = title.replace("“", r_slash).replace("”", r_slash)

            the_title = title.replace("(Classical Guitar)", "").replace("(Mandolin)", "").replace("(Electric Song)", "").replace("(12/twelve String)","").strip()

            song_info += "\n			\"Title\": \"" + the_title + "\","
        
            all_artists = artist.replace("/", ",").split(",") if ("Yusuf" not in artist and "," not in artist or "Eurythmics" in artist and "AC/DC" not in artist) else artist.split("&&&")
            artist = all_artists[0]

            artist = "AC/DC" if ("AC" in artist) else artist

            song_info += "\n			\"Artist\": \"" + artist + "\","

            other_artists = ""

            for i in range(1, len(all_artists)):
                other_artists += all_artists[i] + ", "

            other_artists = "" if (artist == "AC/DC") else other_artists

            song_info += "\n			\"Other_Artists\": \"" + other_artists[:-2].strip().replace("  ", " ") + "\","

    
            if "Machine Gun" in title: 
                appearances = replace_nth(appearances," (Electric Song)", "", 2)
            
            if "Led Boots" in title:
                appearances = appearances.replace(" 50 (Electric Song)", " 50 (Electric riff)")

            song_info += "\n			\"Appearances\": \"" + appearances[:-1] + "\","

            other += title.replace("É", "E").replace("í", "i").replace("é", "e").replace("á","a").replace("à", "a").replace("Á", "A").replace("ü", "u") + ", " if (not title.isascii()) else ""
            other += title.replace("'","").replace(r_slash,"").replace(" & ", " and ").replace("-", " ").replace(",", "").replace(".","") + ", " if ("'" in title or "\"" in title or "&" in title or "-" in title or "," in title) else ""

            other += title.replace(" and ", " & ") + ", " if ("and" in title) else ""
            other += title.replace("'", "’").replace(".", "") + ", " if ("'" in title or "." in title) else ""
            other += title.replace(" you ", " u ").replace(",", "").replace("'", "") + ", " if ("you" in title.lower()) else ""

            other += artist.replace("-", " ").replace(",", " ") + ", " if ("-" in artist or "," in artist) else ""
            
            other += "Pink, " if (artist == "P!nk") else ""
            other += "The Red Hot Chili Peppers, " if ("Red Hot Chili" in artist) else ""
            other += "Neil Young, " if ("Young" in artist and artist != "Neil Young") else ""
            other += "Star bird, " if ("Starbird" in title) else ""
            other += "The Allman Brothers, " if ("Allman" in artist) else ""
            other += "Rap, " if ("Nelly" in artist or "Flo Rida" in artist) else ""
            other += "The Extreme, " if ("Extreme" == artist) else ""
            other += title.replace("Grey", "Gray") + ", " if ("Grey" in title) else ""
            other += artist.replace("Grey", "Gray") + ", " if ("Grey" in artist) else ""
            other += "I Am A Man of Constant Sorrow, " if ("Man Of Constant Sorrow" in title) else ""
            other += "Vincent (Starry, Starry Night), " if ("Vincent" == title) else ""
            other += "Happy Christmas, " if ("Xmas" in title) else ""
            other += "Merry Christmas, " if ("Xmas" in title) else ""
            other += "Simon and Garfunkel, " if ("Simon & Gar" in artist) else ""
            other += "Bubble, " if ("Bublé" in artist) else ""
            other += "Buble, " if ("Bublé" in artist) else ""
            
            if ("Simon & Gar" in artist): other += "Paul Simon, "
            if (artist == "AC/DC"): other += "ACDC, " 
            if (artist == "Dire Straits"): "The Dire Straits, " 
            if (artist == "Joe Walsh"): other += "The Eagles, " 
            if (artist == "Elliott Smith"): other += "Elliot , "
            if (title == "Trouble So Hard"): other += "Natural Blues by Moby, "
            if (title == "Natural Blues"): other += "Trouble So Hard by Vera Hall, "

            other += artist.strip().replace(".", "").replace("'", "").replace("’", "").replace("‘", "'") + ", " if ("." in artist or "'" in artist or "’" in artist) else ""
            other += artist.replace('É', 'E').replace('í', 'i').replace('é','e').replace('á','a').replace("ü", "u") + ", " if ('É' in artist or 'í' in artist or 'é' in artist or 'á' in artist or "ü" in artist) else ""

            song_info += "\n			\"Other\": \"" + other[:-2].replace("  ", " ") + "\","

            instrument = instruments.strip()[:-1]

            song_info += "\n			\"Instruments\": \"" + instrument + "\","

            has_accent = ['Édith Piaf','Agustín Barrios Mangoré','Beyoncé, JAY-Z','Francisco Tárrega']

            if not artist.isascii() and artist.strip() not in has_accent:
                print(colored("HAS ACCENT ", "cyan") + artist + ". Needs to be added to list manually!")


            artist = artist.replace('É', 'E').replace('í', 'i').replace('é','e').replace('á','a').replace("ü", "u").replace("/", ":")
            
            if artist.replace(":", "/").strip() not in artists_played:
                print(colored("\nNEW ARTIST", 'magenta') + " \"" + artist + "\" written to file!")

                artists_played.append(artist.strip())
                
                with open("../json_files/artists.json", 'w') as wf:
                    wf.write(format_list(artists_played))

                pic_question = input("Do you want to exit the program to find a picture? : ")

                if pic_question.upper() in ["Y", "YES"]:
                    exit()

            artist_pic = artist.strip().replace(".", "").replace("'", "").replace("’", "").replace("‘", "'")
            
            image = f"../pics/{artist_pic}.jpg"

            os.chdir('../')
            file_exists = exists(image)

            if not file_exists:
                print(colored("This artist needs a picture", 'red') + ": " + artist_pic)
                print()
                print(colored("ADD THIS ARTIST TO THE API SERVER", 'cyan') + ": " + artist_pic)
                
            os.chdir('../format/python')

            song_info += "\n			\"Image\": \"" + image[1:] + "\","
            song_info += "\n			\"Links\": \"" + links[:-3] + "\"" 
            song_info += "\n		},"


song_info = song_info[:-1]
song_info += "\n	]"
song_info += "\n}"

os.chdir('../')

with open('../json/database/song_list.json', 'w') as f:
    f.write(song_info)

json_repeat.close()
json_keys.close()
all_the_artists.close()

song_list = []

with open('../json/database/song_list.json', 'r') as read_songs:
    the_dict = json.load(read_songs)

    for song in the_dict['songs']:
        song_list.append(song["Title"] + " by " + song["Artist"])

letter = "A"
alphabet_list = []

for a_song in sorted(song_list):
    if (a_song[0].upper() != letter and len(alphabet_list) > 0):
        letter = a_song[0].upper()
        alphabet_list.append("")

    r_slash = r"\""
    alphabet_list.append(a_song.replace('"', r_slash))

with open("json_files/repertoire.json", 'w') as wfile:
    wfile.write(format_list(alphabet_list))

print("File " + colored("successfully","green") + " written to!")
num = round(the_time() - start_time, 2)
print(f"Program took {num} seconds to run.")

question = input("\nDo you want to open the \"song_list.json\" file? : ")
print()

os.chdir('../')

if question.upper().strip() in ["Y", "YES"]:
    os.system(f'open -a "Visual Studio Code" json/database/song_list.json')

open_repo = input("Do you want to open the Github Repository? : ")

if open_repo.upper() in ["Y", "YES"]:
    Google.open("https://github.com/MichaelT-178/LivestreamDirectory")
    
#Commands to add to Github
os.system('git add .')
os.system('git commit -m "Adding files"')
os.system('git push')