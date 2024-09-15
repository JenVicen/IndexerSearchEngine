using System.Globalization;
using CsvHelper;

public class CsvDocument : Document
{
    public CsvDocument(string filePath) : base(filePath) { }

    public override void Parse()
    {
        using var reader = new StreamReader(FilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        Content = string.Join(" ", csv.GetRecords<dynamic>());
    }
}
