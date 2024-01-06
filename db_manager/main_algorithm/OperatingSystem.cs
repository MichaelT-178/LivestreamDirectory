
using System.Diagnostics;

/**
 * This class allows you to run operating system commands
 *
 * Methods
 * OpenFileInVSCode | Opens a file in VSCode for viewing.
 * ExecuteGitCommands | Adds, commits, and pushes project to 
 * ExecuteCommand | Executes a command on the operating system.
 * UpdateSQLiteDatabase | Runs the run_all.py file to update the SQLite table.
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
        ExecuteCommand("git add .");
        Color.PrintLine("git add completed successfully", "Green");

        ExecuteCommand("git commit -m \"Adding files\"");
        Color.PrintLine("git commit completed successfully", "Green");

        ExecuteCommand("git push");
        Color.PrintLine("git push completed successfully", "Green");
    }

    /**
     * Executes a command on the operating system.
     * @param command to be executed.
     */
    public static void ExecuteCommand(string command)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            UseShellExecute = false
        };

        Process process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();

        process.StandardInput.WriteLine(command);
        process.StandardInput.Flush();
        process.StandardInput.Close();

        process.WaitForExit();
    }

    /**
     * Runs the run_all.py file in the database/table directory
     * to update the SQLite table.
     */
    public static void UpdateSQLiteDatabase()
    {   
        // Directory Livestream/
        string originalDirectory = Environment.CurrentDirectory;

        // Directory Livestream/database/table
        string targetPath = Path.Combine(originalDirectory, "database/table");
        Environment.CurrentDirectory = targetPath;

        string pythonInterpreter = "python3";
        string scriptPath = "run_all.py";

        ProcessStartInfo startInfo = new()
        {
            FileName = pythonInterpreter,
            Arguments = scriptPath,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new () { StartInfo = startInfo })
        {
            process.Start();
            process.WaitForExit();
        }
        
        // Directory Livestream/
        Environment.CurrentDirectory = originalDirectory;
    }

}