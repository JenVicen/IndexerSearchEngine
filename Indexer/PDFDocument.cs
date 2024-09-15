using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

public class PDFDocument : Document
{
    public PDFDocument(string filePath) : base(filePath) { }

    public override void Parse()
    {
        using var pdfReader = new PdfReader(FilePath);
        using var pdfDoc = new PdfDocument(pdfReader);
        var strategy = new SimpleTextExtractionStrategy();
        Content = string.Empty;
        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        {
            Content += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
        }
    }
}
