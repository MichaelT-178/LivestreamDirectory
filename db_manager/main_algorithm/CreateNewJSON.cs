using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

/**
 * This class creates the new JSON file for the VueLivestreamDirectory.
 *
 * Methods
 * AddAlbumAttribute | Sets the "Album" and "Year" attributes in song_list.json
 * UpdateAlbums | Creates the VueLivestreamDirectory albums.json file
 * UpdateFavoriteCovers | Update VueLivestreamDirectory FavCovers.json file.
 * CreateVueRepertoire | Create the repertoire.json file in VueLivestreamDirectory.
 * CreateSongsFile | Create the songs.json file in VueLivestreamDirectory.
 * MergeAppearancesAndLinks | Make an object list out of appearances and links
 * CreateSearchData | Create the SearchData.json file in VueLivestreamDirectory.
 * CreateArtistFile | Create the artists.json file in VueLivestreamDirectory.
 * AddArtistToMap | Adds a song and its associated album to the artist entry in the map.
 * GetLocalArtistData | Gets the JSON data from db_manager/json_files/artists.json
 * CreateCountriesFile | Creates the countries.json file in VueLivestreamDirectory.
 * AddAlbumSongInfo | Add 'song' to albums with same name as song. Fix L.A. woman
 *
 * @author Michael Totaro
 */
class CreateNewJSON
{

    /**
     * Sets the "Album" and "Year" attributes in song_list.json to the songs 
     * album based on the values from albums.json.
     */
    public static void AddAlbumAttribute()
    {
        List<Album> albums = AlbumRepertoireHandler.GetAlbums();
        List<Song> songs = JSONHelper.GetDatabaseSongs();

        foreach (var album in albums)
        {
            var matchingSong = songs.FirstOrDefault(song =>
                song.Title == album.Song &&
                (album.Artist == null || song.Artist == album.Artist)
            );

            if (matchingSong != null)
            {
                if (!string.IsNullOrWhiteSpace(album.AlbumTitle))
                {
                    string originalTitle = album.AlbumTitle!;
                    string cleanedTitle = TextCleaner.CleanText(originalTitle);

                    matchingSong.Album = originalTitle;
                    matchingSong.CleanedAlbum = cleanedTitle;
                }
                else
                {
                    matchingSong.Album = album.AlbumTitle!;
                    matchingSong.CleanedAlbum = null!;
                }

                // Set Year if currently 0
                if (matchingSong.Year == 0 && album.Year != 0)
                {
                    matchingSong.Year = album.Year;
                }
            }
        }

        var updatedData = new { songs };
        string json = JsonConvert.SerializeObject(updatedData, Formatting.Indented);
        File.WriteAllText("./database/song_list.json", json);
    }


    /**
     * Creates the VueLivestreamDirectory albums.json file by 
     * converting db_manager/json_files/albums.json to 
     * VueLivestreamDirectory/src/assets/Data/albums.json.
     */
    public static void UpdateAlbums()
    {
        List<Album> albums = AlbumRepertoireHandler.GetAlbums();

        var grouped = albums
            .Where(album => !string.IsNullOrEmpty(album.CleanedAlbumTitle))
            .GroupBy(album => album.CleanedAlbumTitle)
            .OrderBy(g => g.Key)
            .ToDictionary(
                group => group.Key!,
                group =>
                {
                    var first = group.First();

                    return new
                    {
                        AlbumTitle = first.AlbumTitle,
                        CleanedAlbumTitle = group.Key,
                        Artist = first.Artist,
                        CleanedArtist = first.CleanedArtist,
                        Year = first.Year,
                        Songs = group.Select(song => new
                        {
                            Song = song.Song,
                            CleanedSong = TextCleaner.CleanText(song.Song)
                        }).ToList()
                    };
                }
            );

        string json = JsonConvert.SerializeObject(grouped, Formatting.Indented);
        JSONHelper.WriteJSONToVueData("albums.json", json);
    }


    /**
     * Write the contents of the local fav_covers.json file 
     * to the VueLivestreamDirectory FavCovers.json file
     */
    public static void UpdateFavoriteCovers()
    {
        string ogPath = "./db_manager/json_files/fav_covers.json";
        string newPath = "../VueLivestreamDirectory/src/assets/Data/FavCovers.json";

        JSONHelper.WriteJSONToDifferentFile(ogPath, newPath);
    }


