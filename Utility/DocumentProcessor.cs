using System.Text;
using UglyToad.PdfPig;
using Xceed.Words.NET;

namespace DocumentManagement.Utility
{
    /// <summary>
    /// Provides Helper methods for document text extraction.
    /// </summary>
    public class DocumentProcessor
    {
        public string ExtractText(byte[] fileContent, string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension == ".pdf")
                return ExtractTextFromPdf(fileContent);
            if (extension == ".docx")
                return ExtractTextFromDocx(fileContent);

            return string.Empty; // Unsupported format
        }

        private string ExtractTextFromPdf(byte[] fileContent)
        {
            using var stream = new MemoryStream(fileContent);
            using var pdf = PdfDocument.Open(stream);
            var sb = new StringBuilder();

            foreach (var page in pdf.GetPages())
                sb.AppendLine(page.Text);

            return sb.ToString();
        }

        private string ExtractTextFromDocx(byte[] fileContent)
        {
            using var stream = new MemoryStream(fileContent);
            using var doc = DocX.Load(stream);

            return doc.Text;
        }
    }
}
