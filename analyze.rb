#See if og_song_list and song_list are equivalent after running both algorithm files.
#Note now og_main_algorithm is in ancient-python-files/newer_old_files

og_song_list = "../LivestreamDirectory/database/og_song_list.json"
new_song_list = "../LivestreamDirectory/database/song_list.json"

if File.read(og_song_list) == File.read(new_song_list)
    puts "SONG_LIST EQUAL: YES"
else
    puts c("The files are not equivalent.", 'red')
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