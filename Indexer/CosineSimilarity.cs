using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class CosineSimilarity
{
    // Method to calculate cosine similarity between two documents based on their TF-IDF vectors
    public double CalculateCosineSimilarity(Dictionary<string, double> tfidfVector1, Dictionary<string, double> tfidfVector2)
    {
        // Step 1: Calculate the dot product of the two vectors
        double dotProduct = 0;
        foreach (var term in tfidfVector1.Keys)
        {
            if (tfidfVector2.ContainsKey(term))
            {
                dotProduct += tfidfVector1[term] * tfidfVector2[term];
            }
        }

        // Step 2: Calculate the magnitude of the two vectors
        double magnitudeA = Math.Sqrt(tfidfVector1.Values.Sum(x => x * x));
        double magnitudeB = Math.Sqrt(tfidfVector2.Values.Sum(x => x * x));

        // Step 3: Calculate the cosine similarity
        if (magnitudeA == 0 || magnitudeB == 0)
        {
            // Return 0 if one of the vectors has no magnitude (to avoid division by zero)
            return 0.0;
        }

        return dotProduct / (magnitudeA * magnitudeB);
    }
}
