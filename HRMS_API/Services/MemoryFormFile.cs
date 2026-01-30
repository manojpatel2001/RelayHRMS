using Microsoft.AspNetCore.Http;
using System.IO;

public class MemoryFormFile : IFormFile
{
    private readonly MemoryStream _memoryStream;

    public MemoryFormFile(MemoryStream memoryStream, string fileName)
    {
        _memoryStream = memoryStream;
        Name = "file";
        FileName = fileName;
        Length = memoryStream.Length;
    }

    public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{FileName}\"";
    public IHeaderDictionary Headers => new HeaderDictionary();
    public long Length { get; }
    public string Name { get; }
    public string FileName { get; }

    public Stream OpenReadStream()
    {
        _memoryStream.Position = 0;
        return _memoryStream;
    }

    public void CopyTo(Stream target)
    {
        _memoryStream.Position = 0;
        _memoryStream.CopyTo(target);
    }

    public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        _memoryStream.Position = 0;
        await _memoryStream.CopyToAsync(target, cancellationToken);
    }
}
