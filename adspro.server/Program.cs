using Adspro.Server.API.Extensions;
using Adspro.Server.Application.Interfaces;
using Adspro.Server.Application.Services;
using Adspro.Server.Domain.Interfaces;
using Adspro.Server.Infrastructure.Data;
using Adspro.Server.Infrastructure.Extensions;
using Adspro.Server.Infrastructure.Repositories;
using Adspro.Server.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<AdsDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.Services.InitializeDatabase();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("-=== dev mode ===-");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");

app.Run();
