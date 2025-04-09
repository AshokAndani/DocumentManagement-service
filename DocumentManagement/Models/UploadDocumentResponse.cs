namespace DocumentManagement.Models
{
    /// <summary>
    /// Represents the response model for document upload.
    /// </summary>
    public class UploadDocumentResponse
    {
        /// <summary>
        /// Gets or sets the Document ID.
        /// </summary>
        public int? DocId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string? Message { get; set; }
    }
}
