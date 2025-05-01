class CSharpMethods:
    def get_all_artists(self) -> list[str]:
        with open("../database/song_list.json", 'r') as file:
            for line in file:
                print(line)