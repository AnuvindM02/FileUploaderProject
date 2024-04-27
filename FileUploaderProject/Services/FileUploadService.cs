using FileUploaderProject.DTO;
using System.Security.AccessControl;

namespace FileUploaderProject.Services
{
    public class FileUploadService
    {
        private readonly string? _tempDirectory;
        private readonly string? _tempSubDirectory;


        public FileUploadService(IWebHostEnvironment env)
        {
            _tempDirectory = Path.Combine(env.WebRootPath, "temp");

            if (!Directory.Exists(_tempDirectory))
            {
                Directory.CreateDirectory(_tempDirectory); // Create the temp directory if it doesn't exist

                // Get the directory's access control
                DirectoryInfo directoryInfo = new DirectoryInfo(_tempDirectory);
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

                // Specify the access rule
                FileSystemAccessRule rule = new FileSystemAccessRule("Users", FileSystemRights.Read | FileSystemRights.Write | FileSystemRights.ExecuteFile, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);

                // Add the access rule to the directory's access control
                directorySecurity.AddAccessRule(rule);

                // Set the modified access control to the directory
                directoryInfo.SetAccessControl(directorySecurity);

                Console.WriteLine("Directory created with permissions.");
            }

            foreach (DocumentType documentType in Enum.GetValues(typeof(DocumentType)))
            {
                _tempSubDirectory = Path.Combine(_tempDirectory, documentType.ToString());
                if (!Directory.Exists(_tempSubDirectory))
                {
                    Directory.CreateDirectory(_tempSubDirectory); 
                }
            }

        }

        public async Task<FileUploadResponse> UploadFiles(FileUploadRequest request)
        {
            foreach(Document document in request.Documents)
            {
                string subDirectoryPath = Path.Combine(_tempDirectory, document.DocumentType.ToString());
                foreach(IFormFile file in document.Files)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(_tempDirectory, file.Name); // Combine the uploads directory path with the file name
                        using var stream = new FileStream(subDirectoryPath, FileMode.Create);
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return new FileUploadResponse { Message = "Files uploaded successfuly" };
        }
    }
}
