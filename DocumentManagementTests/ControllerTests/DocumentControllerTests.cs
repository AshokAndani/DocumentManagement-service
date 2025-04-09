using DocumentManagement.Controllers;
using DocumentManagement.Entities;
using DocumentManagement.Models;
using DocumentManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Text;

namespace DocumentManagementTests.ControllerTests
{
    public class DocumentControllerTests
    {
        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly DocumentController _controller;

        public DocumentControllerTests()
        {
            _documentServiceMock = new Mock<IDocumentService>();
            _controller = new DocumentController(_documentServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task UploadDocument_ReturnsBadRequest_WhenFileIsNull()
        {
            var result = await _controller.UploadDocument(null!);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid file.", badRequest.Value);
        }

        [Fact]
        public async Task UploadDocument_ReturnsBadRequest_WhenFileIsEmpty()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            var result = await _controller.UploadDocument(fileMock.Object);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid file.", badRequest.Value);
        }

        [Fact]
        public async Task UploadDocument_ReturnsOk_WithDocumentId()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);

            _documentServiceMock.Setup(s => s.SaveDocument(fileMock.Object, 1))
                .ReturnsAsync(123);

            var result = await _controller.UploadDocument(fileMock.Object);
            var ok = Assert.IsType<OkObjectResult>(result);
            var val = ok.Value as UploadDocumentResponse;
            Assert.Equal(123, val!.DocId);
        }

        [Fact]
        public async Task DownloadDocument_ReturnsNotFound_WhenDocNotExists()
        {
            _documentServiceMock.Setup(s => s.GetDocumentById(1)).ReturnsAsync((Document?)null);

            var result = await _controller.DownloadDocument(1);
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Document not found.", notFound.Value);
        }

        [Fact]
        public async Task DownloadDocument_ReturnsFile_WhenDocumentExists()
        {
            var doc = new Document
            {
                FileContent = Encoding.UTF8.GetBytes("sample content"),
                FileName = "file.txt"
            };

            _documentServiceMock.Setup(s => s.GetDocumentById(1)).ReturnsAsync(doc);

            var result = await _controller.DownloadDocument(1);
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.Equal("file.txt", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task SearchDocuments_ReturnsBadRequest_WhenQueryIsInvalid()
        {
            var result = await _controller.SearchDocuments(" ");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("invalid Search", badRequest.Value);
        }

        [Fact]
        public async Task SearchDocuments_ReturnsOk_WithResults()
        {
            var results = new List<KeyValuePair<int, string>>
        {
                new KeyValuePair<int, string>(1, "file1.doc"),
        };

            _documentServiceMock.Setup(s => s.SearchTextFromDoc("test")).ReturnsAsync(results);

            var result = await _controller.SearchDocuments("test");
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(results, ok.Value);
        }
    }

}
