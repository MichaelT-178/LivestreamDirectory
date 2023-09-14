
from termcolor import colored as c

def are_files_equal(file1, file2):
    # Open both files in binary mode
    with open(file1, 'rb') as f1, open(file2, 'rb') as f2:
        # Read and compare the content byte by byte
        while True:
            byte1 = f1.read(1)
            byte2 = f2.read(1)
            if byte1 != byte2:
                return False
            if not byte1:
                break  # Reached the end of both files

    return True

# Usage example
file1 = "/Users/michaeltotaro/LivestreamDirectory/database/og_song_list.json"
file2 = "/Users/michaeltotaro/LivestreamDirectory/database/song_list.json"

if are_files_equal(file1, file2):
    print("SONG_LIST EQUAL: YES")
else:
    print(c("The files are not equivalent.", 'red'))

file1Content = []

with open(file1, 'r') as file1r:
    for line in file1r:
        file1Content.append(line)

file2Content = []
with open(file2, 'r') as file2r:
    for line in file2r: 
        file2Content.append(line)

for i in range(len(file1Content)):
    if file1Content[i] != file2Content[i]:
        print()
        print()
        print(f"1. {file1Content[i]}")
        print(f"2. {file2Content[i]}")

file1 = "/Users/michaeltotaro/LivestreamDirectory/db_manager/json_files/artists.json"
file2 = "/Users/michaeltotaro/LivestreamDirectory/temp/artists.json"

if are_files_equal(file1, file2):
    print("artists.json EQUAL: YES")
else:
    print(c("The files are not equivalent.", 'red'))


file1 = "/Users/michaeltotaro/LivestreamDirectory/db_manager/json_files/no_repeats.json"
file2 = "/Users/michaeltotaro/LivestreamDirectory/temp/no_repeats.json"

if are_files_equal(file1, file2):
    print("no_repeats.json EQUAL: YES")
else:
    print(c("The files are not equivalent.", 'red'))


file1 = "/Users/michaeltotaro/LivestreamDirectory/db_manager/json_files/only_with_keys.json"
file2 = "/Users/michaeltotaro/LivestreamDirectory/temp/only_with_keys.json"

if are_files_equal(file1, file2):
    print("only_with_keys EQUAL: YES")
else:
    print(c("The files are not equivalent.", 'red'))

file1 = "/Users/michaeltotaro/LivestreamDirectory/db_manager/json_files/repertoire.json"
file2 = "/Users/michaeltotaro/LivestreamDirectory/temp/repertoire.json"

if are_files_equal(file1, file2):
    print("only_with_keys EQUAL: YES")
else:
    print(c("The files are not equivalent.", 'red'))

file1Content = []

with open(file1, 'r') as file1r:
    for line in file1r:
        file1Content.append(line)

file2Content = []
with open(file2, 'r') as file2r:
    for line in file2r: 
        file2Content.append(line)

for i in range(len(file1Content)):
    if file1Content[i] != file2Content[i]:
        print(f"1. {file1Content[i]}")
        print(f"2. {file2Content[i]}")



