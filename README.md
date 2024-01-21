# GetJustMyFiles
## A CLI program to retrieve files from a folder, excluding those unwanted

GetJustMyFiles recursively scans the Windows folders that start from a given source folder, copying on a destination folder only folders, extensions and files that aren't excluded by a list read from the textfile "exclusions.txt". (for the autoexplaining syntax, see the exclusions.txt file in distribution).  
GetJustMyFiles has a defaults management, that records on a text file the information input from the user, using them as default at the following execution.  
GetJustMyFiles also works on net folders, using mapped "disk" letter, DNS or Windows network name or IP address of the server that contains the net share.  
GetJustMyFiles is a Console program written in C#, under .Net 6. Being Console it can potentially run everywhere, but it has been tested only on Windows.

## Use
### Unattended mode

Giving the parameters from the shell will copy to destination all the content of the source folder, excluded the intended exclusions.

Use:  
C:> .\GetJustMyFiles SourceBaseFolder [DestinationBaseFolder]  

If DestinationBaseFolder is omitted, the program will save on the user's desktop.  

## User input mode

Giving no parameters at the shell, the program will ask the user the input data for source and destination folders.
The program will also ask for info about School's "Class", "Subject" and "Exercise", to scan the network folders of the students.  
The program will automatically retrieve the students' exercises, from folder:

SourceBaseFolder\Class\StudentsFirstName.StudentsLastName\Subject\Exercise

If you answer "any" to ONE of the prompts "Class", "Subject" or "Exercise", the program will NOT skip the files found in that folder.  
If you answer "any" to ALL of the prompts "Class", "Subject" and "Exercise", the program will work like when run unattended from the shell, not skipping any files.

The scanning is based on the folder structure of the LAN server of Technical High School "ITT Blaise Pascal" in Cesena - Italy, so if you have a different structure, you will have to modify the program, or answer all "any" to the prompts "Class", "Subject" and "Exercise".

Use:  
.\GetJustMyFiles  
Then answer the prompts.  
The program manages the defaults for user's inputs, kept in the tab separates values file "defaults.tsv".  
Null input will use the defaults shown by the U.I.  

## License and disclaimer

GetJustMyFiles is free software under the GPL v.2 license.  
Of course there is no guarantee of whatsoever kind.  
If you find errors or want modifications of functionalities, please write it to:

gamon@ingmonti.it 

and be very (VERY!) patient.  
Or (way better solution) fork the repo from:

https://github.com/gamondue/GetJustMyFiles

modify by yourself and pull request your fixes!
