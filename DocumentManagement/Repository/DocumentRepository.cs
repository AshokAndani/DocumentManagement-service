using DocumentManagement.Entities;
using DocumentManagement.Persistance;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Repository
{
    /// <summary>
    /// provides methods to work on Documents in DB
    /// </summary>
    public interface IDocumentRepository
    {
        Task<int?> SaveDocument(Document document);
        Task<Document?> GetDocumentById(int Id);
        Task<List<KeyValuePair<int, string>>?> SearchFromDoc(string query);
    }

    /// <summary>
    /// provides methods to work on Documents in DB
    /// </summary>
    /// <param name="appDbContext"></param>
    public class DocumentRepository(AppDbContext appDbContext) : IDocumentRepository
    {
        public async Task<Document?> GetDocumentById(int Id)
        {
            return await appDbContext.Set<Document>().FirstOrDefaultAsync(x=> x.Id == Id);
        }

        public async Task<int?> SaveDocument(Document document)
        {
            var doc = appDbContext.Set<Document>().Add(document);
            await appDbContext.SaveChangesAsync();
            return doc.Entity.Id;
        }

        public async Task<List<KeyValuePair<int, string>>?> SearchFromDoc(string query)
        {
            var docDetails = await appDbContext.Set<Document>()
                .Where(d => EF.Functions.FreeText(d.ExtractedText!, query))
                .Select(d => new { d.Id, d.FileName })
                .ToListAsync();
            return docDetails.
                Select(x=> new KeyValuePair<int, string>(x.Id, x.FileName))?
                .ToList();
        }
    }
}
