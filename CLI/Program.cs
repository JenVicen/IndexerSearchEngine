using System;

class Program
{
    static void Main(string[] args)
    {
        var indexer = new Indexer();

        if (args.Length > 0 && args[0] == "index" && args[1] == "-f" && args.Length == 3)
        {
            string folderPath = args[2];
            try
            {
                indexer.IndexFolder(folderPath);
                Console.WriteLine("Indexing completed.");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error indexing folder: {ex.Message}");
            }
        }

        else if (args.Length > 0 && args[0] == "load" && args[1] == "-p" && args.Length == 3)
        {
            string indexPath = args[2];
            try
            {
                indexer.LoadIndex(indexPath);
                Console.WriteLine("Index loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading index: {ex.Message}");
            }
        }

        else if (args.Length > 0 && args[0] == "search" && args[1] == "-q" && args.Length == 5 && args[3] == "-k")
        {
            string query = args[2];
            int k;
            if (int.TryParse(args[4], out k))
            {
                try
                {
                    var results = indexer.Search(query, k);
                    Console.WriteLine($"Results for '{query}':");
                    foreach (var result in results)
                    {
                        Console.WriteLine(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error searching: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("k must be an integer.");
            }
        }
        else
        {
            Console.WriteLine("Command not recognized. Use 'index -f <folder>', 'load -p <index_path>' or 'search -q <query> -k <k>'.");
        }
    }
}
