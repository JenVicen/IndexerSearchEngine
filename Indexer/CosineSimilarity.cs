using System;
using System.Collections.Generic;
using System.Linq;

public class CosineSimilarity
{
    private readonly TFIDF _tfidfHandler;

    public CosineSimilarity(TFIDF tfidfHandler)
    {
        _tfidfHandler = tfidfHandler;
    }

    // Method to calculate the cosine similarity between two documents
    public double Calculate(Document doc1, Document doc2)
    {
        var tfidf1 = _tfidfHandler.CalculateTFIDF(doc1);
        var tfidf2 = _tfidfHandler.CalculateTFIDF(doc2);

        double dotProduct = tfidf1.Keys.Intersect(tfidf2.Keys)
            .Sum(term => tfidf1[term] * tfidf2[term]);

        double magnitude1 = Math.Sqrt(tfidf1.Values.Sum(val => val * val));
        double magnitude2 = Math.Sqrt(tfidf2.Values.Sum(val => val * val));

        if (magnitude1 == 0 || magnitude2 == 0) return 0; // Avoid division by zero

        return dotProduct / (magnitude1 * magnitude2);
    }
}
