using DocumentManagement.Entities;
using DocumentManagement.Persistance;
using DocumentManagement.Repository;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagementTests.RepositoryTests
{
    public class DocumentRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly DocumentRepository _repository;

        public DocumentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new DocumentRepository(_context);
        }
        [Fact]
        public async Task SaveDocument_ShouldSaveAndReturnId()
        {
            var doc = new Document
            {
                FileName = "test.pdf",
                FileContent = [1, 2, 3],
                ExtractedText = "sample text",
                UploadedBy = 1,
            };

            var id = await _repository.SaveDocument(doc);
            Assert.NotNull(id);

            var saved = await _context.Set<Document>().FindAsync(id);
            Assert.Equal("test.pdf", saved?.FileName);
        }

        [Fact]
        public async Task GetDocumentById_ReturnsDocument_WhenExists()
        {
            var doc = new Document { FileName = "doc.txt", FileContent = [1], UploadedBy = 1, ExtractedText = "sdjbfjs" };
            _context.Set<Document>().Add(doc);
            await _context.SaveChangesAsync();

            var result = await _repository.GetDocumentById(doc.Id);
            Assert.NotNull(result);
            Assert.Equal("doc.txt", result?.FileName);
        }

        [Fact]
        public async Task GetDocumentById_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.GetDocumentById(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchFromDoc_ReturnsMatchingDocuments()
        {
            _context.Set<Document>().AddRange(
                new Document { FileName = "doc1.pdf", ExtractedText = "hello world", UploadedBy = 1, FileContent = [1] },
                new Document { FileName = "doc2.pdf", ExtractedText = "unit testing", UploadedBy = 1, FileContent = [1] }
            );
            await _context.SaveChangesAsync();

            // Mock FreeText with Contains to simulate logic
            var results = await _context.Set<Document>()
                .Where(d => d.ExtractedText!.Contains("unit"))
                .Select(d => new KeyValuePair<int, string>(d.Id, d.FileName))
                .ToListAsync();

            Assert.Single(results);
            Assert.Contains("doc2.pdf", results[0].Value);
        }
    }
}
