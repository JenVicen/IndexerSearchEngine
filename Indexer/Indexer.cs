using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Indexer 
{
    private Vectorizer _vectorizerHandler;
    private List<Document> _documents;
    private readonly List<string> _supportedExtensions;
    public Dictionary<string, double> _tfidfValues;

    public Indexer()
    {
        _documents = new List<Document>();
        _supportedExtensions = new List<string> { ".txt", ".csv", ".xml", ".json", ".html", ".pdf" };
        _tfidfValues = new Dictionary<string,double>();
    }

    public List<Document> Documents => _documents;

    public void IndexFolder(string folderPath, string type, string distance)
    {
        // Limpiar documentos antes de indexar
        _documents.Clear(); // Asegúrate de que la lista esté vacía antes de indexar

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

        // Initialize the vectorizer based on the type
        if (type == "tfidf")
        {
            _vectorizerHandler = new TFIDF();
        }
        else if (type == "vectorizer")
        {
            _vectorizerHandler = new NormalizedBagOfWords(); // O cualquier otra implementación de Vectorizer
        }
        else
        {
            throw new ArgumentException("Unsupported type. Use 'tfidf' or 'vectorizer'.");
        }

        // Vectorize documents
        _tfidfValues = _vectorizerHandler.VectorizeDocuments(_documents);

        // Save the indexed data to a JSON file
        string indexFilePath = Path.Combine(folderPath, "index.json");
        SaveIndex(indexFilePath); // Asegúrate de que esto se llame aquí
        Console.WriteLine($"Index saved to: {indexFilePath}");
    }

    private Document IndexFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        Document file = null;
        switch (extension)
        {
            case ".txt":
                file = new TxtDocument(filePath);
                break;
            case ".csv":
                file = new CsvDocument(filePath);
                break;
            case ".xml":
                file = new XmlDocument(filePath);
                break;
            case ".json":
                file = new JsonDocument(filePath);
                break;
            case ".html":
                file = new HtmlDocument(filePath);
                break;
            case ".pdf":
                file = new PDFDocument(filePath);
                break;
            default:
                break;
        }

        return file;
    }

    public List<string> Search(string query, int k, string distance)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be empty.");
        }
        Console.WriteLine($"This is your query: {query}");

        var normalizedQuery = NormalizeQuery(query);
        var queryDocument = new Query(normalizedQuery);
        var queryVector = _vectorizerHandler.VectorizeDocument(queryDocument);

        Similarity similarityCalculator = distance == "cosine" ? new CosineSimilarity() : new EuclideanSimilarity();
        var results = new List<(string FileName, double Similarity)>();

        foreach (var document in _documents)
        {
            var documentVector = _vectorizerHandler.VectorizeDocument(document);
            double similarity = similarityCalculator.CalculateSimilarity(queryVector, documentVector);

            if (similarity > 0)
            {
                results.Add((document.FileName, similarity));
            }
        }

        return results.OrderByDescending(r => r.Similarity).Take(k).Select(r => r.FileName).ToList();
    }

    private string NormalizeQuery(string query)
    {
        return query.ToLower();
    }

    public void LoadIndex(string indexPath)
    {
        if (!File.Exists(indexPath))
        {
            throw new FileNotFoundException($"Index file not found: {indexPath}");
        }

        var indexData = File.ReadAllText(indexPath);
        (_documents, _tfidfValues) = DeserializeIndexData(indexData);

        if (_documents == null || !_documents.Any())
        {
            throw new InvalidOperationException("No documents loaded from the index.");
        }
    }

    public (List<Document>, Dictionary<string, double>) DeserializeIndexData(string indexData)
    {
        var indexObject = JsonSerializer.Deserialize<IndexData>(indexData);
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