    /**
     * Create the repertoire.json file in VueLivestreamDirectory.
     */
    public static void CreateVueRepertoire()
    {
        List<Album> albums = AlbumRepertoireHandler.GetAlbums();

        List<Album> sortedAlbums = albums
            .OrderBy(album => album.Song, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var grouped = sortedAlbums
            .GroupBy(album =>
            {
                char firstChar = string.IsNullOrWhiteSpace(album.Song) ? '#' : char.ToUpper(album.Song[0]);
                return char.IsLetter(firstChar) ? $"{firstChar}" : "Miscellaneous";
            })
            .OrderBy(group => group.Key == "Miscellaneous" ? "" : group.Key)
            .ToDictionary(group => group.Key, group => group.Select(album => new
            {
                album.Song,
                album.CleanedSong,
                album.AlbumTitle,
                album.CleanedAlbumTitle,
                album.Artist,
                album.CleanedArtist,
                Category = group.Key == "Miscellaneous" ? "Miscellaneous" : $"{group.Key} Songs",
            }).ToList());

        string json = JsonConvert.SerializeObject(grouped, Formatting.Indented);
        JSONHelper.WriteJSONToVueData("repertoire.json", json);
    }

    /**
     * Create the songs.json file in VueLivestreamDirectory.
     */
    public static void CreateSongsFile()
    {
        List<Song> songs = JSONHelper.GetDatabaseSongs();

        var songDict = songs
            .Where(song => !string.IsNullOrWhiteSpace(song.CleanedTitle))
            .ToDictionary(song => song.CleanedTitle, song => new
            {
                song.Id,
                song.Title,
                song.CleanedTitle,
                song.Artist,
                song.CleanedArtist,
                song.Album,
                song.CleanedAlbum,
                Other_Artists = string.IsNullOrWhiteSpace(song.Other_Artists)
                    ? new List<object>()
                    : song.Other_Artists.Split("+ ")
                        .Select(artistName => (object)new
                        {
                            artist = artistName.Trim(),
                            cleanedArtist = TextCleaner.CleanText(artistName),
                        }).ToList(),

                Instruments = string.IsNullOrWhiteSpace(song.Instruments)
                    ? new List<object>()
                    : song.Instruments.Split(", ")
                        .Select(instr => instr.Trim())
                        .Where(instr => !string.IsNullOrEmpty(instr))
                        .Select(instr => (object)new
                        {
                            name = instr,
                            cleanedName = CreateNewInstrument.CleanInstrument(instr)
                        }).ToList(),

                song.Year,
                song.Search,
                Appearances = MergeAppearancesAndLinks(song.Appearances, song.Links),
            });

        string json = JsonConvert.SerializeObject(songDict, Formatting.Indented);
        JSONHelper.WriteJSONToVueData("songs.json", json);
    }


    /**
     * Make an object list out of appearances and links
     *
     * @param appearances String of all appearances separated by ","
     * @param links String of all links separated by " , " 
     * @return List of new appearance objects
     */
    private static List<object> MergeAppearancesAndLinks(string appearancesStr, string linksStr)
    {
        var appearanceList = appearancesStr.Split(',').Select(a => a.Trim()).ToList();
        var linkList = linksStr.Split([" , "], StringSplitOptions.None).Select(l => l.Trim()).ToList();

        int count = Math.Max(appearanceList.Count, linkList.Count);
        var combinedList = new List<object>();

        for (int i = 0; i < count; i++)
        {
            string fullAppearance = i < appearanceList.Count ? appearanceList[i] : "";
            string appearance = AlgorithmHelper.CleanAppearance(fullAppearance);
            string link = i < linkList.Count ? linkList[i] : "";
            List<string> keys = AlgorithmHelper.ExtractKeysFromAppearance(fullAppearance);

            if (fullAppearance == CreateNewInstrument.RemoveInstrumentKeys(fullAppearance))
            {
                keys.Insert(0, "main");
            }

            combinedList.Add(new
            {
                id = i + 1,
                appearance,
                link,
                keys
            });
        }

        return combinedList;
    }


    /**
     * Create the SearchData.json file in VueLivestreamDirectory.
     */
    public static void CreateSearchData()
    {
        List<BasicArtist> artists = JSONHelper.GetDatabaseArtists();
        List<Song> songs = JSONHelper.GetDatabaseSongs();

        var artistData = artists.Select(a => new
        {
            id = a.Id,
            name = a.Artist,
            cleanedName = a.CleanedArtist,
            Type = "Artist"
        });

        var songData = songs.Select(s => new
        {
            id = s.Id,
            title = s.Title,
            cleanedTitle = s.CleanedTitle,
            artist = s.Artist,
            cleanedArtist = s.CleanedArtist,
            album = s.Album,
            cleanedAlbum = s.CleanedAlbum,
            otherArtists = s.Other_Artists,
            instruments = s.Instruments,
            search = s.Search,
            CleanedPicture = !string.IsNullOrWhiteSpace(s.CleanedAlbum) ? s.CleanedAlbum : s.CleanedArtist,
            ArtistPic = string.IsNullOrWhiteSpace(s.CleanedAlbum),
            Type = "Song"
        });

        var combinedData = artistData.Concat<object>(songData).ToList();

        var wrappedData = new { SearchData = combinedData };

        string json = JsonConvert.SerializeObject(wrappedData, Formatting.Indented);
        JSONHelper.WriteJSONToVueData("SearchData.json", json);
    }



    /**
     * Create the artists.json file in VueLivestreamDirectory.
     */
    public static void CreateArtistFile()
    {
        List<Song> songs = JSONHelper.GetDatabaseSongs();
        List<Album> albums = AlbumRepertoireHandler.GetAlbums();

        var localArtistData = GetLocalArtistData();

        var artistMap = new Dictionary<string, ArtistEntry>();

        foreach (var song in songs)
        {
            AddArtistToMap(song.Artist, song, artistMap, albums, localArtistData);

            if (!string.IsNullOrWhiteSpace(song.Other_Artists))
            {
                var otherArtists = song.Other_Artists.Split("+ ");


                foreach (var otherArtist in otherArtists)
                {
                    string trimmed = otherArtist.Trim();

                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        AddArtistToMap(trimmed, song, artistMap, albums, localArtistData);
                    }
                }
            }
        }

        string json = JsonConvert.SerializeObject(artistMap, Formatting.Indented);
        JSONHelper.WriteJSONToVueData("artists.json", json);
    }

