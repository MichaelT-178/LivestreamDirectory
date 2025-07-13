
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
     * by using python3 update_github_pages.py
     */
    public static void PushChangesInVue()
    {
        string originalDirectory = Environment.CurrentDirectory;
        string vueLivestreamDirectoryPath = Path.Combine(originalDirectory, "../VueLivestreamDirectory");

        OpenFileInVSCode(vueLivestreamDirectoryPath);
        RunFixIndentationScript();

        Color.DisplaySuccess("PUSHING TO VueLivestreamDirectory!!!\n", "\n\n");

        Console.Write("Enter commit message (press 'p' to pass): ");
        string? commitMessage = Console.ReadLine()?.Trim();

        if (commitMessage?.ToLower() == "p")
        {
            Color.DisplaySuccess("Skipped pushing changes.");
            return;
        }

        if (string.IsNullOrWhiteSpace(commitMessage))
        {
            Color.DisplayError("Commit message cannot be empty.");
            return;
        }

        try
        {
            string scriptPath = Path.Combine(vueLivestreamDirectoryPath, "update_github_pages.py");

            if (!File.Exists(scriptPath))
            {
                Color.DisplayError($"Script not found at: {scriptPath}");
                return;
            }

            Environment.CurrentDirectory = Path.GetFullPath(vueLivestreamDirectoryPath);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python3",
                    Arguments = $"\"{scriptPath}\" \"{commitMessage}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    Console.Error.WriteLine(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                Color.DisplaySuccess("update_github_pages.py executed successfully!");
            }
            else
            {
                Color.DisplayError("update_github_pages.py failed.");
            }
        }
        catch (Exception ex)
        {
            Color.DisplayError("An error occurred while running update_github_pages.py.");
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
        try
        {
            string vuePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../VueLivestreamDirectory/python/fix_indentation.py"));

            if (!File.Exists(vuePath))
            {
                Console.WriteLine($"Warning: fix_indentation.py not found at path: {vuePath}");
                return;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python3",
                    Arguments = $"\"{vuePath}\"",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine("Python script output:\n" + output);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("Python script error:\n" + error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while running fix_indentation.py:");
            Console.WriteLine(ex.Message);
        }
    }


    // /**
    //  * Push changes that were made in the VueLivestreamDirectory folder
    //  */
    // public static void PushChangesInVue()
    // {
    //     string originalDirectory = Environment.CurrentDirectory;
    //     string vueLivestreamDirectoryPath = Path.Combine(originalDirectory, "../VueLivestreamDirectory");

    //     OpenFileInVSCode(vueLivestreamDirectoryPath);

    //     RunFixIndentationScript();

    //     Color.DisplaySuccess("PUSHING TO VueLivestreamDirectory!!!\n", "\n\n");

    //     Console.Write("Enter commit message (press 'p' to pass): ");
    //     string? commitMsg = Console.ReadLine()?.Trim();

    //     if (commitMsg?.ToLower() == "p")
    //     {
    //         Color.PrintLine("Push skipped.", "Magenta");
    //         return;
    //     }

    //     if (string.IsNullOrWhiteSpace(commitMsg))
    //     {
    //         Color.DisplayError("Commit message cannot be empty.");
    //         return;
    //     }

    //     try
    //     {
    //         Environment.CurrentDirectory = Path.GetFullPath(vueLivestreamDirectoryPath);
    //         ExecuteGitCommands(commitMsg);

    //         Console.WriteLine(""); //Whitespace
    //     }
    //     catch (Exception ex)
    //     {
    //         Color.DisplayError("Failed to push Vue changes.");
    //         Console.WriteLine(ex.ToString());
    //     }
    //     finally
    //     {
    //         Environment.CurrentDirectory = originalDirectory;
    //     }
    // }
    // 


}