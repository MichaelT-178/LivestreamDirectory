# Adding self makes the method a static Class Method.
def c(txt, color) 

    #Program gave threw an error when I tried declaring these as constants
    red = "\e[31m"
    blue = "\e[34m"
    magenta = "\e[35m"
    cyan = "\e[36m"
    reset = "\e[0m"

    case color
        when "red"
            return "#{red}#{txt}#{reset}"
        when "blue"
            return "#{blue}#{txt}#{reset}"
        when "magenta"
            return "#{magenta}#{txt}#{reset}"
        when "cyan"
            return "#{cyan}#{txt}#{reset}"
        else
            print c("ERROR: ", "red")
            puts "Invalid input"
            exit(0)
    end

end

def are_files_equal(file1_content, file2_content)
    return file1_content == file2_content
end 

# Usage example
file1 = "../LivestreamDirectory/database/og_song_list.json"
file2 = "../LivestreamDirectory/database/song_list.json"

if are_files_equal(File.read(file1), File.read(file2))
    puts "SONG_LIST EQUAL: YES")
else
    puts c("The files are not equivalent.", 'red'))
end

file1_content = []
file2_content = []

File.foreach(file1) do |line|
    file1_content.push(line)
end 

File.foreach(file2) do |line|
    file2_content.push(line)
end 

for i in 0...file1_content.length
    if file1_content[i] != file2_content[i]
        puts "\n\n\n"
        puts "1. #{file1_content[i]}"
        puts "2. #{file2_content[i]}"
    end 
end 

file1 = "../LivestreamDirectory/db_manager/json_files/artists.json"
file2 = "../LivestreamDirectory/temp/artists.json"


if are_files_equal(File.read(file1), File.read(file2))
    puts "artists.json EQUAL: YES"
else
    puts c("The files are not equivalent.", 'red')
end 

file1 = "../LivestreamDirectory/db_manager/json_files/no_repeats.json"
file2 = "../LivestreamDirectory/temp/no_repeats.json"

if are_files_equal(File.read(file1), File.read(file2))
    puts "no_repeats.json EQUAL: YES")
else
    puts c("The files are not equivalent.", 'red'))
end

file1 = "../LivestreamDirectory/db_manager/json_files/only_with_keys.json"
file2 = "../LivestreamDirectory/temp/only_with_keys.json"

if are_files_equal(File.read(file1), File.read(file2))
    puts "only_with_keys EQUAL: YES"
else
    puts c("The files are not equivalent.", 'red')
end 

file1 = "../LivestreamDirectory/db_manager/json_files/repertoire.json"
file2 = "../LivestreamDirectory/temp/repertoire.json"

if are_files_equal(File.read(file1), File.read(file2))
    puts "only_with_keys EQUAL: YES"
else
    puts c("The files are not equivalent.", 'red'))
end 

file1_content = []
file2_content = []
        
File.foreach(file1) do |line|
    file1_content.push(line)
end 

File.foreach(file2) do |line|
    file2_content.push(line)
end 

for i in 0...file1_content.length
    if file1_content[i] != file2_content[i]
        print("1. #{file1Content[i]}")
        print("2. #{file2Content[i]}")
    end 
end 