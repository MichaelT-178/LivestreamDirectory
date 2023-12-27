/**
 * This is the main UI of the Livestream Directory algorithm.
 * @author Michael Totaro
 */
public class Program 
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
            OS.OpenFileInVSCode("./db_manager/timestamps/all-timestamps.txt"); //, "Visual Studio Code");
            Environment.Exit(0);
        }



    }
}
