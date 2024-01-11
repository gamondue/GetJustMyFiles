using System.IO;

namespace GetJustMyFiles
{
    /// <summary>
    /// Reads files from a network share or a folder, copying only those interesting for the user 
    /// prof. Gabriele MONTI - ITT Pascal - Cesena - Italy 
    /// </summary>
    class Program
    {
        private static string _sourceInitialFolder;
        private static string _destinationInitialFolder;
        private static string _destinationFolder;
        private static string _className;
        private static string _subjectName;
        private static string _exerciseFolderName;

        static int _folderRecursionLevel = 0;
        static int _foldersWhereSkippingFiles = 4;
        static int _nFoldersFound = 0;

        static HashSet<string> foldersExclusionsList = new HashSet<string>();
        static HashSet<string> extensionsExclusionsList = new HashSet<string>();
        static HashSet<string> filesExclusionsList = new HashSet<string>();

        private static string fileExclusions = @".\exclusions.txt";
        //private static string fileInclusions = @".\inclusions.txt";
        private static string fileDefaults = @".\defaults.tsv";

        /// <summary>
        /// Copies all the files of a source directory and sons into a destination directory
        /// Doesn't copy some folders not useful for the user 
        /// </summary>
        /// <param name="args">
        /// args[0] = source path 
        /// args[1] = destination path
        /// With no parameters given asks the class and the subject 
        /// </param>
        static void Main(string[] args)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine($"GetJustMyFiles v.{version}");
            ReadDefaults(args);
            ReadExclusions();
            //ReadInclusions();
            bool canCopy = true;
            if (args.Length == 0)
            {   // no parameters, ask the user 

                //Console.WriteLine("\nUnattended use: .\\GetJustMyFiles <Source initial folder> <Destination folder>");
                //Console.WriteLine("If <Destination folder> is omitted will save on desktop");
                //Console.WriteLine("Giving no parameters will ask the user. Void input will set the default");
                //Console.WriteLine("Giving all \"any\" to Class, Subject and Exercize will copy ALL subdirectories.");

                Console.WriteLine("\nUso senza UI: .\\GetJustMyFiles <Cartella sorgente> <Cartella di destinazione>");
                Console.WriteLine("Se si omette <Cartella di destinazione> salva sul desktop");
                Console.WriteLine("Se non si danno parametri da shell il programma chiede i dati all'utente. Risposte vuote useranno il default.");
                Console.WriteLine("Rispondere con tutti \"any\" a Classe, Materia e Cartella dell'esercizio, copia cartella e TUTTE le sottocartelle.");

                //Console.Write("\nSource base folder (default " + _sourceInitialFolder + "): ");
                Console.Write("\nCartella base sorgente (default " + _sourceInitialFolder + "): ");

                string dummy = Console.ReadLine();
                if (dummy != "")
                    _sourceInitialFolder = dummy;
                //Console.Write("Should I add today's date in source directory? (y/n) default n: ");
                //dummy = Console.ReadLine();
                //addDateInSourceFolder = false;
                //if (dummy != "")
                //    addDateInSourceFolder = (dummy.Substring(0, 1) == "y");
                Console.WriteLine();
                //Console.Write("Destination base folder (ex. give Z: for network share)\n(default " + _destinationInitialFolder + "): ");
                Console.Write("Cartella base di destinazione (per share di rete, dare Z:)\n(default " + _destinationInitialFolder + "): ");
                dummy = Console.ReadLine();
                if (dummy != "")
                    _destinationInitialFolder = dummy;
                _destinationFolder = _destinationInitialFolder;

                //Console.Write("Class (give 'any' for any class folder, Enter for default)\n(default " + _className + "): ");
                Console.Write("Classe (dare 'any' per tutte, Enter per default)\n(default " + _className + "): ");
                dummy = Console.ReadLine();
                if (dummy != "" && dummy != "any")
                    _className = dummy;
                else if (dummy.ToLower() == "any" || _className == "any")
                    _className = "";

                //Console.Write("Subject (give 'any' for any subject folder, Enter for default)\n(default " + _subjectName + "): ");
                Console.Write("Materia (dare 'any' per tutte, Enter per default)\n(default " + _subjectName + "): ");
                dummy = Console.ReadLine();
                if (dummy != "" && dummy != "any")
                    _subjectName = dummy;
                else if (dummy.ToLower() == "any" || _subjectName == "any")
                    _subjectName = "";

                //Console.Write("Exercise's folder partial name (give 'any' for any folder, Enter for default)\n(default " +
                Console.Write("Cartella dell'esercizio (dare 'any' per tutte, Enter per default)\n(default " +
                    _exerciseFolderName + "): ");
                dummy = Console.ReadLine();
                string dummyLower = dummy.ToLower();
                if (dummyLower != "" && dummyLower != "any")
                    _exerciseFolderName = dummy;
                if (dummyLower == "any" || _exerciseFolderName == "any")
                    _exerciseFolderName = "";
                if (args.Length == 0)
                    SaveDefaults();
                string NewDestinationFolderName = "";
                if (_className != "")
                    NewDestinationFolderName += _className + "_";
                if (_subjectName != "")
                    NewDestinationFolderName += _subjectName + "_";
                if (_exerciseFolderName != "")
                    NewDestinationFolderName += _exerciseFolderName;
                NewDestinationFolderName = Path.Combine(NewDestinationFolderName, DateTime.Now.ToString("yyyy-MM-dd"));
                //NewDestinationFolderName = NewDestinationFolderName.Replace("\\", "");

                if (NewDestinationFolderName != null)
                    _destinationFolder = Path.Combine(_destinationFolder, NewDestinationFolderName);
                if (!ShowTargetFolders())
                    canCopy = false;
            }
            else
            {
                Console.WriteLine("Cartella sorgente : " + _sourceInitialFolder);
                Console.WriteLine("Cartella destinaz.: " + _destinationInitialFolder);
                _destinationFolder = _destinationInitialFolder;
                canCopy = true;
            }
            if (canCopy)
            {
                if (_className == "")
                    _foldersWhereSkippingFiles--;
                if (_subjectName == "")
                    _foldersWhereSkippingFiles--;
                if (_exerciseFolderName == "")
                    _foldersWhereSkippingFiles--;
                // starts copying 
                //Console.WriteLine("Copying from " + _sourceInitialFolder + " to " + _destinationFolder);
                Console.WriteLine("Copia da " + _sourceInitialFolder + " a " + _destinationFolder);
                //Console.WriteLine(_destinationFolder);
                Directory.CreateDirectory(_destinationFolder);
                Console.WriteLine();
                //Console.WriteLine("Directories being created and files being copied:");
                Console.WriteLine("Cartelle create e file copiati:");
                CopyFilesInThisFolderAndChildFolders(_sourceInitialFolder, _destinationFolder);
            }
            Console.WriteLine();
            //Console.WriteLine("Done!");
            Console.WriteLine("Finito!");
            Console.ReadLine();
        }
        private static bool ShowTargetFolders()
        {
            bool copy = true;
            string folderToScan = Path.Combine(_sourceInitialFolder, _className);
            if (!Directory.Exists(folderToScan))
            {
                //Console.WriteLine("Folder to scan does not exist!");
                Console.WriteLine("La cartella da scansionare (" + folderToScan + " non esiste!");
                copy = false;
                return copy;
            }
            if (_exerciseFolderName != "")
            {
                bool repeatListing = true;
                string readResponse = "";
                while (repeatListing)
                {
                    //Console.WriteLine("List of all {0} folders found in given base folder",
                    Console.WriteLine("Lista di tutte le cartelle {0} trovate nella cartella base",
                    _subjectName + "\\*" + _exerciseFolderName + "*");
                    _nFoldersFound = 0;
                    ShowFoldersWithPartialNameInside(folderToScan);
                    //Console.WriteLine("No of folders: {0}", _nFoldersFound);
                    Console.WriteLine("Numero di cartelle {0}:", _nFoldersFound);
                    //Console.WriteLine("Input c and Enter to continue and copy, any key and Enter to repeat the list, q to quit");
                    Console.WriteLine("Immettere 'c' e Enter per cominciare a copiare, q per uscire, altro per ripetere la lettura della lista");
                    readResponse = Console.ReadLine();
                    if (readResponse != "")
                        readResponse = readResponse.Substring(0, 1).ToLower();
                    repeatListing = (readResponse == "") || readResponse != "c" && readResponse != "q";
                }
                copy = readResponse == "c";
            }
            return copy;
        }
        private static void ShowFoldersWithPartialNameInside(string SourceFolder)
        {
            string justName = Path.GetFileName(SourceFolder);
            string justNameToLower = justName.ToLower();
            string ExercisesFolderPartialNameToLower = _exerciseFolderName.ToLower();
            if (justNameToLower.Contains(ExercisesFolderPartialNameToLower))
            {
                Console.WriteLine(SourceFolder);
                _nFoldersFound++;
                // returns to avoid showing more than once the application name in subfolders
                return;
            }
            string[] allFolders = Directory.GetDirectories(SourceFolder);
            foreach (string sourceNewFolder in allFolders)
            {
                ShowFoldersWithPartialNameInside(sourceNewFolder);
            }
        }
        private static void CopyFilesInThisFolderAndChildFolders(string SourceFolderName,
            string DestinationFolderName)
        {
            // copy source files in the destination directory
            // (those in lower levels are not copied, based on the number of parameters given)

            if (_folderRecursionLevel >= _foldersWhereSkippingFiles - 1)
            {
                // read all files in this folder
                string[] allFiles = Directory.GetFiles(SourceFolderName);
                foreach (string file in allFiles)
                {
                    string justName = Path.GetFileName(file);
                    string destinationFile = "";
                    if (justName != null)
                        destinationFile = Path.Combine(DestinationFolderName, justName);

                    bool excludedFile = false;
                    if (filesExclusionsList.Contains(Path.GetFileName(file.ToLower())))
                        excludedFile = true;
                    if (extensionsExclusionsList.Contains(Path.GetExtension(file).ToLower()))
                        excludedFile = true;

                    // when traversing the students' net folders, we shouldn't copy the files that stay in 
                    // <last name>.<first name>, and <Subject> folders (should copy files in <Exercise> folder)
                    // !!!! TODO !!!! improve performance of the following 
                    if (Path.GetFileName(SourceFolderName) == _className || Path.GetFileName(SourceFolderName) == _subjectName)
                        excludedFile = true;

                    // copy if not previously excluded
                    if (!excludedFile)
                    {
                        //Console.WriteLine("Source directory: " + SourceFolderName);
                        Console.WriteLine("Cartella sorgente : " + SourceFolderName);
                        //Console.WriteLine("Destination dir.: " + DestinationFolderName);
                        Console.WriteLine("Cartella destinaz.: " + DestinationFolderName);
                        // copy with overwrite
                        Console.WriteLine("File: " + justName);
                        //string dir = Path.GetDirectoryName(destinationFile);
                        if (!Directory.Exists(DestinationFolderName))
                            Directory.CreateDirectory(DestinationFolderName);
                        File.Copy(file, destinationFile, true);
                    }
                }
            }
            string[] allFolders = Directory.GetDirectories(SourceFolderName);
            string classNameLower = _className.ToLower();
            // treat daughter folders 
            // copy all folders in this folder, filtering out those that we have to filter.. 
            _folderRecursionLevel++;
            try
            {
                foreach (string sourceNewFolder in allFolders)
                {
                    bool shouldRecurseThisDirectory = true;
                    var justTheLastFolderInPath = new DirectoryInfo(sourceNewFolder).Name;
                    var justTheLastFolderInPathLower = justTheLastFolderInPath.ToLower();
                    switch (_folderRecursionLevel)
                    {
                        case 1:
                            {   // class Name in source directory 
                                if (classNameLower != "")
                                {
                                    // if class has been specified and the level is 1, we keep only the
                                    // subfolder that has the right ClassName 
                                    if (justTheLastFolderInPathLower != classNameLower)
                                    {
                                        shouldRecurseThisDirectory = false;
                                    }
                                }
                                break;
                            }
                        case 2:
                            {   // student's Name in source directory 
                                // nothing: the folders of the students' names get always recursed,
                                // unless they are taken out in the following, if they are in the list of excluded folders 
                                break;
                            }
                        case 3:
                            {   // subject's Name in source directory 
                                // if subject has been specified, we keep only the "subject" subfolder
                                // we recurse only if we have the subject and the subject given by user
                                // is the same as the current directory
                                // if subject hasn't been specified, we recurse all this level 
                                if (_subjectName != "")
                                {
                                    // we continue examining the folder only if it has the name of the subject 
                                    if (justTheLastFolderInPathLower != _subjectName.ToLower())
                                    {
                                        shouldRecurseThisDirectory = false;
                                    }
                                }
                                break;
                            }
                        case 4:
                            {   // exercise folder's Name in source directory 
                                // if we have a exercise folder we recurse only the folder that have the 
                                // partial name inside 
                                if (_exerciseFolderName != "")
                                {
                                    if (!justTheLastFolderInPathLower.Contains(_exerciseFolderName.ToLower()))
                                    {
                                        shouldRecurseThisDirectory = false;
                                    }
                                }
                                break;
                            }
                            // after level 4 any folder and file non excludable will be copied 
                    }
                    // some always unuseful folders will not be copied regardless their level! 
                    if (foldersExclusionsList.Contains(justTheLastFolderInPathLower))
                    {
                        shouldRecurseThisDirectory = false;
                    }
                    if (shouldRecurseThisDirectory)
                    {
                        string localNewFolder = DestinationFolderName;
                        // we DON'T create a _className folder, unless the _className is not there
                        // (it is implicit and also in the name of the main local subdirectory) 
                        if (_folderRecursionLevel != 1 || _className == "")
                            localNewFolder = Path.Combine(DestinationFolderName, justTheLastFolderInPath);
                        CopyFilesInThisFolderAndChildFolders(sourceNewFolder, localNewFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (ex.GetType() == )
                Console.WriteLine(ex.Message);
                return;
            }
            _folderRecursionLevel--;
        }
        private static void ReadDefaults(string[] args)
        {
            if (!File.Exists(fileDefaults))
            {
                // default file does not exist, make program's defaults
                _sourceInitialFolder = @"\\10.0.0.10\Studenti\";
                _destinationInitialFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                _className = "3E";
                _subjectName = "Informatica";
                _exerciseFolderName = "Prova";
                return;
            }
            string[] lines = File.ReadAllLines(fileDefaults);
            if (lines.Length == 0)
            {
                // default file has no rows, make program's defaults
                _sourceInitialFolder = @"\\10.0.0.10\Studenti\";
                _destinationInitialFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                _className = "3E";
                _subjectName = "Informatica";
                _exerciseFolderName = "Prova";
                return;
            }
            else
            {
                _sourceInitialFolder = (lines[0].Split('\t'))[1].Trim();
                _destinationInitialFolder = (lines[1].Split('\t'))[1].Trim();
                _className = (lines[2].Split('\t'))[1].Trim();
                _subjectName = (lines[3].Split('\t'))[1].Trim();
                _exerciseFolderName = (lines[4].Split('\t'))[1].Trim();
            }
            if (args.Length == 2)
            {
                _sourceInitialFolder = args[0];
                _destinationInitialFolder = args[1];
                _className = "";
                _subjectName = "";
                _exerciseFolderName = "";
            }
            else if (args.Length == 1)
            {
                _sourceInitialFolder = args[0];
                _destinationInitialFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                _className = "";
                _subjectName = "";
                _exerciseFolderName = "";
            }
        }
        private static void SaveDefaults()
        {
            string[] defaults = new string[5];
            defaults[0] = "SourceInitialFolder\t" + _sourceInitialFolder;
            defaults[1] = "DestinationFolder\t" + _destinationInitialFolder;
            if (_className == "")
                _className = "any";
            defaults[2] = "Class\t" + _className;
            if (_subjectName == "")
                _subjectName = "any";
            defaults[3] = "Subject\t" + _subjectName;
            if (_exerciseFolderName == "")
                _exerciseFolderName = "any";
            defaults[4] = "Exercise folder\t" + _exerciseFolderName;
            File.WriteAllLines(fileDefaults, defaults);
        }
        private static void ReadExclusions()
        {
            if (!File.Exists(fileExclusions))
            {
                foldersExclusionsList.Clear();
                extensionsExclusionsList.Clear();
                filesExclusionsList.Clear();
                Console.WriteLine("\nFile 'exclusions.txt' non existent." +
                    "\nProgram will not exclude any files or folder.");
                return;
            }
            string[] lines = File.ReadAllLines(fileExclusions);
            if (lines.Length == 0)
            {
                foldersExclusionsList.Clear();
                extensionsExclusionsList.Clear();
                filesExclusionsList.Clear();
                Console.WriteLine("File 'exclusions.txt' unreadable." +
                    "\nProgram will not exclude any files or folder.");
                return;
            }
            //if (lines[0].Substring(0, 4) != "- fo")
            //{
            //    foldersExclusionsList.Clear();
            //    extensionsExclusionsList.Clear();
            //    filesExclusionsList.Clear();
            //    Console.WriteLine("Format of file 'exclusions.txt' wrong." +
            //        "\nProgram will not exclude any files or folder.");
            //    return;
            //}
            int index = 0;
            bool exit = false;
            do
            {
                if (lines[index] != "")
                {
                    int pos = lines[index].IndexOf('#');
                    string line = lines[index];
                    string firstChars = "";
                    if (lines[index] != "" && pos >= 0)
                    {
                        firstChars = lines[index].Substring(0, pos);
                        if (firstChars == "- ex")
                        {
                            exit = true;
                            break;
                        }
                    }
                    if (line != "" && firstChars != "- fo" && firstChars != "- ex")
                    {
                        foldersExclusionsList.Add(line.Trim().ToLower());
                    }
                }
                index++;
            } while (!exit);
            exit = false;
            do
            {
                string line = lines[index].Substring(0, lines[index].IndexOf('#'));
                string firstChars = line.Substring(0, 4);
                if (line != "")
                {
                    if (firstChars == "- ex")
                        exit = true;
                    else
                        extensionsExclusionsList.Add(line.Trim().ToLower());
                }
                index++;
            } while (!exit);
            while (index < lines.Length)
            {
                string line = lines[index].Substring(0, lines[index].IndexOf('#'));
                string firstChars = line.Substring(0, 4);
                if (line != "")
                {
                    if (firstChars == "- ex")
                        exit = true;
                    else
                        filesExclusionsList.Add(lines[index].Trim().ToLower());
                }
                index++;
            }
        }
        private static void ReadInclusions()
        {
            //if (!File.Exists(fileDefaults))
            //{

            //    return;
            //}
            //return;
        }
    }
}
