using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModelBindingSample.Models
{
    [ModelBinder(BinderType = typeof(IssueModelBinder),Name = nameof(Issue))]
    public class Issue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Assignee { get; set; }
        public User Reporter { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Issue()
        {
            Id = ObjectId.Empty.ToString();
            Name = Description = String.Empty;
            Assignee = Reporter = new User();
            CreatedDate = ModificationDate = DateTime.MinValue;
        }
    }
}
