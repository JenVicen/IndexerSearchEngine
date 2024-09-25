public class Query : Document 
{
    public Query(string query) : base()
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be null or empty.");
        }
        Content = query; // Assign the query to the content
        Parse();
    }

    protected override void GetFileContents()
    {
        // No need to implement this for a QueryDocument
    }
}