    /**
     * Adds a song and its associated album to the artist entry in the map.
     *
     * @param artistName The name of the artist to associate with the song
     * @param song The song object
     * @param map The dictionary mapping cleaned artist names to their corresponding ArtistEntry objects
     * @param allAlbums The full list of album entries
     * @param localArtistData The JSON data from db_manager/json_files/artists.json
     */
    private static void AddArtistToMap(
        string artistName,
        Song song,
        Dictionary<string, ArtistEntry> map,
        List<Album> allAlbums,
        Dictionary<string, LocalArtist> localArtistData)
    {
        string cleanedArtist = TextCleaner.CleanText(artistName);

        if (!map.TryGetValue(cleanedArtist, out var entry))
        {
            localArtistData.TryGetValue(cleanedArtist, out var meta);

            entry = new ArtistEntry
            {
                Artist = artistName,
                CleanedArtist = cleanedArtist,
                Location = meta!.Location,
                YearFormed = meta.YearFormed,
                Genre = meta.Genre,
                Country = meta.Country,
                CleanedCountry = meta.CleanedCountry,
                Emoji = meta.Emoji,
                Songs = new List<ArtistSong>(),
                Albums = new List<AlbumArtist>()
            };

            map[cleanedArtist] = entry;
        }

        entry.Songs.Add(new ArtistSong
        {
            Id = song.Id,
            Title = song.Title,
            CleanedTitle = song.CleanedTitle,
            Album = song.Album,
            CleanedAlbum = song.CleanedAlbum
        });

        if (!string.IsNullOrWhiteSpace(song.Album))
        {
            string cleanedAlbum = song.CleanedAlbum ?? TextCleaner.CleanText(song.Album);

            bool albumAlreadyExists = entry.Albums.Any(a => a.CleanedTitle == cleanedAlbum);

            if (!albumAlreadyExists)
            {
                var albumData = allAlbums.FirstOrDefault(a =>
                    a.CleanedSong == song.CleanedTitle
                );

                entry.Albums.Add(new AlbumArtist
                {
                    Title = song.Album,
                    CleanedTitle = cleanedAlbum,
                    Year = albumData?.Year ?? 0
                });

                entry.Albums = entry.Albums.OrderBy(a => a.Year).ToList();

                // var albumYearMap = entry.Albums.ToDictionary(a => a.CleanedTitle, a => a.Year);

                // entry.Songs = entry.Songs
                //     .OrderBy(song =>
                //         albumYearMap.TryGetValue(song.CleanedAlbum ?? "", out var year) ? year : int.MaxValue
                //     )
                //     .ThenBy(song => song.Title)
                //     .ToList();
            }
        }
    }

