public class TxtDocument : Document
{
    public TxtDocument(string filePath) : base(filePath) { }

    // Function in charge of getting the contents of the .txt file
    protected override void GetFileContents()
    {
        // Step 1: Read the content from the file 
        Content = File.ReadAllText(FilePath);
    }
}