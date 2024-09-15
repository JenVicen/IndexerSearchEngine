public class TxtDocument : Document
{
    public TxtDocument(string filePath) : base(filePath) { }

    public override void Parse()
    {
        Content = File.ReadAllText(FilePath);
    }
}
