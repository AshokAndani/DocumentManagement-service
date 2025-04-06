

namespace DocumentManagement.Entities
{
    /// <summary>
    /// Db Entity containing the all the information regarding a document.
    /// </summary>
    public class Document
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] FileContent { get; set; }

        public string? ExtractedText { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? ModifyTimestamp { get; set; }
        public DateTime? CreateTimestamp { get; set; }

        public int? UploadedBy { get; set; }
    }

}
