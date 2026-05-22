
using HRMS_Utility;
using System.Configuration;

namespace HRMS_API.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public FileUploadService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<string?> UploadAndReplaceDocumentAsync(IFormFile document, string folderName, string? existingFileUrl = null)
        {
            try
            {
                Log.LogToFile($"Starting upload in folder: {folderName}");

                if (document == null || document.Length == 0)
                {
                    Log.LogToFile("Upload failed: Document is null or empty.");
                    return null;
                }

                var uploadDir = Path.Combine(_env.WebRootPath, folderName);
                Log.LogToFile($"Upload directory resolved to: {uploadDir}");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                    Log.LogToFile($"Directory created: {uploadDir}");
                }

                // Delete existing file if any
                if (!string.IsNullOrEmpty(existingFileUrl))
                {
                    string fileName = Path.GetFileName(new Uri(existingFileUrl).LocalPath);
                    var existingPath = Path.Combine(uploadDir, fileName);
                    Log.LogToFile($"Attempting to delete existing file: {existingPath}");

                    if (File.Exists(existingPath))
                    {
                        File.Delete(existingPath);
                        Log.LogToFile($"Existing file deleted: {existingPath}");
                    }
                    else
                    {
                        Log.LogToFile($"Existing file not found: {existingPath}");
                    }
                }

                // Create new unique file name
                var extension = Path.GetExtension(document.FileName);
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var originalName = Path.GetFileNameWithoutExtension(document.FileName);
                var newFileName = $"{originalName}_{timestamp}{extension}";
                var fullPath = Path.Combine(uploadDir, newFileName);

                Log.LogToFile($"Saving file to path: {fullPath}");

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }

                // Build dynamic base URL
                var request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}";
                if (request==null||baseUrl == null)
                {
                    baseUrl = _configuration["BaseUrlSettings:baseUrl"];
                }
                var fileUrl = $"{baseUrl}/{folderName.Replace("\\", "/")}/{newFileName}";
                Log.LogToFile($"File uploaded successfully. URL: {fileUrl}");

                return fileUrl;
            }
            catch (Exception ex)
            {
                Log.LogToFile($"Exception in UploadAndReplaceDocumentAsync: {ex}");
                return null;
            }
        }

        public bool DeleteUploadedFile(string folderName, string? fileUrl)
        {
            try
            {
                Log.LogToFile($"Attempting delete in folder: {folderName}");

                var uploadDir = Path.Combine(_env.WebRootPath, folderName);
                Log.LogToFile($"Delete directory resolved to: {uploadDir}");

                if (!Directory.Exists(uploadDir))
                {
                    Log.LogToFile("Delete skipped: Directory does not exist.");
                    return false;
                }

                if (!string.IsNullOrEmpty(fileUrl))
                {
                    string fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
                    var existingPath = Path.Combine(uploadDir, fileName);
                    Log.LogToFile($"Attempting to delete file: {existingPath}");

                    if (File.Exists(existingPath))
                    {
                        File.Delete(existingPath);
                        Log.LogToFile($"File deleted: {existingPath}");
                    }
                    else
                    {
                        Log.LogToFile($"Delete skipped: File not found - {existingPath}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.LogToFile($"Exception in DeleteUploadedFile: {ex}");
                return false;
            }
        }
    }

}
