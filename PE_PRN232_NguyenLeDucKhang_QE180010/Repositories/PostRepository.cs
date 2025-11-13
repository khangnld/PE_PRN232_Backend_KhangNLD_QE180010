using MongoDB.Driver;
using PE_PRN232_NguyenLeDucKhang_QE180010.Models;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IMongoCollection<Post> _posts;

    public PostRepository(IMongoDatabase database)
    {
        _posts = database.GetCollection<Post>("Posts");
    }

    public async Task<List<Post>> GetAllAsync()
    {
        return await _posts.Find(_ => true).ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(string id)
    {
        return await _posts.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Post> CreateAsync(Post post)
    {
        await _posts.InsertOneAsync(post);
        return post;
    }

    public async Task<Post?> UpdateAsync(string id, Post post)
    {
        post.UpdatedAt = DateTime.UtcNow;
        var result = await _posts.ReplaceOneAsync(p => p.Id == id, post);
        return result.ModifiedCount > 0 ? post : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _posts.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<Post>> SearchByNameAsync(string searchTerm)
    {
        var filter = Builders<Post>.Filter.Regex("name", new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
        return await _posts.Find(filter).ToListAsync();
    }

    public async Task<List<Post>> SortByNameAsync(bool ascending = true)
    {
        var sort = ascending 
            ? Builders<Post>.Sort.Ascending(p => p.Name)
            : Builders<Post>.Sort.Descending(p => p.Name);
        
        return await _posts.Find(_ => true).Sort(sort).ToListAsync();
    }
}



