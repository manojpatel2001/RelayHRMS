using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Helper
{
    public static class UploadDocument
    {
        public static async Task<string?> UploadAndReplaceDocumentAsync(HttpRequest request, IFormFile document, string folderName, string? existingFileName = null)
        {
            try
            {
                if (document == null || document.Length == 0)
                    return null;

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Delete existing file if given
                if (!string.IsNullOrEmpty(existingFileName))
                {
                    string fileName = Path.GetFileName(new Uri(existingFileName).LocalPath);
                    var existingPath = Path.Combine(uploadDir, fileName);
                    if (File.Exists(existingPath))
                    {
                        File.Delete(existingPath);
                    }
                }

                var extension = Path.GetExtension(document.FileName);
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var originalName = Path.GetFileNameWithoutExtension(document.FileName);
                var newFileName = $"{originalName}_{timestamp}{extension}";
                var fullPath = Path.Combine(uploadDir, newFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }

                // Build full URL
                var baseUrl = UriHelper.GetDisplayUrl(request);
                var uri = new Uri(baseUrl);
                var apiBase = $"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? "" : ":" + uri.Port)}";
                var fileUrl = $"{apiBase}/{folderName.Replace("\\", "/")}/{newFileName}";

                return fileUrl;
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return null;
            }
        }
        public static bool DeleteUploadedFile(HttpRequest request,  string folderName, string? existingFileName )
        {
            try
            {
                
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

                if (!Directory.Exists(uploadDir))
                { 
                    return false; 
                }
                // Delete existing file if given
                if (!string.IsNullOrEmpty(existingFileName))
                {
                    string fileName = Path.GetFileName(new Uri(existingFileName).LocalPath);
                    var existingPath = Path.Combine(uploadDir, fileName);
                    if (File.Exists(existingPath))
                    {
                        File.Delete(existingPath);
                    }
                }

                return true ;
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return false;
            }
        }


    }
}
