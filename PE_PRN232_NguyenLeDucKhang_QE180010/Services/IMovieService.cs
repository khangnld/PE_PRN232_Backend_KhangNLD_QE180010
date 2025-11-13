using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Responses;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Services;

public interface IMovieService
{
    Task<List<MovieResponse>> GetAllMoviesAsync();
    Task<MovieResponse?> GetMovieByIdAsync(string id);
    Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request);
    Task<MovieResponse?> UpdateMovieAsync(string id, UpdateMovieRequest request);
    Task<bool> DeleteMovieAsync(string id);
    Task<List<MovieResponse>> SearchMoviesByTitleAsync(string searchTerm);
    Task<List<MovieResponse>> FilterMoviesByGenreAsync(string genre);
    Task<List<MovieResponse>> SortMoviesByRatingAsync(bool ascending = true);
    Task<List<MovieResponse>> SortMoviesByTitleAsync(bool ascending = true);
}

