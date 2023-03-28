using BarcodeAPI.Entities;
using BarcodeAPI.Exceptions;
using BarcodeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using BarcodeAPI.Entities;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarcodeAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly ProductsDbContext _productsDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AppSettings _appSettings;

        public AccountService(ProductsDbContext dbContext, IPasswordHasher<User> passwordHasher, AppSettings appSettings)
        {
            _productsDbContext = dbContext;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings;
        }

        public TokenResponseModel GenerateJWT(LoginUserDto dto)
        {
            var user = _productsDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(x => x.Email == dto.Email);
            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_appSettings.JwtExpireMinutes);
            var token = new JwtSecurityToken(_appSettings.JwtIssuer,
                _appSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();

            var response = new TokenResponseModel()
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresAt = expires,
                StatusCode = 200
            };
            return response;
        }

        public void AddNewUser(AddUserBodyRequest body)
        {
            var newUser = new User()
            {
                Email = body.Email,
                FirstName = body.FirstName,
                LastName = body.LastName,
            };

            var hashedPass = _passwordHasher.HashPassword(newUser, body.Password);
            newUser.PasswordHash = hashedPass;

            _productsDbContext.Users.Add(newUser);
            _productsDbContext.SaveChanges();
        }
    }
}