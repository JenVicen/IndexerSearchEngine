using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public abstract class Similarity 
{
    // Abstract method implemented by both Children classes: Euclidean and Cosine
    public abstract double CalculateSimilarity(Dictionary<string, double> Vector1, Dictionary<string, double> Vector2);
}

public class EuclideanSimilarity : Similarity
{
    // Override method to write the new implementation
    public override double CalculateSimilarity(Dictionary<string, double> Vector1, Dictionary<string, double> Vector2)
    {
        // If an null vector is passed the return value should be 0, to prevent exception calls
        if (Vector1 == null || Vector2 == null) return 0.0; 
        // Get the Sum of the squared difference between Vector1 and Vector2 values
        double sumOfSquaredDifferences = 0;
        foreach (var term in Vector1.Keys)
        {
            double val1 = Vector1.ContainsKey(term) ? Vector1[term] : 0;
            double val2 = Vector2.ContainsKey(term) ? Vector2[term] : 0;
            sumOfSquaredDifferences += Math.Pow(val1 - val2, 2);
        }

        // Calculate the distance between the Euclidean Distance and return the value
        double euclideanDistance = Math.Sqrt(sumOfSquaredDifferences);
        return 1 / (1 + euclideanDistance);
    }
}
 
public class CosineSimilarity : Similarity
{
    public override double CalculateSimilarity(Dictionary<string, double> Vector1, Dictionary<string, double> Vector2)
    {
        // Checking for null vectors again 
        if (Vector1 == null || Vector2 == null) return 0.0; 
        // Step 1: Calculate the dot product of the two vectors
        double dotProduct = 0;
        foreach (var term in Vector1.Keys)
        {
            if (Vector2.ContainsKey(term))
            {
                dotProduct += Vector1[term] * Vector2[term];
            }
        }

        // Step 2: Calculate the magnitude of the two vectors
        double magnitudeA = Math.Sqrt(Vector1.Values.Sum(x => x * x));
        double magnitudeB = Math.Sqrt(Vector2.Values.Sum(x => x * x));

        // Step 3: Calculate the cosine similarity
        if (magnitudeA == 0 || magnitudeB == 0)
        {
            // Return 0 if one of the vectors has no magnitude (to avoid division by zero)
            return 0.0;
        }

        return dotProduct / (magnitudeA * magnitudeB);
    }
}
