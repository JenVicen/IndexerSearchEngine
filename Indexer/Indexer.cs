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
    public List<Document> Documents => _documents;
    public TFIDF TFIDFHandler => _tfidfHandler; 
    public void IndexFolder (string folderPath)
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
        }

        return File;
    }

}