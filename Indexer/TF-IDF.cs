using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using iText.Forms.Fields;

public class TFIDF 
{   
    public Dictionary<string, double> IDFValues {get; set; } = new Dictionary<string, double>();
    public Dictionary<Document, Dictionary<string, double>> DocumentTFIDFScores {get; private set; } = new Dictionary<Document, Dictionary<string, double>>(); 
    
    // Calculates the IDF and TFIDF of all terms in a list of documents
    public void CalculateIDFandTFIDF(List<Document> documents)
    {
        int totalDocuments = documents.Count;
        var IDFValuesCount = new Dictionary<string,int> ();

        // Calculate IDF of all terms
        foreach (var document in documents)
        {   
            // Gets all unique terms in the document
            var uniqueTerms = document.GetUniqueTerms();

            // Seeing if the term needs to be added to the list of terms
            // of all documents, and incrementing count of that term
            foreach (var term in uniqueTerms)
            {
                if (!IDFValuesCount.ContainsKey(term))
                {
                    IDFValuesCount[term] = 0;
                }
                IDFValuesCount[term]++;
            }
        }

        // Calculating the IDF
        foreach (var term in IDFValuesCount.Keys)
        {
            IDFValues[term] = Math.Log((double) totalDocuments / (1 + IDFValuesCount[term]));
        }

        // After calculating the IDF we can then begin calculating the TFIDF of all documents
        foreach (var document in documents)
        {
            var tfidfScores = CalculateTFIDF(document);
            DocumentTFIDFScores[document] = tfidfScores;
        }
    }

    // Function that calculates the TFScores of terms inside a list of terms 
    private Dictionary<string, double> CalculateTFScores(Document document){
        var tfScores = new Dictionary<string, double>();
        var terms = document.GetTerms();
        var totalTerms = terms.Count;

        // Goes through every term 
        // checks if the term frequency hasn't been calculated yet
        // if it hasn't been calculated it calculates it 
        // else it looks for the next term, until all have been handled
        foreach (var term in terms)
        {
            if (!tfScores.ContainsKey(term))
            {
                var termCount = terms.Count(t => t == term);
                var tf = (double) termCount / totalTerms;
                tfScores[term] = tf;
            }
        }

        return tfScores;
    }
    private Dictionary<string, double> CalculateTFIDF(Document document)
    {
        var tfScores = CalculateTFScores(document);
        var tfidfScores = new Dictionary<string, double>();

        foreach (var term in IDFValues.Keys)
        {
            var tf = tfScores.ContainsKey(term) ? tfScores[term] : 0;

            var tfidf = tf * IDFValues[term];
            tfidfScores[term] = tfidf;
        }

        return tfidfScores;
    }

}