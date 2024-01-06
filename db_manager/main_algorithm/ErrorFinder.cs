/**
 * Finds formatting errors before running main algorithm 
 *
 * Methods
 * FindCapErrors | Find capitalization errors.
 * AllArtistPicturesExist | Ensures every artist has picture.
 *
 * @author Michael Totaro
 */
class ErrorFinder
{   
    /**
     * Makes sure that all songs with the same name
     * have the same capitalization in the all-timestamps.txt
     * file. Then returns a string list containing all songs 
     * once without their keys.
     * @return A string list that contains each song title once 
     *         without it's keys.
     */
    public static List<string> FindCapErrors()
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

                if (!Helper.ListContainsSongWithCorrectQuotes(songs, song)
                    && Helper.ListContainsSongWithCorrectQuotes(lowerSongs, song.ToLower()))
                {
                    Color.Print("CAPITALIZATION ERROR: ", "Red");
                    Console.Write("\"" + song.Trim().Replace("’", "'").Replace("‘", "'"));
                    Console.WriteLine("\" on Line " + (lineNum + 1));
                    capErrorFound = true;
                }

                if (!Helper.ListContainsSongWithCorrectQuotes(songs, song)) 
                {
                    songs.Add(Helper.ReplaceWithCorrectQuotes(song.Trim()));
                    lowerSongs.Add(Helper.ReplaceWithCorrectQuotes(song.Trim().ToLower()));
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

        return songs;
    }

    /**
     * Checks that all image paths exist and every artist 
     * has a picture. If error occurs, message is printed 
     * and the program is exited.
     */
    public static void AllArtistPicturesExist()
    {
        string filePath = "./db_manager/timestamps/all-timestamps.txt";

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                List<string> checkedArtists = new List<string>();
                List<string> artistsWithOutImage = new List<string>();

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    string[]? songAndArtist = Helper.GetSongAndArtist(line ?? "");

                    if (songAndArtist != null)
                    {
                        string artists = songAndArtist[1];
                        string artist = artists.Split('/')[0].Trim();

                        if (artist == "AC") artist = "AC/DC";
                        if (artist == "Yusuf") artist = "Yusuf / Cat Stevens";

                        if (!checkedArtists.Contains(artist))
                        {
                            checkedArtists.Add(artist);

                            artist = artist.Replace(".", "").Replace("'", "").Replace("/", ":");
                            artist = Helper.ReplaceNonAsciiChars(artist);

                            string imagePath = "../LivestreamDirectory/pics/" + artist + ".jpg";

                            //Check if image exists 
                            if (!File.Exists(imagePath))
                            {
                                artistsWithOutImage.Add(artist);
                            }
                        } //checked artists contains ends
                    } //songAndArtist conditional ends 
                } //While reader has stream

                if (artistsWithOutImage.Count != 0)
                {
                    Console.WriteLine();

                    foreach (string artist in artistsWithOutImage)
                    {
                        Color.Print("Image not found", "Red");
                        Console.WriteLine($": {artist}");
                    }

                    Console.WriteLine("Go find images for these artists and rerun\n");
                    Environment.Exit(0);
                }
            } //Using StreamReader ends 
        } //Check file exists ends 
        else
        {
            Color.DisplayError("File not found: " + filePath);
            Environment.Exit(0);
        }
    }
    
}