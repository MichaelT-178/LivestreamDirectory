class CreateNewJSON
{
    public static void AddAlbumAttribute()
    {
        List<Album> albums =  AlbumRepertoireHandler.GetAlbums();
        List<Song> songs = JSONHelper.GetDatabaseSongs();

        for (int i = 0; i < albums.Count; i++) 
        {
            Album album = albums[i];
            Song song = songs[i];

            //String comparison
            if (song.Title == album.Song)
            {
                // add an attribute called "album" to the song_list.json song
                // Set it to album.AlbumTitle
            }

        }
    }


}