# GetJustMyFiles v.0.9.2

## A console program to retrieve file from a folder, excluding those unwanted

GetJustMyFiles recursively scans the Windows folders that start from a given source folder, copying on a destination folder only folders, extensions and files that aren't excluded by a list read from the textfile "exclusions.txt". (see the exclusions.txt file for the autoexplaining syntax).  
GetJustMyFiles has a defaults management, that records on a text file the information input from the user, using them as default at the following execution.  
GetJustMyFiles also works on net folders, using mapped letter, dns name or IP address of the server that contains the net share.  
GetJustMyFiles is a Console program written in C#, tested only in Windows.

## Unattended mode

Giving the parameters from the shell will copy to destination all the content of the source folder, excluded the intended exclusions.

Use:  
.\GetJustMyFiles SourceInitialFolder [DestinationFolder]  
If DestinationFolder is omitted, the program will save on the user's desktop.  

## User input mode

Giving no parameters at the shell will ask the user the input data for source and destination folders.
The program will also ask for info about School's "Class", "Subject" and "Exercise", to scan the network folders of the students.  
The program will automatically retrieve the students' exercitations, stored in folder:

<base path given by the user>\Class\<Student's Last Name>.<Student's First Name>\Subject\Exercise.

If you answer "any" to all the prompts "Class", "Subject" and "Exercise", the program will work like when run unattended from the shell. 

The scanning is based on the folder structure of the LAN server of Technical High School "ITT Blaise Pascal" in Cesena - Italy, so if you have a different structure, you will have to modify the program, or answer all "any" to the prompts "Class", "Subject" and "Exercise".

Use:  
.\GetJustMyFiles  
Then answer the prompts.  
The program manages the defaults for user's inputs, in the tab separates values file "defaults.tsv".  
Null input will use the defaults shown by the U.I.  

## License and disclaimer

GetJustMyFiles is free software under the GPL v.2 licence.  
Of course there is no guarantee of the program to be working nor compliance with specifications.  
If you find errors or want modifications on functionalities, please write to:

gamon@ingmonti.it 

being very (VERY!) patient.  
Or (way better) fork the repo:

https://github.com/gamondue/GetJustMyFiles

and pull request your fixes!