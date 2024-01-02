
using System.ComponentModel;
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
     * the allSongs string list and all-timestamps.txt file.
     * @param allSongs All songs from all-timestamps.txt. Each song
     *        only appears one time and has been stripped of it's keys.
     */
    public static void Run(List<string> allSongs)
    {
        string filePath = "./db_manager/timestamps/all-timestamps.txt";
        string[] allTimestampLines = File.ReadAllLines(filePath);


        StringBuilder jsonString = new StringBuilder();

        jsonString.Append("{\n    \"songs\":[\n");
        
        int count = 0;

        foreach (string song in allSongs)
        {
            string title = song;
            string artist = "";
            string otherArtists = "";
            string appearances =  "";
            string instruments = "";
            string other = "";
            string links = "";

            string currentLivestream = "";
            string currentLink = "";
            string onlyAppearsWithKeys = "";

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
                    string fileSongTimestamp = line.Split(" ")[0];
                    string fileSongWithKeys = AlgorithmHelper.ReplaceWithCorrectQuotes(fileSongAndArtist[0]);
                    string fileArtists = fileSongAndArtist[1];
                    string currentLivestreamNoKeys = Helper.RemoveKeys(currentLivestream);
                    string fileSongWithOutKeys = Helper.RemoveKeys(fileSongWithKeys);

                    string fileMainArtist = fileArtists.Split("/")[0];

                    string[] otherArtistsFromFile = fileArtists.Split('/').Skip(1).ToArray(); //Get all artists except first.
                    string[]? fileOtherArtists = otherArtistsFromFile.Length != 0 ? otherArtistsFromFile : null;

                    List<string> allSongsKeysAsList = AlgorithmHelper.GetAllKeysFromLines(currentLivestream, fileSongWithKeys);
                    string joinedAppearanceKeys = AlgorithmHelper.GetKeysJoinedAsString(allSongsKeysAsList);

                    if (song == fileSongWithOutKeys)
                    {
                        
                    } //song == fileSongWithoutKeys if block ends 
                } //fileSongAndArtist != null if block ends.
            } //line in allTimestampLines for loop ends
        } //song in allSongs for loop ends 
    } //Run method ends 
}