    /**
     * Gets the JSON data from db_manager/json_files/artists.json
     * 
     * @return The data from the local artists.json file
     */
    public static Dictionary<string, LocalArtist> GetLocalArtistData()
    {
        string path = "./db_manager/json_files/artists.json";
        string json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<Dictionary<string, LocalArtist>>(json)
            ?? throw new ArgumentException("Could not load local artist metadata.");
    }

    /**
     * Creates the countries.json file in VueLivestreamDirectory.
     */
    public static void CreateCountriesFile()
    {
        string inputPath = "./db_manager/json_files/artists.json";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("artists.json not found.");
            return;
        }

        string jsonString = File.ReadAllText(inputPath);
        var artists = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonObject>>(jsonString)!;

        var countries = new Dictionary<string, JsonObject>();
        var allCountriesList = new List<JsonObject>();

        foreach (var entry in artists)
        {
            JsonObject artistData = entry.Value;

            JsonNode countryNode = artistData["CleanedCountry"]
                                   ?? throw new Exception($"Artist is missing 'CleanedCountry': {artistData}");

            string cleanedCountry = countryNode.ToString().ToLower();
            string countryName = artistData["Country"]?.ToString() ?? cleanedCountry;
            string cleanedCountryName = artistData["CleanedCountry"]?.ToString() ?? cleanedCountry;
            string emoji = artistData["Emoji"]?.ToString() ?? "";

            artistData.Remove("CleanedCountry");
            artistData.Remove("Country");
            artistData.Remove("Emoji");

            if (!countries.ContainsKey(cleanedCountry))
            {
                var countryObj = new JsonObject
                {
                    ["name"] = countryName,
                    ["cleanedName"] = cleanedCountryName,
                    ["numOfArtists"] = 0,
                    ["emoji"] = emoji,
                    ["artists"] = new JsonArray()
                };

                countries[cleanedCountry] = countryObj;

                // Add to AllCountries list
                var allCountryEntry = new JsonObject
                {
                    ["country"] = countryName,
                    ["cleanedCountry"] = cleanedCountryName,
                    ["emoji"] = emoji
                };

                allCountriesList.Add(allCountryEntry);
            }

            ((JsonArray)countries[cleanedCountry]["artists"]!).Add(artistData);
        }

        foreach (var entry in countries)
        {
            var artistList = (JsonArray)entry.Value["artists"]!;
            entry.Value["numOfArtists"] = artistList.Count;
        }

        var finalOutput = new JsonObject();

        foreach (var entry in countries)
        {
            finalOutput[entry.Key] = entry.Value;
        }

        finalOutput["AllCountries"] = System.Text.Json.JsonSerializer.Deserialize<JsonArray>(
            System.Text.Json.JsonSerializer.Serialize(allCountriesList)
        );

        var options = new JsonSerializerOptions { WriteIndented = true };
        string outputJson = System.Text.Json.JsonSerializer.Serialize(finalOutput, options);

        JSONHelper.WriteJSONToVueData("countries.json", outputJson);
    }

    /**
     * Add 'song' to albums with same name as song. Fix L.A. woman
     */
    public static void AddAlbumSongInfo()
    {
        List<Song> songs = JSONHelper.GetDatabaseSongs();

        foreach (var song in songs)
        {
            bool modified = false;

            // Add "la woman" if CleanedAlbum is "la-woman"
            if (song.CleanedAlbum == "la-woman")
            {
                string laWoman = "la woman";

                if (string.IsNullOrWhiteSpace(song.Search))
                {
                    song.Search = laWoman;
                }
                else if (!song.Search.Contains(laWoman, StringComparison.OrdinalIgnoreCase))
                {
                    song.Search += $" {laWoman}";
                }

                modified = true;
            }

            // If Title == Album, add "Title Song"
            if (!string.IsNullOrWhiteSpace(song.Title) && song.Title == song.Album)
            {
                string titleSong = $"{song.Title} Song";

                if (string.IsNullOrWhiteSpace(song.Search))
                {
                    song.Search = titleSong;
                }
                else if (!song.Search.Contains(titleSong, StringComparison.OrdinalIgnoreCase))
                {
                    song.Search += $" {titleSong}";
                }

                modified = true;
            }

            if (modified)
            {
                song.Search = song.Search.Trim();
            }
        }

        var updatedData = new { songs };
        string json = JsonConvert.SerializeObject(updatedData, Formatting.Indented);
        File.WriteAllText("./database/song_list.json", json);
    }

}