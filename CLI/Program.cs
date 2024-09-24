using System;

class Program 
{
    static void Main(string[] args)
    {
        var indexer = new Indexer();

        // Index file command
        if (args.Length > 0 && args[0] == "index" && args[1] == "f" && args.Length == 3)
        {   
            // esto me imagino q esta bien 
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
        // Load Index command 
        else if (args.Length > 0 && args[0] == "load" && args[1] == "-p" && args.Length == 3)
        {
            // no tengo ni idea de q hacer en esto 
        }
        // 
        else if (args.Length > 0 && args[0] == "search" && args[1] == "-q" && args.Length >= 5 && args[args.Length - 2] == "-k")
        {
            // Capture the query terms between "-q" and "-k"
            string query = string.Join(" ", args.Skip(2).Take(args.Length - 4));
            int k;

            // Checking if the value of k is an int, if true continue else complain 
            if (int.TryParse(args[args.Length - 1], out k))
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
                Console.WriteLine("k must be of type: integer.");
            }
        }
        else
        {
            Console.WriteLine("Command not recognized. Use 'index -f <folder>', 'load -p <index_path>' or 'search -q <query> -k <k>'.");
        }
    }
}