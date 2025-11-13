using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Responses;
using PE_PRN232_NguyenLeDucKhang_QE180010.Models;
using PE_PRN232_NguyenLeDucKhang_QE180010.Repositories;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly ISupabaseService _supabaseService;

    public MovieService(IMovieRepository movieRepository, ISupabaseService supabaseService)
    {
        _movieRepository = movieRepository;
        _supabaseService = supabaseService;
    }

    public async Task<List<MovieResponse>> GetAllMoviesAsync()
    {
        var movies = await _movieRepository.GetAllAsync();
        return movies.Select(MapToResponse).ToList();
    }

    public async Task<MovieResponse?> GetMovieByIdAsync(string id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        return movie != null ? MapToResponse(movie) : null;
    }

    public async Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request)
    {
        string? posterImageUrl = null;

        if (request.PosterImage != null)
        {
            posterImageUrl = await _supabaseService.UploadImageAsync(request.PosterImage);
        }
        else if (!string.IsNullOrEmpty(request.PosterImageUrl))
        {
            posterImageUrl = request.PosterImageUrl;
        }

        var movie = new Movie
        {
            Title = request.Title,
            Genre = request.Genre,
            Rating = request.Rating,
            PosterImageUrl = posterImageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdMovie = await _movieRepository.CreateAsync(movie);
        return MapToResponse(createdMovie);
    }

    public async Task<MovieResponse?> UpdateMovieAsync(string id, UpdateMovieRequest request)
    {
        var existingMovie = await _movieRepository.GetByIdAsync(id);
        if (existingMovie == null)
            return null;

        string? posterImageUrl = existingMovie.PosterImageUrl;

        if (request.PosterImage != null)
        {
            if (!string.IsNullOrEmpty(existingMovie.PosterImageUrl))
            {
                await _supabaseService.DeleteImageAsync(existingMovie.PosterImageUrl);
            }
            posterImageUrl = await _supabaseService.UploadImageAsync(request.PosterImage);
        }
        else if (!string.IsNullOrEmpty(request.PosterImageUrl))
        {
            if (!string.IsNullOrEmpty(existingMovie.PosterImageUrl) && existingMovie.PosterImageUrl != request.PosterImageUrl)
            {
                await _supabaseService.DeleteImageAsync(existingMovie.PosterImageUrl);
            }
            posterImageUrl = request.PosterImageUrl;
        }

        existingMovie.Title = request.Title;
        existingMovie.Genre = request.Genre;
        existingMovie.Rating = request.Rating;
        existingMovie.PosterImageUrl = posterImageUrl;
        existingMovie.UpdatedAt = DateTime.UtcNow;

        var updatedMovie = await _movieRepository.UpdateAsync(id, existingMovie);
        return updatedMovie != null ? MapToResponse(updatedMovie) : null;
    }

    public async Task<bool> DeleteMovieAsync(string id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null)
            return false;

        if (!string.IsNullOrEmpty(movie.PosterImageUrl))
        {
            await _supabaseService.DeleteImageAsync(movie.PosterImageUrl);
        }

        return await _movieRepository.DeleteAsync(id);
    }

    public async Task<List<MovieResponse>> SearchMoviesByTitleAsync(string searchTerm)
    {
        var movies = await _movieRepository.SearchByTitleAsync(searchTerm);
        return movies.Select(MapToResponse).ToList();
    }

    public async Task<List<MovieResponse>> FilterMoviesByGenreAsync(string genre)
    {
        var movies = await _movieRepository.FilterByGenreAsync(genre);
        return movies.Select(MapToResponse).ToList();
    }

    public async Task<List<MovieResponse>> SortMoviesByRatingAsync(bool ascending = true)
    {
        var movies = await _movieRepository.SortByRatingAsync(ascending);
        return movies.Select(MapToResponse).ToList();
    }

    public async Task<List<MovieResponse>> SortMoviesByTitleAsync(bool ascending = true)
    {
        var movies = await _movieRepository.SortByTitleAsync(ascending);
        return movies.Select(MapToResponse).ToList();
    }

    private MovieResponse MapToResponse(Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id ?? string.Empty,
            Title = movie.Title,
            Genre = movie.Genre,
            Rating = movie.Rating,
            PosterImageUrl = movie.PosterImageUrl,
            CreatedAt = movie.CreatedAt,
            UpdatedAt = movie.UpdatedAt
        };
    }
}

