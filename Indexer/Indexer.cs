using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;

public class Indexer 
{
    private readonly TFIDF _tfidfHandler;
    private List<Document> _documents;
    private readonly List<string> _supportedExtensions;
    public Dictionary<string, double> _tfidfValues;

    public Indexer()
    {
        _tfidfHandler = new TFIDF();
        _documents = new List<Document>();
        _supportedExtensions = new List<string> { ".txt", ".csv", ".xml", ".json", ".html", ".pdf" };
        _tfidfValues = new Dictionary<string,double> ();
    }
    public List<Document> Documents => _documents;
    public TFIDF TFIDFHandler => _tfidfHandler; 
    public void IndexFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {folderPath}");
        }

        var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .Where(file => _supportedExtensions.Contains(Path.GetExtension(file).ToLower()) && 
                           Path.GetFileName(file) != "index.json"); // Exclude index.json

        foreach (var file in files)
        {
            try
            {
                var document = IndexFile(file);
                if (document != null)
                {
                    _documents.Add(document);
                    Console.WriteLine($"Indexed: {file}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error indexing file {file}: {ex.Message}");
            }
        }

        if (_documents.Count == 0)
        {
            Console.WriteLine("No documents indexed.");
            return;
        }

        _tfidfValues = _tfidfHandler.CalculateIDFandTFIDF(_documents);

        // Save the indexed data to a JSON file
        string indexFilePath = Path.Combine(folderPath, "index.json");
        SaveIndex(indexFilePath);
        Console.WriteLine($"Index saved to: {indexFilePath}");
    }
    private Document IndexFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        Document File = null;
        switch (extension)
        {
            case ".txt":
                File = new TxtDocument(filePath);
                break;
            case ".csv":
                File = new CsvDocument(filePath);
                break;
            case ".xml":
                File = new XmlDocument(filePath);
                break;
            case ".json":
                File = new JsonDocument(filePath);
                break;
            case ".html":
                File = new HtmlDocument(filePath);
                break;
            case ".pdf":
                File = new PDFDocument(filePath);
                break;
            default:
                break;
        }

        return File;
    }

    // Recieves a query and a number of results to display 
    public List<string> Search(string query, int k)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be empty.");
        }
        Console.WriteLine($"This is your query: {query}");

        var queryDocument = new Query(query);
        Console.WriteLine($"This is your queryyyyy: {queryDocument}");
        var queryTfidfScores = _tfidfHandler.CalculateTFIDF(queryDocument);
        
        // Debugging: Check if queryTfidfScores is populated
        Console.WriteLine($"Query TF-IDF Scores count: {queryTfidfScores.Count}");

        // Calculate cosine similarity for each document using loaded TF-IDF values
        var cosineSimilarity = new CosineSimilarity();
        var similarities = new List<(Document document, double similarity)>();

        foreach (var document in _documents)
        {
            if (!_tfidfValues.TryGetValue(document.FileName, out var tfidfScore))
            {
                Console.WriteLine($"No TF-IDF scores found for document: {document.FileName}");
                continue; // Skip this document if no scores are found
            }

            double similarity = cosineSimilarity.CalculateCosineSimilarity(queryTfidfScores, new Dictionary<string, double> { { document.FileName, tfidfScore } });
            similarities.Add((document, similarity));
        }

        // Sort documents by similarity and take the top k results
        return similarities.OrderByDescending(x => x.similarity)
                           .Take(k)
                           .Select(x => x.document.FileName)
                           .ToList();
    }

    public void LoadIndex(string indexPath)
    {
        if (!File.Exists(indexPath))
        {
            throw new FileNotFoundException($"Index file not found: {indexPath}");
        }

        // Load the index data from the file
        var indexData = File.ReadAllText(indexPath);
        // Deserialize the index data to restore documents and TF-IDF values
        (_documents, _tfidfValues) = DeserializeIndexData(indexData);

        // Check if documents are loaded
        if (_documents == null || !_documents.Any())
        {
            throw new InvalidOperationException("No documents loaded from the index.");
        }
    }

    public (List<Document>, Dictionary<string, double>) DeserializeIndexData(string indexData)
    {
        // Assuming the indexData is in JSON format
        var indexObject = JsonSerializer.Deserialize<IndexData>(indexData);
        
        // Convert the deserialized data back into documents
        var documents = new List<Document>();
        foreach (var docData in indexObject.Documents)
        {
            Document document = docData.Extension switch
            {
                ".txt" => new TxtDocument(docData.FilePath),
                ".csv" => new CsvDocument(docData.FilePath),
                ".xml" => new XmlDocument(docData.FilePath),
                ".json" => new JsonDocument(docData.FilePath),
                ".html" => new HtmlDocument(docData.FilePath),
                ".pdf" => new PDFDocument(docData.FilePath),
                _ => throw new NotSupportedException($"Unsupported document type: {docData.Extension}")
            };
            documents.Add(document);
        }

        return (documents, indexObject.TFIDFValues);
    }

    // Class to represent the structure of the deserialized index data
    private class IndexData
    {
        public List<DocumentData> Documents { get; set; }
        public Dictionary<string, double> TFIDFValues { get; set; }
    }

    private class DocumentData
    {
        public string FilePath { get; set; }
        public string Extension { get; set; }
    }

    public void SaveIndex(string indexPath)
    {
        var indexData = new IndexData
        {
            Documents = _documents.Select(doc => new DocumentData
            {
                FilePath = doc.FilePath,
                Extension = Path.GetExtension(doc.FilePath).ToLower()
            }).ToList(),
            TFIDFValues = _tfidfValues
        };

        var json = JsonSerializer.Serialize(indexData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(indexPath, json);
    }
}