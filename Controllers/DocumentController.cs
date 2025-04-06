using DocumentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DocumentManagement.Controllers
{

    /// <summary>
    /// Provides routes for managing Documents.
    /// </summary>
    /// <param name="documentService"></param>
    [Route("api/documents")]
    [ApiController]
    public class DocumentController(
        IDocumentService documentService
        ) : ControllerBase
    {

        /// <summary>
        /// upload the document.
        /// </summary>
        /// <param name="file">PDF or Docx.</param>
        /// <returns>Returns the Document ID.</returns>
        [HttpPost("")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var docId = await documentService.SaveDocument(file, int.Parse(userId));

            return Ok(new { Message = "File uploaded successfully!", docId });
        }

        /// <summary>
        /// Gets the uploaded document for current user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var document = await documentService.GetDocumentById(id);
            if (document == null)
                return NotFound("Document not found.");

            return File(document.FileContent, "application/octet-stream", document.FileName);
        }

        /// <summary>
        /// provides the list of file details based on the search query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SearchDocuments([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("invalid Search");

            var docDetails = await documentService.SearchTextFromDoc(query);
            return Ok(docDetails);
        }
    }
}
