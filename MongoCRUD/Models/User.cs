using MongoDB.Bson.Serialization.Attributes;
using System;

namespace webblabb2net.Models
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
