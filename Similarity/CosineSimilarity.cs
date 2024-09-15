using System;
using System.Collections.Generic;
using System.Linq;

public static class CosineSimilarity
{
    public static double Calculate(double[] vectorA, string document)
    {
        var terms = TFIDF.TokenizeAndNormalize(document);
        var termFrequency = new Dictionary<string, int>();

        foreach (var term in terms)
        {
            if (!termFrequency.ContainsKey(term))
            {
                termFrequency[term] = 0;
            }
            termFrequency[term]++;
        }

        double[] vectorB = new double[vectorA.Length];
        int index = 0;

        foreach (var term in termFrequency.Keys)
        {
            vectorB[index++] = termFrequency[term]; // Assuming term frequency as the vector component
        }

        return CalculateCosine(vectorA, vectorB);
    }

    private static double CalculateCosine(double[] vectorA, double[] vectorB)
    {
        double dotProduct = 0;
        double magnitudeA = 0;
        double magnitudeB = 0;

        for (int i = 0; i < vectorA.Length; i++)
        {
            dotProduct += vectorA[i] * vectorB[i];
            magnitudeA += Math.Pow(vectorA[i], 2);
            magnitudeB += Math.Pow(vectorB[i], 2);
        }

        if (magnitudeA == 0 || magnitudeB == 0) return 0;

        return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
    }
}
