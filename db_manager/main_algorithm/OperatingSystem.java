package db_manager.main_algorithm;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

public class OperatingSystem {
    public void clear() {

    }

    public void openApp(String path, String app) {
        try {
            String[] cmd = {"open", "-a", app, path};
            Process process = new ProcessBuilder(cmd).start();
            process.waitFor();
        } catch (IOException | InterruptedException e) {
            e.printStackTrace();
        }
    }

    public void chdir(String newPath) throws IOException {
        Path newDirectory = Paths.get(newPath).toAbsolutePath();

        if (newDirectory.toFile().isDirectory()) {
            System.setProperty("user.dir", newDirectory.toString());
        } else {
            printRedError("Failed to change directories");
            System.exit(0);
        }
    }

    public void executeGitCommands() throws IOException, InterruptedException {
        ProcessBuilder processBuilder = new ProcessBuilder();

        //git add .
        processBuilder.command("git", "add", ".");

        Process process = processBuilder.start();
        int exitCode = process.waitFor();

        if (exitCode == 0) {
            System.out.println("git add completed successfully");
        } else {
            printRedError("git add failed");
            return;
        }

        // Run "git commit -m 'Adding files'"
        processBuilder.command("git", "commit", "-m", "Adding files");

        process = processBuilder.start();
        exitCode = process.waitFor();

        if (exitCode == 0) {
            System.out.println("git commit completed successfully");
        } else {
            printRedError("git commit failed");
            return;
        }

        // Run "git push"
        processBuilder.command("git", "push");

        process = processBuilder.start();
        exitCode = process.waitFor();

        if (exitCode == 0) {
            System.out.println("git push completed successfully");
        } else {
            printRedError("git push failed");
        }
    }


    public boolean fileExists(String filePath) {
        Path path = Paths.get(filePath);
        return Files.exists(path);
    }

    private static void printRedError(String str) {
        System.out.print("\u001B[31m" + str + "\u001B[0m");
    }

}