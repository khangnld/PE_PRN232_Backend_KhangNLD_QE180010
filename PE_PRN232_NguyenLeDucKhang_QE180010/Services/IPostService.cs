using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Requests;
using PE_PRN232_NguyenLeDucKhang_QE180010.DTOs.Responses;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Services;

public interface IPostService
{
    Task<List<PostResponse>> GetAllPostsAsync();
    Task<PostResponse?> GetPostByIdAsync(string id);
    Task<PostResponse> CreatePostAsync(CreatePostRequest request);
    Task<PostResponse?> UpdatePostAsync(string id, UpdatePostRequest request);
    Task<bool> DeletePostAsync(string id);
    Task<List<PostResponse>> SearchPostsByNameAsync(string searchTerm);
    Task<List<PostResponse>> SortPostsByNameAsync(bool ascending = true);
}



