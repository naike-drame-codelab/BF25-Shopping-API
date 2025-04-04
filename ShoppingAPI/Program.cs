using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.API.Security;
using Shopping.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShoppingContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    );

// ajout de notre générateur de JWT
TokenManager.Config config = builder.Configuration.GetSection("Jwt").Get<TokenManager.Config>() ?? throw new Exception("Missing Jwt config.");

builder.Services.AddSingleton<ITokenManager, TokenManager>(
    _ => new TokenManager(config)
 );

// configuration du middleware d'authentification
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        o => o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config.Issuer,
            ValidateAudience = true,
            ValidAudience = config.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret))
        }
    );


// CORS : Cross-Origin Resource Sharing
// partage de ressource inter-domaine
builder.Services.AddCors(p =>
{
    p.AddDefaultPolicy(o =>
        o.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin() // l'API devient publique
                          // .WithOrigins("https://www.mojovelo.be")
                          // .WIthMethods("Get") // API accessible uniquement en GET
    );
    p.AddPolicy("OtherPolicy", o =>
    o.WithOrigins("https://www.mojovelo.be")
    .WithMethods("Get")
    .AllowAnyHeader()
    );
});

var app = builder.Build();
app.UseCors();
app.UseCors("OtherPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
