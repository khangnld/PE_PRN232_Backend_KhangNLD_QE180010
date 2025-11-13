using MongoDB.Driver;
using PE_PRN232_NguyenLeDucKhang_QE180010.Models;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IMongoCollection<Movie> _movies;

    public MovieRepository(IMongoDatabase database)
    {
        _movies = database.GetCollection<Movie>("Movies");
    }

    public async Task<List<Movie>> GetAllAsync()
    {
        return await _movies.Find(_ => true).ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(string id)
    {
        return await _movies.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        await _movies.InsertOneAsync(movie);
        return movie;
    }

    public async Task<Movie?> UpdateAsync(string id, Movie movie)
    {
        movie.UpdatedAt = DateTime.UtcNow;
        var result = await _movies.ReplaceOneAsync(m => m.Id == id, movie);
        return result.ModifiedCount > 0 ? movie : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _movies.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<Movie>> SearchByTitleAsync(string searchTerm)
    {
        var filter = Builders<Movie>.Filter.Regex("title", new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"));
        return await _movies.Find(filter).ToListAsync();
    }

    public async Task<List<Movie>> FilterByGenreAsync(string genre)
    {
        var filter = Builders<Movie>.Filter.Eq("genre", genre);
        return await _movies.Find(filter).ToListAsync();
    }

    public async Task<List<Movie>> SortByRatingAsync(bool ascending = true)
    {
        var sort = ascending 
            ? Builders<Movie>.Sort.Ascending(m => m.Rating)
            : Builders<Movie>.Sort.Descending(m => m.Rating);
        
        return await _movies.Find(_ => true).Sort(sort).ToListAsync();
    }

    public async Task<List<Movie>> SortByTitleAsync(bool ascending = true)
    {
        var sort = ascending 
            ? Builders<Movie>.Sort.Ascending(m => m.Title)
            : Builders<Movie>.Sort.Descending(m => m.Title);
        
        return await _movies.Find(_ => true).Sort(sort).ToListAsync();
    }
}

