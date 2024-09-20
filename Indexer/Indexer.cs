using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;

public class Indexer 
{
    private readonly TFIDF _tfidfHandler;
    private readonly List<Document> _documents;
    private readonly List<string> _supportedExtensions;
    private Dictionary<string, double> _tfidfValues;

    public Indexer()
    {
        _tfidfHandler = new TFIDF();
        _documents = new List<Document>();
        _supportedExtensions = new List<string> { ".txt", ".csv", ".xml", ".json", ".html", ".pdf" };
        _tfidfValues = new Dictionary<string,double> ();
    }

    public void IndexFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {folderPath}");
        }

        var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .Where(file => _supportedExtensions.Contains(Path.GetExtension(file).ToLower()));
        
        foreach (var file in files)
        {
            _documents.Add(IndexFile(file));
        }

        _tfidfValues = _tfidfHandler.CalculateIDFandTFIDF(_documents);
    }

    public void LoadIndex(string indexPath)
    {
        if (!File.Exists(indexPath))
        {
            throw new FileNotFoundException($"Index file not found: {indexPath}");
        }

        // Cargar el índice desde el archivo
        var indexData = File.ReadAllText(indexPath);
        _tfidfValues = JsonSerializer.Deserialize<Dictionary<string, double>>(indexData);
        // Aquí podrías cargar también los documentos si es necesario
    }

    public List<string> Search(string query, int k)
    {
        var queryDocument = new QueryDocument(query); // Crear un documento de consulta
        var queryTfidf = _tfidfHandler.CalculateTFIDF(queryDocument);
        var cosineSimilarity = new CosineSimilarity(_tfidfHandler);

        // Calcular similitudes
        var similarities = new Dictionary<Document, double>();
        foreach (var document in _documents)
        {
            double similarity = cosineSimilarity.Calculate(queryDocument, document);
            similarities[document] = similarity;
        }

        // Obtener los k documentos más similares
        return similarities.OrderByDescending(x => x.Value)
                           .Take(k)
                           .Select(x => x.Key.FilePath) // Asumiendo que FilePath es la propiedad que quieres mostrar
                           .ToList();
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
        }

        return file;
    }
}