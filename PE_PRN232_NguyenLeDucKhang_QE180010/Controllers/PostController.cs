using Microsoft.AspNetCore.Mvc;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Responses;
using PE_PRN232_NguyenLeDucKhang_QE180010.Services;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<PostController> _logger;

    public PostController(IPostService postService, ILogger<PostController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PostResponse>>>> GetAllPosts()
    {
        try
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(new ApiResponse<List<PostResponse>>
            {
                Success = true,
                Message = "Posts retrieved successfully",
                Data = posts
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving posts");
            return StatusCode(500, new ApiResponse<List<PostResponse>>
            {
                Success = false,
                Message = "Error retrieving posts",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PostResponse>>> GetPostById(string id)
    {
        try
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound(new ApiResponse<PostResponse>
                {
                    Success = false,
                    Message = "Post not found",
                    Errors = new List<string> { "Post not found" }
                });
            }

            return Ok(new ApiResponse<PostResponse>
            {
                Success = true,
                Message = "Post retrieved successfully",
                Data = post
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving post");
            return StatusCode(500, new ApiResponse<PostResponse>
            {
                Success = false,
                Message = "Error retrieving post",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PostResponse>>> CreatePost([FromForm] CreatePostRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<PostResponse>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var post = await _postService.CreatePostAsync(request);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, new ApiResponse<PostResponse>
            {
                Success = true,
                Message = "Post created successfully",
                Data = post
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating post");
            return StatusCode(500, new ApiResponse<PostResponse>
            {
                Success = false,
                Message = "Error creating post",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PostResponse>>> UpdatePost(string id, [FromForm] UpdatePostRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<PostResponse>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var post = await _postService.UpdatePostAsync(id, request);
            if (post == null)
            {
                return NotFound(new ApiResponse<PostResponse>
                {
                    Success = false,
                    Message = "Post not found",
                    Errors = new List<string> { "Post not found" }
                });
            }

            return Ok(new ApiResponse<PostResponse>
            {
                Success = true,
                Message = "Post updated successfully",
                Data = post
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating post");
            return StatusCode(500, new ApiResponse<PostResponse>
            {
                Success = false,
                Message = "Error updating post",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeletePost(string id)
    {
        try
        {
            var result = await _postService.DeletePostAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Post not found",
                    Errors = new List<string> { "Post not found" }
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Post deleted successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting post");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Error deleting post",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<List<PostResponse>>>> SearchPosts([FromQuery] string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new ApiResponse<List<PostResponse>>
                {
                    Success = false,
                    Message = "Search term is required",
                    Errors = new List<string> { "Search term is required" }
                });
            }

            var posts = await _postService.SearchPostsByNameAsync(name);
            return Ok(new ApiResponse<List<PostResponse>>
            {
                Success = true,
                Message = "Posts retrieved successfully",
                Data = posts
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching posts");
            return StatusCode(500, new ApiResponse<List<PostResponse>>
            {
                Success = false,
                Message = "Error searching posts",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("sort")]
    public async Task<ActionResult<ApiResponse<List<PostResponse>>>> SortPosts([FromQuery] bool ascending = true)
    {
        try
        {
            var posts = await _postService.SortPostsByNameAsync(ascending);
            return Ok(new ApiResponse<List<PostResponse>>
            {
                Success = true,
                Message = "Posts retrieved successfully",
                Data = posts
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sorting posts");
            return StatusCode(500, new ApiResponse<List<PostResponse>>
            {
                Success = false,
                Message = "Error sorting posts",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}



