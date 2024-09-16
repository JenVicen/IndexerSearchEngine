using Newtonsoft.Json.Linq;
// Package take from: https://www.nuget.org/packages/Newtonsoft.Json/13.0.3
public class JsonDocument : Document
{
    public JsonDocument(string filePath) : base(filePath) { }

    protected override void GetFileContents()
    {
        string jsonText = File.ReadAllText(FilePath);
        JObject jsonObj = JObject.Parse(jsonText);
        Content = jsonObj.ToString();
    }
}