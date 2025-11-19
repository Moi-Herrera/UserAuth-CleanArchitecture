using Microsoft.EntityFrameworkCore;
using UserAuth.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

//Conexion desde appsettings.json
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Registrar EF Core
builder.Services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(ConnectionString)); 



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   // app.UseSwagger();
   // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
