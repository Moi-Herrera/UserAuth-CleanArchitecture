
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAuth.Aplication.Interfaces;
using UserAuth.Infrastructure.Data;
using UserAuth.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Conexion desde appsettings.json
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Registrar EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(ConnectionString));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IuserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefeshTokenRepository>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
