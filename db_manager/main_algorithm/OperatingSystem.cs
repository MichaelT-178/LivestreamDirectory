
using System.Diagnostics;

/**
 * This class allows you to run operating system commands
 *
 * Methods
 * OpenFileInVSCode | Opens a file in VSCode for viewing.
 * ExecuteGitCommands | Adds, commits, and pushes project to 
 * ExecuteCommand | Executes a command on the operating system.
 * UpdateSQLiteDatabase | Runs the run_all.py file to update the SQLite table.
 * PushChangesInVue | Push changes that were made in the VueLivestreamDirectory folder
 * RunFixIndentationScript | Run fix_indentation.py in VueLivestreamDirectory
 *
 * @author Michael Totaro
 */
class OS
{

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
     * @param commitMsg The optional commit message.
     */
    public static void ExecuteGitCommands(string commitMsg = "Adding changes")
    {
        ExecuteCommand("git add .");
        Color.DisplaySuccess("git add completed successfully");

        ExecuteCommand($"git commit -m \"{commitMsg}\"");
        Color.DisplaySuccess("git commit completed successfully");

        ExecuteCommand("git push");
        Color.DisplaySuccess("git push completed successfully");
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
            RedirectStandardError = true,
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

        using (Process process = new() { StartInfo = startInfo })
        {
            process.Start();
            process.WaitForExit();
        }

        // Directory Livestream/
        Environment.CurrentDirectory = originalDirectory;
    }

    /**
     * Push changes that were made in the VueLivestreamDirectory folder
     */
    public static void PushChangesInVue()
    {
        string originalDirectory = Environment.CurrentDirectory;
        string vueLivestreamDirectoryPath = Path.Combine(originalDirectory, "../VueLivestreamDirectory");

        OpenFileInVSCode(vueLivestreamDirectoryPath);

        Color.DisplaySuccess("PUSHING TO VueLivestreamDirectory!!!\n", "\n\n");

        Console.Write("Enter commit message (press 'p' to pass): ");
        string? commitMsg = Console.ReadLine()?.Trim();

        if (commitMsg?.ToLower() == "p")
        {
            Color.PrintLine("Push skipped.", "Magenta");
            return;
        }

        if (string.IsNullOrWhiteSpace(commitMsg))
        {
            Color.DisplayError("Commit message cannot be empty.");
            return;
        }

        try
        {
            Environment.CurrentDirectory = Path.GetFullPath(vueLivestreamDirectoryPath);
            ExecuteGitCommands(commitMsg);

            Console.WriteLine(""); //Whitespace
        }
        catch (Exception ex)
        {
            Color.DisplayError("Failed to push Vue changes.");
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            Environment.CurrentDirectory = originalDirectory;
        }
    }

    /**
     * Run fix_indentation.py in VueLivestreamDirectory
     * 
     * python3 ../../../VueLivestreamDirectory/python/fix_indentation.py
     */
    public static void RunFixIndentationScript()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python3",
                Arguments = "../../../VueLivestreamDirectory/python/fix_indentation.py",
                UseShellExecute = false,
                RedirectStandardError = true
            }
        };

        process.Start();

        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine("Python script error:\n" + error);
        }
    }

}