// Package for CSV File Manipulation and Handling
// Retrieved from: https://www.nuget.org/packages/CsvHelper  
using CsvHelper;
using System.Globalization;
public class CsvDocument : Document
{
    public CsvDocument(string filePath) : base(filePath) { }

    // Function in charge of getting the contents of the .txt file
    protected override void GetFileContents()
    {
        // Step 1: Read the content from the file 
        using var reader = new StreamReader(FilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        Content = File.ReadAllText(FilePath);
    }
}