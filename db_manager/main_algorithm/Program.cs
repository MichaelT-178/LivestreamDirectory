using System.Diagnostics;

/**
 * This is the main UI of the Livestream Directory algorithm.
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

        List<string> noRepeats = JSONHelper.GetFilesJSONData("./db_manager/json_files/no_repeats.json");
        List<string> onlyKeys = JSONHelper.GetFilesJSONData("./db_manager/json_files/only_with_keys.json");
        List<string> artistsPlayed = JSONHelper.GetFilesJSONData("./db_manager/json_files/artists.json");

        List<List<string>> updatedLists = ErrorFinder.FindCapErrors();
    
        List<string> songs = updatedLists[0];
        List<string> allSongs = updatedLists[1];

        
        


        //Stop the watch
        stopwatch.Stop();
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        Console.WriteLine($"Elapsed Time: {elapsedSeconds} seconds");



    }
}
