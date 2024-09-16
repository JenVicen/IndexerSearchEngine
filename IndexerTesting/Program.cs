public class Program
{
    public static void Main(){
        TxtDocument txtDoc = new TxtDocument("C:\\Users\\Usuario\\Desktop\\Git Repositories\\IndexerSearchEngine\\IndexerTesting\\Example.txt");
        Console.WriteLine("Processed Terms:");
        foreach (var term in txtDoc.NormalizedTerms)
        {
            Console.WriteLine(term);
        }
        Console.WriteLine();
        CsvDocument csvDoc = new CsvDocument("C:\\Users\\Usuario\\Desktop\\Git Repositories\\IndexerSearchEngine\\IndexerTesting\\customers-100.csv");
        foreach (var term in csvDoc.NormalizedTerms)
        {
            Console.WriteLine(term);
        }
    }
}