/**
 * Creates the song_list.json file out of the all-timestamps.txt file
 * @author Michael Totaro
 */
public class Color
{   
    /** The beginning escape unicode sequence to print colored text */
    public const string escape = "\u001B[";
    
    /** The ending reset unicode sequence to print colored text */
    public const string reset = "\u001B[0m";

    /**
     * Prints a colored string without a newline
     * @param message The message to be displayed
     * @param color The color the message will be
     */
    public static void Print(string message, string color)
    {
        string colorCode = GetColorCode(color);
        Console.Write(escape + colorCode + message + reset);
    }

    /**
     * Prints a colored string with a newline
     * @param message The message to be displayed
     * @param color The color the message will be
     */
    public static void PrintLine(string message, string color)
    {   
        string colorCode = GetColorCode(color);
        Console.WriteLine(escape + colorCode + message + reset);
    }

    /**
     * Prints a red colored error message.
     * @param message The message to be displayed
     */
    public static void DisplayError(string message)
    {   
        string colorCode = GetColorCode("Red");
        Console.WriteLine(escape + colorCode + message + reset);
    }

    /**
     * Gets the correlating color code of the parameter.
     * @param color The color of the string that will be printed.
     * @return correlating color code of the parameter.
     * @throws ArgumentOutOfRangeException If parameter is not magenta, 
     *         red, cyan or green.
     */
    public static string GetColorCode(string color)
    {
        return color.ToUpper() switch
        {
            "MAGENTA" => "35m",
            "RED" => "31m",
            "CYAN" => "36m",
            "GREEN" => "32m",
            _ => throw new ArgumentOutOfRangeException("Incorrect color value passed!"),
        };
    }
}

