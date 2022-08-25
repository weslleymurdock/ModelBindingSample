using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelBindingSample.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Username")]
        public string Username { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [BsonElement("IsAdmin")]
        public bool IsAdmin { get; set; }
        public User()
        {
            Id = ObjectId.Empty.ToString();
            Name = Username = Password = Email = string.Empty;
            CreatedDate = DateTime.MinValue;
        }
    }
}
