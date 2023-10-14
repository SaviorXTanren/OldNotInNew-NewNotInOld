namespace OldNotInNew_NewNotInOld
{
    public class Program
    {
        public const string OldShaInputFilename = "Old.sha1.txt";
        public const string NewShaInputFileName = "New.sha1.txt";

        private static readonly HashSet<char> validShaHashCharacters = new HashSet<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        private const string OldNotInNewOutputFileName = "OldNotInNew.txt";
        private const string NewNotInOldOuptutFileName = "NewNotInOld.txt";

        public static void Main(string[] args)
        {
            var results = ProcessOldAndNewFiles(OldShaInputFilename, NewShaInputFileName);
            if (results != null)
            {
                File.WriteAllLines(OldNotInNewOutputFileName, results.Item1);
                File.WriteAllLines(NewNotInOldOuptutFileName, results.Item2);
            }
        }

        public static Tuple<List<string>, List<string>> ProcessOldAndNewFiles(string oldShaInputFilePath, string newShaInputFilePath)
        {
            if (!File.Exists(oldShaInputFilePath) || !File.Exists(newShaInputFilePath))
            {
                Console.WriteLine("ERROR: Old or New SHA file does not exist");
                return null;
            }

            if (!ParseFile(oldShaInputFilePath, out Dictionary<string, List<string>> oldShaFiles) ||
                !ParseFile(newShaInputFilePath, out Dictionary<string, List<string>> newShaFiles))
            {
                return null;
            }

            List<string> oldNotInNewFiles = new List<string>();
            foreach (var oldShaFile in oldShaFiles)
            {
                if (!newShaFiles.ContainsKey(oldShaFile.Key))
                {
                    foreach (var file in oldShaFile.Value)
                    {
                        oldNotInNewFiles.Add(file);
                    }
                }
            }

            List<string> newNotInOldFiles = new List<string>();
            foreach (var newShaFile in newShaFiles)
            {
                if (!oldShaFiles.ContainsKey(newShaFile.Key))
                {
                    foreach (var file in newShaFile.Value)
                    {
                        newNotInOldFiles.Add(file);
                    }
                }
            }

            return new Tuple<List<string>, List<string>>(oldNotInNewFiles, newNotInOldFiles);
        }

        private static bool ParseFile(string filename, out Dictionary<string, List<string>> files)
        {
            files = new Dictionary<string, List<string>>();
            try
            {
                string[] lines = File.ReadAllLines(filename);
                if (lines != null && lines.Length > 0)
                {
                    foreach (string line in lines)
                    {
                        bool validLine = false;

                        // Verify the first space index is 40 characters into the line, signifying we have a 40 character hash
                        int breakIndex = line.IndexOf(' ');
                        if (breakIndex == 40)
                        {
                            string hash = line.Substring(0, breakIndex).ToLowerInvariant();
                            string filePath = line.Substring(breakIndex + 1);

                            // Validate each character in the hash is a valid hex character
                            if (hash.All(c => validShaHashCharacters.Contains(c)))
                            {
                                // Multiple files could have the same hash in a backup, so store list of files that contain the same hash
                                if (!files.ContainsKey(hash))
                                {
                                    files[hash] = new List<string>();
                                }
                                files[hash].Add(filePath);

                                validLine = true;
                            }
                        }

                        if (!validLine)
                        {
                            Console.WriteLine($"ERROR: Failed to parse file {filename} - Invalid line: {line}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to parse file {filename} - {ex}");
                return false;
            }
            return true;
        }
    }
}