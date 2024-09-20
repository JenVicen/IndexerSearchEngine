// QueryDocument is a special type of Document that is used to represent a user's search query.
public class QueryDocument : Document
{
    public QueryDocument(string query) : base("query")
    {
        Content = query; // Assign the query to the content
    }

    protected override void GetFileContents()
    {
        // No is necessary to implement this for a QueryDocument
    }
}