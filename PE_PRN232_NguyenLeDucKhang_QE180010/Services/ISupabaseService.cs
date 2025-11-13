namespace PE_PRN232_NguyenLeDucKhang_QE180010.Services;

public interface ISupabaseService
{
    Task<string> UploadImageAsync(IFormFile image);
    Task<bool> DeleteImageAsync(string imageUrl);
}



