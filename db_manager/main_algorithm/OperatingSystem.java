package db_manager.main_algorithm;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

/**
 * This class helps with operating system processes 
 * @author Michael Totaro
 */
public class OperatingSystem {

    /**
     * Opens an application or file. 
     * @param path The path of the application
     * @param app The application to be opened 
     */
    public void openApp(String path, String app) {
        try {
            String[] cmd = {"open", "-a", app, path};
            Process process = new ProcessBuilder(cmd).start();
            process.waitFor();
        } catch (IOException | InterruptedException e) {
            printRedError("Application could not be opened.");
            e.printStackTrace();
        }
    }

    /**
     * Executes the git commands git add . , git commit, and git push. 
     */
    public void executeGitCommands() {
        try {
            ProcessBuilder processBuilder = new ProcessBuilder();

            //git add .
            processBuilder.command("git", "add", ".");

            Process process = processBuilder.start();
            int exitCode = process.waitFor();

            if (exitCode == 0) {
                MainAlgorithm.printWithColor("git add completed successfully", MainAlgorithm.Color.GREEN, "\n");
            } else {
                printRedError("git add failed");
                return;
            }

            // Run "git commit -m 'Adding files'"
            processBuilder.command("git", "commit", "-m", "Adding files");

            process = processBuilder.start();
            exitCode = process.waitFor();

            if (exitCode == 0) {
                MainAlgorithm.printWithColor("git commit completed successfully", MainAlgorithm.Color.GREEN, "\n");
            } else {
                printRedError("git commit failed");
                return;
            }

            // Run "git push"
            processBuilder.command("git", "push");

            process = processBuilder.start();
            exitCode = process.waitFor();

            if (exitCode == 0) {
                MainAlgorithm.printWithColor("git push completed successfully", MainAlgorithm.Color.GREEN, "\n");
            } else {
                printRedError("git push failed");
            }
        } catch (IOException | InterruptedException e) {
            printRedError("Error occured when attempted to push to GitHub");
            return;
        }
    }

    /**
     * Checks if file on the given file path exists 
     * @param filePath Filepath the file to be checked is on.
     * @return True if file exists, else false.
     */
    public boolean fileExists(String filePath) {
        Path path = Paths.get(filePath);
        return Files.exists(path);
    }

    /**
     * Prints a string colored red. 
     * @param str The string to be colored red.
     */
    private static void printRedError(String str) {
        System.out.println("\u001B[31m" + str + "\u001B[0m");
    }

}