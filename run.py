import os

#Manually Change to 1 if you want to recompile java files 
recompile = 1

if recompile:
    os.chdir("./db_manager/main_algorithm/")
    os.system("python3 compile_files.py")
    #print(os.getcwd())
    os.chdir("../../")
    #print(os.getcwd())

ask = input("GO ahead? ")

if ask.upper() in ["YES", "Y"]: 
    os.system(f"java -cp db_manager/main_algorithm/bin:json-20230618.jar db_manager/main_algorithm/MainAlgorithm")