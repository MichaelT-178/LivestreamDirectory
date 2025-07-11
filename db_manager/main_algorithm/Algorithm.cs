
using System.Text;

/**
 *
 * This class runs the algorithm to convert all-timestamps.txt
 * to song_list.json in the database directory.
 *
 * Methods
 * Run | Runs the algorithm
 *
 * @author Michael Totaro
 */
class Algorithm
{   
    /**
     * This algorithm creates the song_list.json database from 
     * the songsWithoutKeys string list and all-timestamps.txt file.
     * @param allSongs songs from all-timestamps.txt. Each song
     *        only appears one time and has been stripped of it's keys.
     */
    public static void Run(List<string> allSongs)
    {
        string filePath = "./db_manager/timestamps/all-timestamps.txt";
        string[] allTimestampLines = File.ReadAllLines(filePath);

        int idCount = 1;

        StringBuilder songListBuilder = new();

        songListBuilder.Append("{\n    \"songs\":[\n");

        foreach (string song in allSongs)
        {
            string title = song;
            string artist = "";
            string[] otherArtistsList = [];
            string appearances =  "";
            string instruments = "";

            StringBuilder searchBuilder = new StringBuilder();

            string links = "";

            //Ex: Livestream 95 (Audio Issues) or Solo Video
            string currentLivestream = "";
            string currentLink = "";

            foreach (string line in allTimestampLines)
            {
                if (line.Contains("Solo Video"))
                {
                    currentLivestream = line.Trim();
                    continue;
                }

                if (line.Contains("Livestream"))
                {
                    currentLivestream = line.Trim();
                    continue;
                }

                if (line.Contains("https"))
                {
                    currentLink = line.Trim();
                    continue;
                }

                if (string.IsNullOrEmpty(line.Trim()))
                {
                    continue;
                }

                string[]? fileSongAndArtist = Helper.GetSongAndArtist(line);

                if (fileSongAndArtist != null)
                {   
                    //Ex: 1:14:31
                    string fileSongTimestamp = line.Split(" ")[0];
                    //Ex: Satisfied Mind (MDT)
                    string fileSongWithKeys = Helper.ReplaceWithCorrectQuotes(fileSongAndArtist[0]);
                    //Ex: Jeff Buckley/Porter Wagoner/Red Hays
                    string fileArtists = fileSongAndArtist[1];
                    //Livestream 95
                    string currentLivestreamNoKeys = Helper.RemoveKeys(currentLivestream);
                    //Ex: Satisfied Mind
                    string fileSongWithOutKeys = Helper.RemoveKeys(fileSongWithKeys);
                    //Ex: Jeff Buckley
                    string fileMainArtist = fileArtists.Split("+")[0];

                    //Ex: ["Porter Wagoner", "Red Hays"]
                    string[] otherArtistsFromFile = fileArtists.Split('+').Skip(1).ToArray(); //Get all artists except first.
                    string[]? fileOtherArtists = otherArtistsFromFile.Length != 0 ? otherArtistsFromFile : null;

                    //Ex: ["(Pink Moon Album)", "(M)"]
                    List<string> allSongsKeysAsList = AlgorithmHelper.GetAllKeysFromLines(currentLivestream, fileSongWithKeys);
                    //Ex: (Pink Moon Album/M)
                    string joinedAppearanceKeys = AlgorithmHelper.GetKeysJoinedAsString(allSongsKeysAsList);

                    if (song == fileSongWithOutKeys)
                    {
                        appearances += $"{currentLivestreamNoKeys}{joinedAppearanceKeys},";
                        artist = fileMainArtist;
                        otherArtistsList = (fileOtherArtists?.Length ?? 0) > otherArtistsList.Length
                                            ? fileOtherArtists!
                                            : otherArtistsList;
                        
                        instruments += AlgorithmHelper.AddDefaultAcousticGuitar(fileSongWithKeys, instruments);
                        instruments += AlgorithmHelper.AddDefaultElectricGuitar(fileSongWithKeys, instruments);
                        instruments += AlgorithmHelper.AddDefaultClassicalGuitar(fileSongWithKeys, instruments);
                        instruments += AlgorithmHelper.GetInstrumentsFromSong(fileSongWithKeys, instruments);

                        SearchHelper.GetInfo(ref searchBuilder, title, fileArtists, instruments);
                        links += AlgorithmHelper.GetYouTubeLink(currentLink, fileSongTimestamp);
                    } //song == fileSongWithoutKeys if block ends 

                } //fileSongAndArtist != null if block ends.

            } //line in allTimestampLines for loop ends

            if (!string.IsNullOrEmpty(appearances))
            {
                appearances = appearances[..^1]; //get rid of last "," character
            }
            
            if (links.Length > 3)
            {
                links = links[..^3]; //get rid of last " , " character
            }

            title = title.Replace("\"", "\\\"");
            //Removes the keys that are used to identify songs with the same name but different artists.
            title = title.Replace("(I)", "").Replace("(C)", "").Trim();

            string titlePartialAndIssueKey = AlgorithmHelper.GetSongTitlePartialAndIssuesKey(appearances);
            title += titlePartialAndIssueKey;

            string otherArtists = AlgorithmHelper.GetOtherArtistsAsString(otherArtistsList);

            string cleanedArtist = TextCleaner.CleanText(artist);
            
            string search = searchBuilder.ToString();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim()[..^1]; //get rid of last "," character
                search = search.Replace("\"", "\\\"");
            }

            if (title.Contains("Led Boots")) appearances = appearances.Replace(" 50 (Electric Song)", " 50 (Electric riff)");

            if (title.Contains("Electric Riff Session #")) title = title.Replace(" (Electric riff)", "");

            if (title.Contains("Fox Chase") || title.Contains("Down The River Rhine"))
            {
                title = title.Replace(" (H)", "");
            }

            if (instruments.Length > 2)
            {
                instruments = instruments[..^2]; //get rid of last " , " character
            }

            if (appearances.Contains("Rein Rutnik") && !search.Contains("Rein Rutnik"))
            {
                search += " Rein Rutnik";
            }
            
            instruments = AlgorithmHelper.RemoveDuplicateGuitars(instruments);
            instruments = AlgorithmHelper.MoveAcousticGuitarToFront(instruments);

            if (appearances.Contains("Rein Rutnik"))
            {
                appearances = AlgorithmHelper.AddHarmonicaToAppearances(appearances);
            }

            // Handled later. Just leave as a blank string for now. AddAlbumAttribute
            string album = "";
            string cleanedAlbum = "";
            int year = 0;

            string cleanedTitle = TextCleaner.CleanText(title);
            cleanedTitle = JSONHelper.GetRepeatCleanedTitle(cleanedTitle, artist);

            Song jsonSong = new(idCount, title, cleanedTitle, artist, cleanedArtist, 
                                album, cleanedAlbum, otherArtists, instruments, year,
                                search, appearances, links);
            
            songListBuilder.Append(JSONHelper.GetJSONSongAsString(jsonSong));
            idCount++;
        } //song in allSongs for loop ends 

        string jsonSongString = songListBuilder.ToString().Trim()[..^1];
        jsonSongString += "\n    ]\n}";
        
        // song_list.json created
        JSONHelper.WriteTextToJSONFile(jsonSongString);

        AlbumRepertoireHandler.UpdateRepertoireFile(Helper.GetSortedAlphabetList(JSONHelper.GetDatabaseSongsAsString()));

        // CREATE NEW FILES
        AlbumRepertoireHandler.CheckForNoRepeatAlbums();
        AlbumRepertoireHandler.SyncAlbumsWithRepertoire();



        // Just ensures all cleaned attributes are up to date 
        // and formatted properly before checking images
        AlbumRepertoireHandler.UpdateCleanedAttributes();

        CreateNewJSON.AddAlbumAttribute();

        CreateNewJSON.AddAlbumSongInfo();

        ErrorFinder.FindDuplicates();

    } //Run method ends 
}