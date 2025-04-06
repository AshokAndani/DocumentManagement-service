using DocumentManagement.Entities;
using DocumentManagement.Repository;
using DocumentManagement.Utility;

namespace DocumentManagement.Services
{
    /// <summary>
    /// Provides methods for Additional processing for Documents
    /// </summary>
    public interface IDocumentService
    {
        Task<int?> SaveDocument(IFormFile file, int uploadedBy);
        Task<Document?> GetDocumentById(int Id);
        Task<List<KeyValuePair<int, string>>?> SearchTextFromDoc(string query);
    }

    /// <summary>
    /// Provides methods for Additional processing for Documents
    /// </summary>
    /// <param name="documentRepository"></param>
    /// <param name="documentProcessor"></param>
    public class DocumentService(
        IDocumentRepository documentRepository,
        DocumentProcessor documentProcessor) : IDocumentService
    {
        public async Task<Document?> GetDocumentById(int Id)
        {
            return await documentRepository.GetDocumentById(Id);
        }

        public async Task<int?> SaveDocument(IFormFile file, int uploadedBy)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            string extractedText = documentProcessor.ExtractText(fileBytes, file.FileName);

            var document = new Document
            {
                FileName = file.FileName,
                FileContent = fileBytes,  
                UploadedBy = uploadedBy,
                ExtractedText = extractedText,
            };
            return await documentRepository.SaveDocument(document);
        }

        public Task<List<KeyValuePair<int, string>>?> SearchTextFromDoc(string query)
        {
            return documentRepository.SearchFromDoc(query);
        }
    }
}
