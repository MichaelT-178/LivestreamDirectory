jar_path = "../../json-20230618.jar"

compile_str = "javac -cp #{jar_path} -d ./bin ./MainAlgorithm.java"

files = ["Algorithm",
         "ErrorHandler",
         "JSONHelper",
         "OperatingSystem"]

for file in files
    compile_str += " ./#{file}.java "
end 

#Compile program
system(compile_str.strip)