public class Query : Document 
{
    public Query(string query) : base("query")
    {
        Content = query; // Assign the query to the content
    }

    protected override void GetFileContents()
    {
        // No is necessary to implement this for a QueryDocument
        // since the content is the query in itself
    }
}