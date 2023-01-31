import os; os.system('clear')
import json

with open("/Users/michaeltotaro/LivestreamDirectory/json/song_list.json") as file:
    #for line in file:
    f = json.load(file)
    
    
    for dict in f:
        for key in dict:
            if key == "Image":
                print(dict[key].split("/")[-1])
                #Image.split(separator: "/").last
                #https://stackoverflow.com/questions/29784447/swift-regex-does-a-string-match-a-pattern
                
                
                # extension String {
                #     func matches(_ regex: String) -> Bool {
                #         return self.range(of: regex, options: .regularExpression, range: nil, locale: nil) != nil
                #     }
                # }

                # let cars = [
                #     car(name: "Toyota", year: 1997, color: "red"),
                #     car(name: "Ford", year: 1998, color: "blue"),
                #     car(name: "Honda", year: 1957, color: "green"),
                #     car(name: "Lambo", year: 1927, color: "purple")
                # ]
                
                # let shortCars = cars.filter { $0.year > 1995 || $0.color == "purple"}


                print()
            #f['Links']
    #print(f)
    
#        if '"Image": ".' in line:
#            print(line.strip().split("/")[len(line.strip().split("/")) - 1])
