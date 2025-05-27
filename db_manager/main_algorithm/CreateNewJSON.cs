using Newtonsoft.Json;

/**
 * This class creates the new JSON file for the VueLivestreamDirectory.
 *
 * Methods
 * AddAlbumAttribute | Sets the "Album" attribute in song_list.json
 * UpdateAlbums | 
 * UpdateFavoriteCovers | Update VueLivestreamDirectory FavCovers.json file.
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
     * 
     */
    public static void UpdateAlbums()
    {
        List<Song> albums = JSONHelper.GetDatabaseSongs();


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


}