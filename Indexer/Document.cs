// Stemming package in charge of helping retrieve the root of words in documents, uses Porter Stemming Algorithm 
// Retrieved from: https://www.nuget.org/packages/Porter2Stemmer 
using Porter2Stemmer;
public abstract class Document 
{
    public string FilePath { get; }
    public string FileName => Path.GetFileName(FilePath);
    public string Content {get; protected set;}
    public List<string> NormalizedTerms {get; protected set; }

    // This is a HashSet of all strings that aren't analyzed in the document, called Stop Words in NLP
    // allows for documents to be simplified without the use of "filler" words for a more detailed explanation go to:
    // https://www.opinosis-analytics.com/knowledge-base/stop-words-explained/#:~:text=Stop%20words%20are%20a%20set,carry%20very%20little%20useful%20information.
    protected static readonly HashSet<string> StopWords = new HashSet<string>{
        "a", "an", "the", "and", "or", "but", "nor", "for", "yet", "so",
        "in", "on", "at", "by", "with", "under", "over", "between", "among",
        "to", "from", "of", "about", "against", "during", "before", "after",
        "above", "below", "through", "during", "has", "have", "had", "is", "am",
        "are", "was", "were", "will", "would", "can", "could", "should", "shall",
        "might", "may", "must", "very", "too", "just", "also", "even", "never",
        "always", "often", "sometimes", "only", "quite", "this", "that", "these",
        "those", "some", "any", "every", "each", "no", "another", "be", "been",
        "being", "do", "does", "did", "does", "not", "all", "few", "more", "most",
        "such", "than", "then", "because", "due", "to", "for", "in", "fact", "so",
        "as", "well", "if", "when", "where", "how", "why", "that", "x", "y", "h", 
        "k", "z", "v", "https", "www", "net", "http", "com", "org"
    };

    // This is an array of unwanted chars in file Parsing for all documents
    protected static readonly char[] _splitChars = new[] {
    ' ', '.', ',', ';', '\n', '\r', ':', '!', '?', '-', '_', '(', ')', 
    '[', ']', '"', '\\', '/', '<', '>', '=', '{', '}', '\'', '#', 
    '$', '%', '^', '&', '*', '+', '=', '`', '|', '~', '–', '—',
    '•', '«', '»', '¨', '©', '®', '™', '©', '°', '¤', '£', '¥', '₣', '₤'};

    // Library that uses the Porter2Stemmer algorithm for Natural Language Processing
    private readonly EnglishPorter2Stemmer _stemmer = new EnglishPorter2Stemmer();

    public Document(string filePath)
    {
        FilePath = filePath;
        NormalizedTerms = new List<string>();
        GetFileContents();
        Parse();
    }

    // Each type of document is in charge of getting its own contents
    protected abstract void GetFileContents();

    // Parse function is the same for all documents since contents contain the strings of the content.
    private void Parse(){
        // Step 2: Split the content into an array of terms, all lowercased
        string[] words = Content.ToLower().Split(_splitChars, StringSplitOptions.RemoveEmptyEntries);
        
        // Step 3: Remove the StopWords and apply the stemming 
        foreach (var term in words) 
        {
            if (!StopWords.Contains(term))
            {
                // Based on the Porter2Stemmer Package we retrieve the result
                var stemmedWord = _stemmer.Stem(term);
                NormalizedTerms.Add(stemmedWord.Value);
            }
        }
    }

    // Function in charge of returning the terms of a function, all stemmed words in the document 
    public List<string> GetTerms(){
        return NormalizedTerms;
    }

    public List<string> GetUniqueTerms(){
        // Using a HashSet to filter out any duplicate terms in a document, making it a list and returning it
        var uniqueTerms = new HashSet<string>(NormalizedTerms);
        return uniqueTerms.ToList();
    }

}