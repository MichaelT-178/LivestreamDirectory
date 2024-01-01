
using System.Diagnostics;

/**
 * This class allows you to run operating system commands
 *
 * Methods
 * OpenFileInVSCode | Opens a file in VSCode for viewing.
 * ExecuteGitCommands | Adds, commits, and pushes project to Github
 *
 * @author Michael Totaro
 */
class OS {

     /**
      * Opens a file in Visual Studio Code.
      * @param path The path of the file to opened.
      */
     public static void OpenFileInVSCode(string filePath)
     {
        try
        {
            string path = "code";
            string arguments = $"\"{filePath}\"";

            ProcessStartInfo startInfo = new()
            {
                FileName = path,
                Arguments = arguments,
                UseShellExecute = true
            };

            Process process = new() { StartInfo = startInfo };

            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Color.DisplayError("Visual Studio Code could not be opened.");
            Console.WriteLine(ex.ToString());
        }
    }

    /**
     * Executes the git commands git add, git commit, and git push commands. 
     */
    public static void ExecuteGitCommands()
    {
        
    }

}