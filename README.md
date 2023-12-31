# GetJustMyFiles

GetJustMyFiles recursively scans the Windows folders that start from a given source folder, copying on a destination folder only folders, extensions and files that aren't excluded by a list read from the textfile "exclusions.txt" (see the file for the autoexplaining syntax).  
GetJustMyFiles has a defaults management, that records on a text file the information input from the user, using them as default at the following execution.  
GetJustMyFiles Also works on net folders.  
GetJustMyFiles is a Console program written in C#, tested only in Windows.

## Unattended mode

Giving the parameters from the shell will copy all the content of the source, excluded the intended exclusions, to destination.  
Use:  
.\GetJustMyFiles SourceInitialFolder DestinationFolder  
If DestinationFolder is omitted, the program will save on the user's desktop.  

## User input mode

Giving no parameters at the shell will ask the user the input data.
The program will also ask for info about School's "Class", "Subject" and "Exercise", to scan the network folders of the students and automatically retrieve the students' exercitation programs.  
The scanning is based on the folder structure of the LAN server of Technical High School "ITT Blaise Pascal" in Cesena - Italy, so if you have a different structure, you will have to modify the program, or answer all "any" to the prompts "Class", "Subject" and "Exercise".   
Use:  
.\GetJustMyFiles  
Then answer the prompts.  
The program manages the defaults for user's inputs, in the tab separates values file "defaults.tsv".  
Void input will set the defaults read form said file.  
