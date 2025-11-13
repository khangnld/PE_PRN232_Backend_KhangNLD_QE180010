using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PE_PRN232_NguyenLeDucKhang_QE180010.Models;

public class Movie
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("genre")]
    public string? Genre { get; set; }

    [BsonElement("rating")]
    public int? Rating { get; set; }

    [BsonElement("posterImageUrl")]
    public string? PosterImageUrl { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

