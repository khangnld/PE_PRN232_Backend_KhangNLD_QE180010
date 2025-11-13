using System.ComponentModel.DataAnnotations;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;

public class CreateMovieRequest
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters")]
    public string? Genre { get; set; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int? Rating { get; set; }

    public IFormFile? PosterImage { get; set; }

    public string? PosterImageUrl { get; set; }
}

