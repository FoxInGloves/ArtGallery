using Microsoft.IdentityModel.Tokens;

namespace ArtGallery.Models.Services;

public class ImageManipulation
{
    public async Task<string?> GetIconPathAsync(IFormFile? uploadPicture, string? originalIconPath)
    {
        var tempIconPath = originalIconPath;
        if (uploadPicture?.Name == null) return originalIconPath;
        
        originalIconPath = await UploadImageAsync(uploadPicture, MethodSelector.Art);
        if (!string.Equals(tempIconPath, originalIconPath) && originalIconPath != null)
        {
            RemoveImage(tempIconPath);
        }

        return originalIconPath;
    }
    
    public async Task<string?> UploadImageAsync(IFormFile? image, MethodSelector selector)
    {
        string? imagePath;
        if (image != null)
        {
            var fileName = Path.GetFileName(image.FileName);
            var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{_methods[selector]}/{fileName}";
            if (System.IO.File.Exists(uploadPath))
            {
                uploadPath = GenerateUniqueFileName(fileName, selector);
            }
            await using var fileStream = new FileStream(uploadPath, FileMode.Create);
            await image.CopyToAsync(fileStream);
            imagePath = $"/images/{_methods[selector]}/{fileName}";
        }
        else
        {
            imagePath = null;
        }
        return imagePath;
    }
    
    public string GenerateUniqueFileName(string fileName, MethodSelector selector)
    {
        var extension = Path.GetExtension(fileName);
        var fileNameOnly = Path.GetFileNameWithoutExtension(fileName);

        var counter = 1;
        var uniqueFileName = fileName;
        
        while (File.Exists($"{Directory.GetCurrentDirectory()}/wwwroot/images/{_methods[selector]}/{uniqueFileName}"))
        {
            var tempFileName = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{_methods[selector]}/{fileNameOnly}({counter}){extension}";
            uniqueFileName = tempFileName;
            counter++;
        }
        return uniqueFileName;
    }

    public void RemoveImage(string? iconPath)
    {
        if (iconPath == null) return;
        var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/{iconPath}";
        if (File.Exists(uploadPath))
        {
            File.Delete(uploadPath);
        }
    }

    public enum MethodSelector
    {
        Art,
        Artist
    }

    private readonly Dictionary<MethodSelector, string> _methods = new()
    {
        { MethodSelector.Art, "arts" },
        { MethodSelector.Artist, "artists" }
    };
}