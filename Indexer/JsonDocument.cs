using Newtonsoft.Json.Linq;

public class JsonDocument : Document
{
    public JsonDocument(string filePath) : base(filePath) { }

    public override void Parse()
    {
        string jsonText = File.ReadAllText(FilePath);
        JObject jsonObj = JObject.Parse(jsonText);
        Content = jsonObj.ToString();
    }
}
