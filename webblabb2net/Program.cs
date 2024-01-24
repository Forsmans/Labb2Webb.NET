
using webblabb2net.Data;
using webblabb2net.Models;

namespace webblabb2net
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Mongo
            MongoDB db = new MongoDB("Users");

            // Add this line in your ConfigureServices method
            builder.Services.AddCors();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Add this line in your Configure method, before app.UseAuthorization();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


            //C
            app.MapPost("/user", async (User user) =>
            {
                var newUser = await db.AddUser("Users", user);
                return Results.Ok(newUser);
            });

            //R all
            app.MapGet("/users", async () =>
            {
                var users = await db.GetUsers("Users");
                return Results.Ok(users);
            });

            //R by id
            app.MapGet("/user/{id}", async (Guid id) => {
                var user = await db.GetUser("Users", id);
                return Results.Ok(user);
            });

            //U
            app.MapPut("/user/{id}", async (Guid id, User user) =>
            {
                var updateUser = await db.UpdateUser("Users", id, user);
                return Results.Ok(updateUser);
            });

            //D
            app.MapDelete("/user/{id}", async (Guid id) =>
            {
                var user = await db.DeleteUser("Users", id);
                return Results.Ok(user);
            });

            app.Run();
        }
    }
}
