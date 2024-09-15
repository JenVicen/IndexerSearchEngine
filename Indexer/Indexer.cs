using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


    public class Indexer
    {
        private readonly TFIDF _tfidf;
        private readonly Dictionary<string, string> _documents;
        private readonly List<string> _supportedExtensions;

        public Indexer()
        {
            _tfidf = new TFIDF();
            _documents = new Dictionary<string, string>();
            _supportedExtensions = new List<string> { ".txt", ".csv", ".xml", ".json", ".html", ".pdf" };
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
                IndexFile(file);
            }

            _tfidf.CalculateIDFAndTFIDF(_documents.Values);
        }

        private void IndexFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            string content = File.ReadAllText(filePath);
            _documents[filePath] = content;
        }

        public void SaveIndex(string indexPath)
        {
            var indexData = new
            {
                Documents = _documents.Select(kvp => new
                {
                    Path = kvp.Key,
                    Content = kvp.Value,
                    TermFrequencies = new Dictionary<string, int>() // Assuming term frequencies are not needed for simple string storage
                }).ToList(),
                IDF = _tfidf.IDF
            };

            var json = JsonSerializer.Serialize(indexData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(indexPath, json);
        }

        public void LoadIndex(string indexPath)
        {
            if (!File.Exists(indexPath))
            {
                throw new FileNotFoundException($"Index file not found: {indexPath}");
            }

            var json = File.ReadAllText(indexPath);
            var indexData = JsonSerializer.Deserialize<IndexData>(json);

            _documents.Clear();
            foreach (var doc in indexData.Documents)
            {
                _documents[doc.Path] = doc.Content;
            }

            _tfidf.IDF = indexData.IDF;
        }

        public List<SearchResult> Search(string query, int k)
        {
            var queryTerms = TFIDF.TokenizeAndNormalize(query);
            var queryVector = _tfidf.CalculateQueryVector(queryTerms);

            var results = _documents.Select(kvp =>
            {
                var similarity = CosineSimilarity.Calculate(queryVector, kvp.Value);
                return new SearchResult { FilePath = kvp.Key, Similarity = similarity };
            })
            .OrderByDescending(r => r.Similarity)
            .Take(k)
            .ToList();

            return results;
        }
    }

    public class SearchResult
    {
        public string FilePath { get; set; }
        public double Similarity { get; set; }
    }

    internal class IndexData
    {
        public List<DocumentData> Documents { get; set; }
        public Dictionary<string, double> IDF { get; set; }
    }

    internal class DocumentData
    {
        public string Path { get; set; }
        public string Content { get; set; }
        public Dictionary<string, int> TermFrequencies { get; set; }
    }
