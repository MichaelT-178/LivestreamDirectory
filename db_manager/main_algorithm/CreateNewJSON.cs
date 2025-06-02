using Newtonsoft.Json;

/**
 * This class creates the new JSON file for the VueLivestreamDirectory.
 *
 * Methods
 * AddAlbumAttribute | Sets the "Album" attribute in song_list.json
 * UpdateAlbums | Creates the VueLivestreamDirectory albums.json file
 * UpdateFavoriteCovers | Update VueLivestreamDirectory FavCovers.json file.
 * CreateVueRepertoire | Create the repertoire.json file in VueLivestreamDirectory.
 *
 * @author Michael Totaro
 */
class CreateNewJSON
{

    /**
     * Sets the "Album" attribute in song_list.json to the songs 
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
                return char.IsLetter(firstChar) ? $"{firstChar} Songs" : "Miscellaneous";
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
            }).ToList());

        string json = JsonConvert.SerializeObject(grouped, Formatting.Indented);
        JSONHelper.WriteJSONToVueData("repertoire.json", json);
    }


}