/**
 * Methods for printing colored text.
 *
 * Methods
 * Print | Print a colored string
 * PrintLine | Print a colored string with newline
 * DisplayError | Print a red error string
 * PrintWithColoredPart | Prints a single line with a colored substring.
 * GetColorCode | Get a color code given a string
 *
 * @author Michael Totaro
 */
class Color
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
     * Prints a green success message with checkmark in front.
     * @param message The message to be displayed
     */
    public static void DisplaySuccess(string message)
    {
        PrintLine($"✅ {message}", "Green");
    }

    /**
     * Prints a red colored error message.
     * @param message The message to be displayed
     */
    public static void DisplayError(string message)
    {
        PrintLine($"❌ {message}", "Red");
    }

    /**
     * Prints a single line with a colored substring.
     * If message doesn't contain substring, print error
     * message and normal message without color and return.
     * @param message The entire line 
     * @param colorPart The substring that will be colored
     * @param color The color that the substring will be
     * @param newline Whether a newline will be printed. Default false.
     */
    public static void PrintWithColoredPart(string message, string colorPart, string color, bool newline = false)
    {
        if (!message.Contains(colorPart))
        {
            DisplayError("\nThe message below supposed to contain colored text!");
            Console.WriteLine("Message: " + message);
            Console.WriteLine("Substring not in line: " + colorPart + "\n");
        }

        int index = message.IndexOf(colorPart);

        string textBefore = message.Substring(0, index);
        string textAfter = message.Substring(index + colorPart.Length);

        Console.Write(textBefore);
        Print(colorPart, color);
        Console.Write(textAfter);
        Console.Write(newline ? "\n" : ""); 

    }

    /**
     * Gets the correlating color code of the parameter.
     * @param color The color of the string that will be printed.
     * @return correlating color code of the parameter.
     * @throws ArgumentOutOfRangeException If parameter is not magenta, 
     *         red, cyan, green of blue.
     */
    public static string GetColorCode(string color)
    {
        return color.ToUpper() switch
        {
            "MAGENTA" => "35m",
            "RED" => "31m",
            "CYAN" => "36m",
            "GREEN" => "32m",
            "BLUE" => "34m",
            _ => throw new ArgumentOutOfRangeException("Incorrect color value passed!"),
        };
    }
}

