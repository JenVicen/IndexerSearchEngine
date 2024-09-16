using HtmlAgilityPack; 
// Package taken from: https://www.nuget.org/packages/HtmlAgilityPack#supportedframeworks-body-tab 

public class HtmlDocument : Document
{
    public HtmlDocument(string filePath) : base(filePath) { }

    protected override void GetFileContents()
    {
        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        htmlDoc.Load(FilePath);
        Content = htmlDoc.DocumentNode.InnerText;
    }
}