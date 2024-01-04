using System.Diagnostics;

/**
 * This is the main UI of the Livestream Directory algorithm.
 * The purpose of this algorithm is to build and update the database.
 * @author Michael Totaro
 */
class Program 
{
    /**
     * Allows the user to run the algorithm and enter input
     */
    public static void Main()
    {   
        Color.PrintLine("REMEBER TO ADD THE YOUTUBE LINK", "Magenta");

        Console.Write(@"Do you want to open the ""all-timestamps.txt"" file? : ");
        string openTimestamps = Console.ReadLine() ?? "";

        if (Helper.GetAnsweredYes(openTimestamps))
        {
            OS.OpenFileInVSCode("./db_manager/timestamps/all-timestamps.txt");
            Environment.Exit(0);
        }

        //Start timer 
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine("\nPlease be patient. This will take a couple of seconds...");
        Console.WriteLine("Writing to file...");
    
        List<string> songsWithKeys = ErrorFinder.FindCapErrors();

        List<string> songsWithoutKeys = Helper.GetSongListWithoutKeys(songsWithKeys);

        Color.PrintLine("No capitalization errors found!", "Green");

        ErrorFinder.AllArtistPicturesExist();
        
        Color.PrintLine("An image was found for every artist!", "Green");

        Console.WriteLine("Currently running algorithm...");

        //RUN THE MAIN ALGORITHM
        Algorithm.Run(songsWithoutKeys);

        Color.PrintLine("Database song_list.json file was successfuly created!", "Green");

        JSONHelper.WriteJSONToFile(Helper.GetSortedAlphabetList(JSONHelper.GetDatabaseSongs()));

        //Stop the watch
        stopwatch.Stop();
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        Console.WriteLine($"Elapsed Time: {elapsedSeconds} seconds");


        //EXECUTE GITHUB COMMANDS 
        
    }
}
