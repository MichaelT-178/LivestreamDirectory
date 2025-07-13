with open("all-timestamps.txt", 'r') as file:
    for line in file:
        if line.strip():
            if all(keyword not in line for keyword in ("https", "Livestream", "Solo Video", " by ")):
                print(line)