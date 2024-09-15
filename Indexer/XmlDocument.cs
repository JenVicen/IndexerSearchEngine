using System.Xml;

public class XmlDocument : Document
{
    public XmlDocument(string filePath) : base(filePath) { }

    public override void Parse()
    {
        var xmlDoc = new System.Xml.XmlDocument();
        xmlDoc.Load(FilePath);
        Content = xmlDoc.InnerText;
    }
}
