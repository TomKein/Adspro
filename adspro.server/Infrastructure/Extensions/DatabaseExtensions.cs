using Adspro.Server.Domain.Entities;
using Adspro.Server.Infrastructure.Data;
using Adspro.Server.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Adspro.Server.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AdsDbContext>();

        if (!dbContext.Database.GetPendingMigrations().Any()) return;

        dbContext.Database.Migrate();
        SeedData(dbContext);
    }

    private static void SeedData(AdsDbContext dbContext)
    {
        if (dbContext.Users.Any()) return;
        
        var passwordHasher = new PasswordHasher();

        var users = new List<User>();
        for (int i = 1; i <= 20; i++)
        {
            var user = new User(
                login: $"user{i}",
                passwordHash: passwordHasher.HashPassword($"password{i}"));

            users.Add(user);
        }

        dbContext.Users.AddRange(users);
        dbContext.SaveChanges();
    }
    //Эквивалентно
    //migrationBuilder.Sql($@"
    //    INSERT INTO Users (Id, Login, PasswordHash, IsActive)
    //    VALUES (NEWID(), '{login}', '{hashedPassword}', {(isActive ? 1 : 0)})
    //");
}