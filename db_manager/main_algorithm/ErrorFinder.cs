/**
 * Finds formatting errors before running main algorithm 
 *
 * Methods
 * FindCapErrors | Find capitalization errors.
 * 
 *
 *
 *
 * @author Michael Totaro
 */
public class ErrorFinder
{
    public static List<List<string>> FindCapErrors()
    {
        string timestampsFilePath = "./db_manager/timestamps/all-timestamps.txt";

        //Guard clause. If invalid file path, throw exception
        if (!File.Exists(timestampsFilePath))
        {
            throw new FileNotFoundException("Timestamp file path is incorrect!");
        }
        
        string[] file = File.ReadAllLines(timestampsFilePath);
        
        List<string> songs = [];
        List<string> allSongs = [];
        List<string> lowerSongs = [];

        bool capErrorFound = false;
        int lineNum = 0;

        foreach (string line in file) 
        {   
            string[]? songAndArtist = Helper.GetSongAndArtist(line);

            if (songAndArtist != null) {
                string song = songAndArtist[0];

                if (!Helper.ListContainsSongWithoutCommas(songs, song)
                    && Helper.ListContainsSongWithoutCommas(lowerSongs, song.ToLower()))
                {
                    Color.Print("CAPITALIZATION ERROR: ", "Red");
                    Console.Write("\"" + song.Trim().Replace("’", "'").Replace("‘", "'"));
                    Console.WriteLine("\" on Line " + (lineNum + 1));
                    capErrorFound = true;
                }

                if (!Helper.ListContainsSongWithoutCommas(songs, song)) 
                {
                    songs.Add(song.Trim().Replace("’", "'").Replace("‘", "'"));
                    lowerSongs.Add(song.Trim().Replace("’", "'").Replace("‘", "'").ToLower());
                }

                allSongs.Add(song.Trim());
            } //if statement ends 

            lineNum++;
        } //foreach loop ends 

        if (capErrorFound)
        {
            Console.WriteLine("\nGo fix capitalization and rerun\n");
            OS.OpenFileInVSCode("./db_manager/timestamps/all-timestamps.txt");
            Environment.Exit(0);
        }

        return [songs, allSongs];
    }
}