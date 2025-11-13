using Supabase;
using Supabase.Storage;
using StorageFileOptions = Supabase.Storage.FileOptions;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Services;

public class SupabaseService : ISupabaseService
{
    private readonly Supabase.Client _supabase;
    private readonly string _bucketName = "image";
    private readonly string _supabaseUrl;
    private readonly string _supabaseKey;

    public SupabaseService(IConfiguration configuration)
    {
        _supabaseUrl = configuration["SUPABASE_URL"] ?? Environment.GetEnvironmentVariable("SUPABASE_URL") ?? "";
        _supabaseKey = configuration["SUPABASE_KEY"] ?? Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? "";

        var options = new SupabaseOptions
        {
            AutoConnectRealtime = false
        };

        _supabase = new Supabase.Client(_supabaseUrl, _supabaseKey, options);
    }

    public async Task<string> UploadImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("Image file is required");

        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
        var fileExtension = Path.GetExtension(image.FileName).ToLower();

        if (!new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" }.Contains(fileExtension))
            throw new ArgumentException("Invalid image format. Only JPG, PNG, GIF, and WEBP are allowed.");

        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);
        var imageBytes = memoryStream.ToArray();

        var bucket = _supabase.Storage.From(_bucketName);
        var result = await bucket.Upload(imageBytes, fileName, new StorageFileOptions
        {
            ContentType = image.ContentType,
            Upsert = false
        });

        if (result == null)
            throw new Exception("Failed to upload image to Supabase");

        var publicUrl = bucket.GetPublicUrl(fileName);
        return publicUrl;
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return false;

        try
        {
            var uri = new Uri(imageUrl);
            var fileName = Path.GetFileName(uri.LocalPath);
            var bucket = _supabase.Storage.From(_bucketName);
            await bucket.Remove(fileName);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

