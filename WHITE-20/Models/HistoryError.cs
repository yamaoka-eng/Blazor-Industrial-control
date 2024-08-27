using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WHITE_20.Models
{
    public class HistoryError : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string HistoryDate { get; set; } = "";
        public string RestoreDate { get; set; } = "";
        public string Content { get; set; } = "";
    }
}
