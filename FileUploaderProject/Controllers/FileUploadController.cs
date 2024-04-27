using FileUploaderProject.DTO;
using FileUploaderProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploaderProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly FileUploadService _fileUploadService;

        public FileUploadController(IWebHostEnvironment env)
        {
            _fileUploadService = new FileUploadService(env); // Pass the IWebHostEnvironment parameter to the constructor
        }
        /*[HttpPost]
        public async Task<FileUploadResponse> UploadFiles(List<IFormFile> files)
        {
            List<Document> documents = new List<Document> { new Document { DocumentType = DocumentType.Identification, Files = files }, new Document { DocumentType = DocumentType.Disability, Files = files } };
            FileUploadRequest request = new FileUploadRequest { Documents = documents };
            FileUploadResponse response = await _fileUploadService.UploadFiles(request);
            return response;
        }*/

        [HttpPost]
        public async Task<FileUploadResponse> UploadFiles([FromForm]FileUploadRequest request)
        { 
            FileUploadResponse response = await _fileUploadService.UploadFiles(request);
            return response;
        }
    }
}
