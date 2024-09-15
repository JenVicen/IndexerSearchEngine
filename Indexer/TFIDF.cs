using System;
using System.Collections.Generic;
using System.Linq;

public class TFIDF
{
    public Dictionary<string, double> IDF { get; set; } = new Dictionary<string, double>();

    public void CalculateIDFAndTFIDF(IEnumerable<string> documents)
    {
        var termDocumentCount = new Dictionary<string, int>();
        int totalDocuments = documents.Count();

        foreach (var document in documents)
        {
            var terms = TokenizeAndNormalize(document);
            var uniqueTerms = terms.Distinct();

            foreach (var term in uniqueTerms)
            {
                if (!termDocumentCount.ContainsKey(term))
                {
                    termDocumentCount[term] = 0;
                }
                termDocumentCount[term]++;
            }
        }

        foreach (var term in termDocumentCount.Keys)
        {
            IDF[term] = Math.Log((double)totalDocuments / (1 + termDocumentCount[term]));
        }
    }

    public double[] CalculateQueryVector(IEnumerable<string> queryTerms)
    {
        var queryVector = new double[IDF.Count];
        int index = 0;

        foreach (var term in IDF.Keys)
        {
            queryVector[index++] = queryTerms.Count(t => t == term) * IDF[term];
        }

        return queryVector;
    }

    public static IEnumerable<string> TokenizeAndNormalize(string text)
    {
        // Implementa la lógica de tokenización y normalización aquí
        return text.ToLower().Split(new[] { ' ', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
