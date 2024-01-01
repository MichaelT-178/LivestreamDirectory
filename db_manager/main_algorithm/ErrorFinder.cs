/**
 * Finds formatting errors before running main algorithm 
 *
 * Methods
 * FindCapErrors | Find capitalization errors.
 * AllArtistPicturesExist | Ensures every artist has picture.
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
                                Color.Print("Image not found!: ", "Red");
                                Console.WriteLine(artist);
                                Environment.Exit(0);
                            }
                        } //checked artists contains ends
                    } //songAndArtist conditional ends 
                } //While reader has stream
            } //Using StreamReader ends 
        } //Check file exists ends 
        else
        {
            Color.DisplayError("File not found: " + filePath);
            Environment.Exit(0);
        }
    }


}