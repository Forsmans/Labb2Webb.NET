using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using webblabb2net.Models;

namespace webblabb2net.Data
{
    public class MongoDB
    {
        private IMongoDatabase db;

        public MongoDB(string database)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase(database);
        }


        //C
        public async Task<User> AddUser(string table, User user)
        {
            var collection = db.GetCollection<User>(table);
            await collection.InsertOneAsync(user);
            return user;
        }

        //R all
        public Task<List<User>> GetUsers(string table)
        {
            var collection = db.GetCollection<User>(table);
            return collection.AsQueryable().ToListAsync();
        }

        //R by id
        public async Task<User> GetUser(string table, Guid id)
        {
            var collection = db.GetCollection<User>(table);
            var users = await collection.AsQueryable().ToListAsync();
            var user = users.Find(u => u.Id == id);
            return user;
        }

        //U
        public async Task<User> UpdateUser(string table, Guid id, User updateUser)
        {
            var collection = db.GetCollection<User>(table);
            var users = await collection.AsQueryable().ToListAsync();
            var user = users.Find(u => u.Id == id);

            if (user != null)
            {
                var filter = Builders<User>.Filter.Eq(u => u.Id, id);

                await collection.UpdateOneAsync(filter, Builders<User>.Update.Set(u => u.Name, updateUser.Name).Set(u => u.Age, updateUser.Age));
            }

            return user;
        }

        //D
        public async Task<User> DeleteUser(string table, Guid id)
        {
            var collection = db.GetCollection<User>(table);
            var filter = Builders<User>.Filter.Eq(e => e.Id, id);

            var userToDelete = await collection.Find(filter).FirstOrDefaultAsync();

            if (userToDelete != null)
            {
                await collection.DeleteOneAsync(filter);
            }

            return userToDelete;
        }
    }
}
