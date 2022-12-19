using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiPurposeProject.Entities;

public class Product
{

    [BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("code")]
    public int Code { get; set; }

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("brand")]
    public string Brand { get; set; } = string.Empty;

}

