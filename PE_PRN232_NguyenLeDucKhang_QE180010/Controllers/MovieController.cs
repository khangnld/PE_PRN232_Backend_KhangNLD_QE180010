using Microsoft.AspNetCore.Mvc;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Responses;
using PE_PRN232_NguyenLeDucKhang_QE180010.Services;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly ILogger<MovieController> _logger;

    public MovieController(IMovieService movieService, ILogger<MovieController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<MovieResponse>>>> GetAllMovies(
        [FromQuery] string? search,
        [FromQuery] string? genre,
        [FromQuery] string? sortBy,
        [FromQuery] bool ascending = true)
    {
        try
        {
            List<MovieResponse> movies;

            // Get all movies first
            movies = await _movieService.GetAllMoviesAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                movies = movies.Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply genre filter
            if (!string.IsNullOrWhiteSpace(genre))
            {
                movies = movies.Where(m => m.Genre?.Equals(genre, StringComparison.OrdinalIgnoreCase) == true).ToList();
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.ToLower() == "rating")
                {
                    movies = ascending
                        ? movies.OrderBy(m => m.Rating ?? 0).ToList()
                        : movies.OrderByDescending(m => m.Rating ?? 0).ToList();
                }
                else if (sortBy.ToLower() == "title")
                {
                    movies = ascending
                        ? movies.OrderBy(m => m.Title).ToList()
                        : movies.OrderByDescending(m => m.Title).ToList();
                }
            }

            return Ok(new ApiResponse<List<MovieResponse>>
            {
                Success = true,
                Message = "Movies retrieved successfully",
                Data = movies
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving movies");
            return StatusCode(500, new ApiResponse<List<MovieResponse>>
            {
                Success = false,
                Message = "Error retrieving movies",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MovieResponse>>> GetMovieById(string id)
    {
        try
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound(new ApiResponse<MovieResponse>
                {
                    Success = false,
                    Message = "Movie not found",
                    Errors = new List<string> { "Movie not found" }
                });
            }

            return Ok(new ApiResponse<MovieResponse>
            {
                Success = true,
                Message = "Movie retrieved successfully",
                Data = movie
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving movie");
            return StatusCode(500, new ApiResponse<MovieResponse>
            {
                Success = false,
                Message = "Error retrieving movie",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MovieResponse>>> CreateMovie([FromForm] CreateMovieRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<MovieResponse>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var movie = await _movieService.CreateMovieAsync(request);
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, new ApiResponse<MovieResponse>
            {
                Success = true,
                Message = "Movie created successfully",
                Data = movie
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating movie");
            return StatusCode(500, new ApiResponse<MovieResponse>
            {
                Success = false,
                Message = "Error creating movie",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<MovieResponse>>> UpdateMovie(string id, [FromForm] UpdateMovieRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<MovieResponse>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var movie = await _movieService.UpdateMovieAsync(id, request);
            if (movie == null)
            {
                return NotFound(new ApiResponse<MovieResponse>
                {
                    Success = false,
                    Message = "Movie not found",
                    Errors = new List<string> { "Movie not found" }
                });
            }

            return Ok(new ApiResponse<MovieResponse>
            {
                Success = true,
                Message = "Movie updated successfully",
                Data = movie
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating movie");
            return StatusCode(500, new ApiResponse<MovieResponse>
            {
                Success = false,
                Message = "Error updating movie",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteMovie(string id)
    {
        try
        {
            var result = await _movieService.DeleteMovieAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Movie not found",
                    Errors = new List<string> { "Movie not found" }
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Movie deleted successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting movie");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Error deleting movie",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}

