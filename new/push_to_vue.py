
import subprocess
from termcolor import colored as c
import os 

def push_to_vue():
    os.chdir("../../VueLivestreamDirectory")
    os.system('code .')

    print(c("\n\n✅ PUSHING TO VueLivestreamDirectory!!!", 'green'))

    commit_message = input("\nEnter commit message (press 'p' to pass): ")

    if commit_message.strip().upper() == 'P':
        exit(0)


    subprocess.run(["git", "add", "."], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=True)
    print(c("✅ git add completed successfully", 'green'))


    subprocess.run(["git", "commit", "-m", commit_message], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=True)
    print(c("✅ git commit completed successfully", 'green'))


    subprocess.run(["git", "push"], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=True)
    print(c("✅ git push completed successfully", 'green'))

