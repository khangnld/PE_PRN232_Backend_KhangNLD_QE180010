using PE_PRN232_NguyenLeDucKhang_QE180010.Models;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Repositories;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(string id);
    Task<Post> CreateAsync(Post post);
    Task<Post?> UpdateAsync(string id, Post post);
    Task<bool> DeleteAsync(string id);
    Task<List<Post>> SearchByNameAsync(string searchTerm);
    Task<List<Post>> SortByNameAsync(bool ascending = true);
}



