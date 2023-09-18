#Manually Change to true if you want to recompile java files 
recompile = true

ask = ""

if recompile
    Dir.chdir("./db_manager/main_algorithm/")
    system("ruby compile_files.rb")
    Dir.chdir("../../")

    print "Run algorithm? "
    ask = gets.chomp
end 

if ["YES", "Y"].include?(ask.upcase) || !recompile
    system("java -cp db_manager/main_algorithm/bin:json-20230618.jar db_manager/main_algorithm/MainAlgorithm")
end