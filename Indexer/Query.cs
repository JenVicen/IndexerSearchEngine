public class Query : Document 
{
    public Query(string query) : base("query")
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be null or empty.JAJAJJAJJAJJ");
        }
        
        Content = query; // Assign the query to the content
        Parse(); // Call Parse to process the query content
    }

    protected override void GetFileContents()
    {
        // No need to implement this for a QueryDocument
    }

    // Agregar un método para obtener los términos de la consulta
    public List<string> GetQueryTerms()
    {
        return NormalizedTerms; // Retorna los términos normalizados de la consulta
    }
}