/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Testing
{
    public static void Main()
    {
        // Define the folder path to index
        string folderPath = "/Users/jennifervicentesvalle/Desktop/TTU/Object-Oriented Programming/IndexerSearchEngine/IndexerTesting";
        
        // Create an indexer instance
        Indexer indexer = new Indexer();
        
        // Index the folder
        try
        {
            indexer.IndexFolder(folderPath);
            Console.WriteLine("Indexing completed.");
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return; // Exit if the folder does not exist
        }
        
        // Display the IDF values
        Console.WriteLine("IDF Values:");
        foreach (var kvp in indexer.TFIDFHandler.IDFValues)
        {
            Console.WriteLine($"Term: {kvp.Key}, IDF: {kvp.Value}");
        }
        Console.WriteLine();

        // Calculate cosine similarity between the first two indexed documents
        if (indexer.Documents.Count >= 2)
        {
            CosineSimilarity cosineSimilarity = new CosineSimilarity();

            // Get the TF-IDF scores of the first two documents
            var document1 = indexer.Documents[0];
            var document2 = indexer.Documents[1];

            var tfidfScores1 = indexer.TFIDFHandler.DocumentTFIDFScores[document1];
            var tfidfScores2 = indexer.TFIDFHandler.DocumentTFIDFScores[document2];

            // Calculate cosine similarity
            double similarityScore = cosineSimilarity.CalculateCosineSimilarity(tfidfScores1, tfidfScores2);
            Console.WriteLine($"Cosine Similarity between '{document1.FileName}' and '{document2.FileName}': {similarityScore}");
        }
        else
        {
            Console.WriteLine("Not enough documents indexed to calculate similarity.");
        }
    }
}
*/
