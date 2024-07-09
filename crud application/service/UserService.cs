using crud_application.Controllers;
using crud_application.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace crud_application.service
{
    public class UserService
    {
        private readonly IMongoCollection<UserModel> _User;
        private readonly IConfiguration _config;
        private readonly ILogger<UserService> _logger;

        public UserService(IConfiguration config, ILogger<UserService> logger)
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("CarRent");
            _User = database.GetCollection<UserModel>("UserCollection");
            _config = config;
            _logger = logger;
        }

        public async Task<Object> SignIn(UserModel user)
        {
            _logger.LogInformation("User found in service: {UserName}", user.UserName);
            var currentUser=await _User.Find(p => p.UserName == user.UserName).FirstOrDefaultAsync();
            _logger.LogInformation("User found in current service: {UserName}", currentUser);
            if (currentUser != null)
            {
                if(currentUser.Password == user.Password) {
                    var token = GenerateToken();
                    return  token ;
                }
                else
                {
                    return "Incorrect Password";
                }
            }
            else
            {
                return "User Not Found";
            }
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
