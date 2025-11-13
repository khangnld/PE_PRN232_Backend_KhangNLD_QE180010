using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Responses;
using PE_PRN232_NguyenLeDucKhang_QE180010.Models;
using PE_PRN232_NguyenLeDucKhang_QE180010.Repositories;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly ISupabaseService _supabaseService;

    public PostService(IPostRepository postRepository, ISupabaseService supabaseService)
    {
        _postRepository = postRepository;
        _supabaseService = supabaseService;
    }

    public async Task<List<PostResponse>> GetAllPostsAsync()
    {
        var posts = await _postRepository.GetAllAsync();
        return posts.Select(MapToResponse).ToList();
    }

    public async Task<PostResponse?> GetPostByIdAsync(string id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        return post != null ? MapToResponse(post) : null;
    }

    public async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
    {
        string? imageUrl = null;

        if (request.Image != null)
        {
            imageUrl = await _supabaseService.UploadImageAsync(request.Image);
        }

        var post = new Post
        {
            Name = request.Name,
            Description = request.Description,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdPost = await _postRepository.CreateAsync(post);
        return MapToResponse(createdPost);
    }

    public async Task<PostResponse?> UpdatePostAsync(string id, UpdatePostRequest request)
    {
        var existingPost = await _postRepository.GetByIdAsync(id);
        if (existingPost == null)
            return null;

        string? imageUrl = existingPost.ImageUrl;

        if (request.Image != null)
        {
            if (!string.IsNullOrEmpty(existingPost.ImageUrl))
            {
                await _supabaseService.DeleteImageAsync(existingPost.ImageUrl);
            }
            imageUrl = await _supabaseService.UploadImageAsync(request.Image);
        }

        existingPost.Name = request.Name;
        existingPost.Description = request.Description;
        existingPost.ImageUrl = imageUrl;
        existingPost.UpdatedAt = DateTime.UtcNow;

        var updatedPost = await _postRepository.UpdateAsync(id, existingPost);
        return updatedPost != null ? MapToResponse(updatedPost) : null;
    }

    public async Task<bool> DeletePostAsync(string id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            return false;

        if (!string.IsNullOrEmpty(post.ImageUrl))
        {
            await _supabaseService.DeleteImageAsync(post.ImageUrl);
        }

        return await _postRepository.DeleteAsync(id);
    }

    public async Task<List<PostResponse>> SearchPostsByNameAsync(string searchTerm)
    {
        var posts = await _postRepository.SearchByNameAsync(searchTerm);
        return posts.Select(MapToResponse).ToList();
    }

    public async Task<List<PostResponse>> SortPostsByNameAsync(bool ascending = true)
    {
        var posts = await _postRepository.SortByNameAsync(ascending);
        return posts.Select(MapToResponse).ToList();
    }

    private PostResponse MapToResponse(Post post)
    {
        return new PostResponse
        {
            Id = post.Id ?? string.Empty,
            Name = post.Name,
            Description = post.Description,
            ImageUrl = post.ImageUrl,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }
}



