using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Indexer/Search Engine CLI");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            string[] parts = input.Split(' ');

            if (parts.Length == 0) continue;

            switch (parts[0].ToLower())
            {
                case "index":
                    if (parts.Length == 3 && parts[1] == "-f")
                    {
                        IndexFolder(parts[2]);
                    }
                    else
                    {
                        Console.WriteLine("Usage: index -f <folder>");
                    }
                    break;

                case "load":
                    if (parts.Length == 3 && parts[1] == "-p")
                    {
                        LoadIndex(parts[2]);
                    }
                    else
                    {
                        Console.WriteLine("Usage: load -p <index_path>");
                    }
                    break;

                case "search":
                    if (parts.Length == 5 && parts[1] == "-q" && parts[3] == "-k")
                    {
                        Search(parts[2], int.Parse(parts[4]));
                    }
                    else
                    {
                        Console.WriteLine("Usage: search -q <query> -k <k>");
                    }
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Unknown command. Available commands: index, load, search, exit");
                    break;
            }
        }
    }

    static void IndexFolder(string folderPath)
    {
        // TODO: Implement indexing logic
        Console.WriteLine($"Indexing folder: {folderPath}");
    }

    static void LoadIndex(string indexPath)
    {
        // TODO: Implement index loading logic
        Console.WriteLine($"Loading index from: {indexPath}");
    }

    static void Search(string query, int k)
    {
        // TODO: Implement search logic
        Console.WriteLine($"Searching for: {query}, top {k} results");
    }
}
