require 'json'
system("clear")

begin

  json_data = JSON.parse(File.read("../LivestreamDirectory/database/song_list.json"))

  for song in json_data['songs']
    if song['Title'].split(" ").length == 1
        puts song['Title']
    end
  end
  
rescue JSON::ParserError => e
  puts "Error parsing JSON: #{e.message}"
rescue Errno::ENOENT => e
  puts "File not found: #{e.message}"
end