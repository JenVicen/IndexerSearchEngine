/*
public class Testing
{
    public static void Main(){
        // Ensure the file path is correct for the text file
        string txtFilePath = "/Users/jennifervicentesvalle/Desktop/TTU/Object-Oriented Programming/IndexerSearchEngine/IndexerTesting/Example.txt";
        
        // Check if the text file exists before trying to read it
        if (!File.Exists(txtFilePath)) {
            Console.WriteLine("File not found: " + txtFilePath);
            return; // Exit if the file does not exist
        }

        TxtDocument txtDoc = new TxtDocument(txtFilePath);
        Console.WriteLine("Processed Terms:");
        foreach (var term in txtDoc.NormalizedTerms)
        {
            Console.WriteLine(term);
        }
        Console.WriteLine();
        
        // Ensure the file path is correct for the CSV file
        string csvFilePath = "/Users/jennifervicentesvalle/Desktop/TTU/Object-Oriented Programming/IndexerSearchEngine/IndexerTesting/customers-100.csv";
        // Check if the CSV file exists before trying to read it
        if (!File.Exists(csvFilePath)) {
            Console.WriteLine("File not found: " + csvFilePath);
            return; // Exit if the file does not exist
        }

        CsvDocument csvDoc = new CsvDocument(csvFilePath);
        foreach (var term in csvDoc.NormalizedTerms)
        {
            Console.WriteLine(term);
        }
    }
}
*/