using HtmlAgilityPack;

public class HtmlDocument : Document
{
    public HtmlDocument(string filePath) : base(filePath) { }

    public override void Parse()
    {
        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        htmlDoc.Load(FilePath);
        Content = htmlDoc.DocumentNode.InnerText;
    }
}
