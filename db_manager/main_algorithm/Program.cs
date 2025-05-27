using System.Diagnostics;

/**
 * This is the main program of the Livestream Directory algorithm.
 * The purpose of this algorithm is to build and update the database.
 * @author Michael Totaro
 */
class Program 
{
    /**
     * Allows the user to run the algorithm and enter input
     */
    public static async Task Main()
    {


        // CreateNewJSON.UpdateFavoriteCovers();
        // Environment.Exit(0);


        Color.PrintLine("REMEBER TO ADD THE YOUTUBE LINK", "Magenta");
        Color.PrintWithColoredPart(@"Do you want to open the ""all-timestamps.txt"" file? : ", "\"all-timestamps.txt\"", "Cyan");
        
        string openTimestamps = Console.ReadLine() ?? "";

        if (Helper.GetAnsweredYes(openTimestamps))
        {
            OS.OpenFileInVSCode("./db_manager/timestamps/all-timestamps.txt");
            Environment.Exit(0);
        }

        //Start timer 
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine("\nError Check");

        ErrorFinder.CheckKeysToKeep();
    
        List<string> songsWithKeys = ErrorFinder.FindCapErrors();

        List<string> songsWithoutKeys = Helper.GetSongListWithoutKeys(songsWithKeys);

        Color.DisplaySuccess("No capitalization errors found!");

        ErrorFinder.AllLocalArtistPicturesExist();
        //ErrorFinder.AllVueArtistPicturesExist();
        
        Color.DisplaySuccess("An image was found for every artist!");

        // Small check I'm not going to display a message.
        ErrorFinder.CheckDuplicateTitlesWithDifferentArtists();

        Console.WriteLine("\nPlease be patient. This will take roughly 25-30 seconds.");
        Console.WriteLine("Currently running algorithm...");

        //RUN THE MAIN ALGORITHM and print when 5 seconds pass
        Task algorithmTask = Task.Run(() => Algorithm.Run(songsWithoutKeys));
        const int intervalSeconds = 5;

        Console.WriteLine("\nTime Passed");

        while (!algorithmTask.IsCompleted)
        {
            await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
            string secondsPassed = $"{Math.Round(stopwatch.Elapsed.TotalSeconds)}";
            Color.PrintWithColoredPart($"{secondsPassed} seconds passed", secondsPassed, "Blue", true);
        }

        Console.WriteLine("\nUpdating Database");
        Color.DisplaySuccess("Database 'song_list.json' file was successfully updated!");

        //EXECUTE UPDATE SQLite DATABASE COMMAND 
        OS.UpdateSQLiteDatabase();
        Color.DisplaySuccess("SQLite Database successfully updated!");
        
        //Stop the timer
        stopwatch.Stop();
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        string seconds = elapsedSeconds.ToString("0.00");
        Color.PrintWithColoredPart($"\nProgram took {seconds} seconds to run!", seconds, "Blue", true);


        //UPDATE ALL JSON FILES
        CreateNewInstrument.PopulateInstrumentMap();

        //ErrorFinder.AllAlbumPicturesExist();

        CreateNewJSON.UpdateFavoriteCovers();



        
        Color.PrintWithColoredPart("\nDo you want to open the \"song_list.json\" file? : ", "\"song_list.json\"", "Cyan");
        string question = Console.ReadLine() ?? "";
        
        if (Helper.GetAnsweredYes(question))
        {
            OS.OpenFileInVSCode("database/song_list.json");
        }

        Color.PrintWithColoredPart("Do you want to open the Github Repository? : ", "Github Repository", "Cyan");
        string openRepo = Console.ReadLine() ?? "";

        if (Helper.GetAnsweredYes(openRepo))
        {
            Helper.OpenInWebBrowser("https://github.com/MichaelT-178/LivestreamDirectory");
        }


        //EXECUTE GITHUB COMMANDS 
        Console.WriteLine("\nAdding changes to GitHub");
        OS.ExecuteGitCommands();
    }
    // if it has an alias attribute do NOT count it in the list.


}
