using System.Xml;

public class XmlDocument : Document
{
    public XmlDocument(string filePath) : base(filePath) { }

    protected override void GetFileContents()
    {
        var xmlDoc = new System.Xml.XmlDocument();
        xmlDoc.Load(FilePath);
        Content = xmlDoc.InnerText;
    }
}