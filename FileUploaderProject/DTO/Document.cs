namespace FileUploaderProject.DTO
{
    public class Document
    {
        public DocumentType DocumentType { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
