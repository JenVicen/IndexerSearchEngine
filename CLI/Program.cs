using System;
using System.IO;
using System.Linq;

class Program 
{
    static void Main(string[] args)
    {
        var indexer = new Indexer();
        Console.WriteLine("Welcome to the Indexer/Search Engine CLI");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            var commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (commandParts.Length == 0) continue;

            string command = commandParts[0];
            switch (command.ToLower())
            {
                case "index":
                    if (commandParts.Length == 3 && commandParts[1] == "-f")
                    {
                        string folderPath = commandParts[2];
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
                    else
                    {
                        Console.WriteLine("Usage: index -f <folder>");
                    }
                    break;

                case "load":
                    if (commandParts.Length == 3 && commandParts[1] == "-p")
                    {
                        string indexPath = commandParts[2];
                        try
                        {
                            indexer.LoadIndex(indexPath);
                            Console.WriteLine("Index loaded successfully.");
                        }
                        catch (FileNotFoundException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading index: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Usage: load -p <index_path>");
                    }
                    break;

                case "search":
                    if (commandParts.Length >= 5 && commandParts[1] == "-q" && commandParts[^2] == "-k")
                    {
                        string query = string.Join(" ", commandParts.Skip(2).Take(commandParts.Length - 4));
                        if (int.TryParse(commandParts[^1], out int k))
                        {
                            try 
                            {
                                Console.WriteLine($"Searching for query: '{query}' with k = {k}"); // Debugging line
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
                                Console.WriteLine(ex.StackTrace); // Print stack trace for more context
                            }
                        }
                        else 
                        {
                            Console.WriteLine("k must be of type: integer.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Usage: search -q <query> -k <k>");
                    }
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Command not recognized. Use 'index -f <folder>', 'load -p <index_path>' or 'search -q <query> -k <k>'. Type 'exit' to quit.");
                    break;
            }
        }
    }
}