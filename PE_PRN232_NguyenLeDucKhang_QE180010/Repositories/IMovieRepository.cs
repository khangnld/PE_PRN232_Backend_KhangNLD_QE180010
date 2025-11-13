using PE_PRN232_NguyenLeDucKhang_QE180010.Models;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Repositories;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(string id);
    Task<Movie> CreateAsync(Movie movie);
    Task<Movie?> UpdateAsync(string id, Movie movie);
    Task<bool> DeleteAsync(string id);
    Task<List<Movie>> SearchByTitleAsync(string searchTerm);
    Task<List<Movie>> FilterByGenreAsync(string genre);
    Task<List<Movie>> SortByRatingAsync(bool ascending = true);
    Task<List<Movie>> SortByTitleAsync(bool ascending = true);
}

