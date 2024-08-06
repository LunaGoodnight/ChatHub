using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;

namespace ChatApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public UploadController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string? filename)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Please upload a file.");

        string sanitizedFilename = SanitizeFilename(filename ?? Path.GetFileNameWithoutExtension(file.FileName));
        string fileExtension = Path.GetExtension(file.FileName);
        string uniqueFilename = $"{sanitizedFilename}_{Guid.NewGuid().ToString().Substring(0, 8)}{fileExtension}";

        var uploads = Path.Combine(_env.WebRootPath, "images");
        if (!Directory.Exists(uploads))
            Directory.CreateDirectory(uploads);

        var filePath = Path.Combine(uploads, uniqueFilename);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        var url = $"{Request.Scheme}://{Request.Host}/images/{Path.GetFileName(filePath)}";
        return Ok(new { Url = url });
    }

    private string SanitizeFilename(string filename)
    {
        return Regex.Replace(filename, @"[^a-zA-Z0-9_\-]", "_");
    }
}