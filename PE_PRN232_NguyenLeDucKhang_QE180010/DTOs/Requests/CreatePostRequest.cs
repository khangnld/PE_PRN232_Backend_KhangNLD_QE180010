using System.ComponentModel.DataAnnotations;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;

public class CreatePostRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }
}





