import os

jar_path = "../../json-20230618.jar"

compile_str = f"javac -cp {jar_path} -d ./bin ./MainAlgorithm.java"

files = ["Algorithm",
         "ErrorHandler",
         "JSONHelper",
         "OperatingSystem"]

for file in files:
    compile_str += f" ./{file}.java "

#Compile program
os.system(compile_str.strip())
