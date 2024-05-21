using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using auth_jwt_net8.Custom;
using auth_jwt_net8.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de la cadena de conexión
builder.Services.AddDbContext<DbauthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configuración de la clase Utilities
builder.Services.AddSingleton<Utilities>();

// Configuración de JWT
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Se valida la clave de firma
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)), // Clave secreta
        ValidateIssuer = false, // No se valida el emisor del token
        ValidateAudience = false, // No se valida el receptor del token
        ValidateLifetime = true, // Se valida la vigencia del token
        ClockSkew = TimeSpan.Zero // Se establece el tiempo de expiración en 0

    };

});

// Cors, esto para que se pueda consumir la API desde cualquier origen.
// Si se desea restringir el acceso a solo algunos origenes, se puede hacer de la siguiente manera:
// options.AddPolicy("AllowSpecificOrigin", builder =>
// {
//     builder.WithOrigins("http://example.com");
// });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
