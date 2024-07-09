using crud_application.service;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using crud_application.Models;

// Set your Cloudinary credentials
//=================================

//Cloudinary cloudinary = new Cloudinary("cloudinary://898341683799318:Fdi5ztVovbbVC06xOHr9eqVH5as@dufo0m5eu");
//cloudinary.Api.Secure = true;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// Register CloudinarySettings from configuration (appsettings.json or environment variables)
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// Register CloudinaryService
builder.Services.AddTransient<CloudinaryService>();

builder.Services.AddAuthorization();

// Configuration
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<CarService>();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<ItemService>();
builder.Services.AddTransient<UserService>();

// Singleton MongoClient instance
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();



//var uploadParams = new ImageUploadParams()
//{
//    File = new FileDescription(@"https://cloudinary-devs.github.io/cld-docs-assets/assets/images/cld-sample.jpg"),
//    UseFilename = true,
//    UniqueFilename = false,
//    Overwrite = true
//};
//var uploadResult = cloudinary.Upload(uploadParams);
//Console.WriteLine(uploadResult.JsonObj);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
