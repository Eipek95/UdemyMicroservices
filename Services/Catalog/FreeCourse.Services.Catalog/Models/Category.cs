using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Models
{
    public class Category
    {
        [BsonId]//mongodb tanıtma id 
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]//id nin tipini biizm gönderdiğimiz tipe göre belirle
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
