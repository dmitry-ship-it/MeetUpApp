using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using MeetUpApp.Resources;
using MeetUpApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MeetUpApp.Managers
{
    public class UserManager
    {
        private readonly IConfigurationSection config;
        private readonly IRepository<User> repository;

        public UserManager(IConfiguration config, IRepository<User> repository)
        {
            this.config = config.GetSection("AuthSettings");
            this.repository = repository;
        }

        /// <summary>
        /// Creates User object for database.
        /// This algorithm uses PBKDF2 with HMAC-SHA-256 and random 32-byte 'salt' to protect user password.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>User instance with filled Username, PasswordHash and Salt (in base64 form).</returns>
        public async Task AddUser(
            string name,
            string password,
            CancellationToken cancellationToken = default)
        {
            // generate random salt (256 bit)
            var salt = RandomNumberGenerator.GetBytes(32);

            // generate the salted and hashed password
            var saltedAndHashedPassword = SaltAndHashPassword(
                password, Convert.ToBase64String(salt));

            await repository.InsertAsync(new User()
            {
                Name = name,
                PasswordHash = saltedAndHashedPassword,
                Salt = Convert.ToBase64String(salt)
            }, cancellationToken);
        }

        public async Task<User> CheckCredentials(
            UserViewModel viewModel,
            string password,
            CancellationToken cancellationToken = default)
        {
            var user = await GetAsync(
                u => u.Name == viewModel.Username,
                cancellationToken);

            var saltedAndHashedPassword = SaltAndHashPassword(password, user.Salt);
            if (saltedAndHashedPassword != user.PasswordHash)
            {
                throw new ArgumentException(ExceptionMessages.InvalidPassword);
            }

            return user;
        }

        public void CreateAuthenticationTicket(User user, ISession session)
        {
            var key = Encoding.Default.GetBytes(config["Token"]!);
            var JWToken = new JwtSecurityToken(
                issuer: config["ApiDomain"],
                audience: config["ApiDomain"],
                claims: GetUserClaims(user),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature));

            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            session.SetString("JWToken", token);
        }

        public async Task<User> GetAsync(
            Expression<Func<User, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            var user = await repository.GetByExpressionAsync(
                expression, cancellationToken);

            if (user is null)
            {
                throw new ArgumentException(ExceptionMessages.UserNotFound);
            }

            return user;
        }

        private static IEnumerable<Claim> GetUserClaims(User user)
        {
            var claims = new List<Claim>();
            var claim = new Claim(ClaimTypes.Name, user.Name);
            claims.Add(claim);

            return claims;
        }

        private static string SaltAndHashPassword(string password, string salt)
        {
            var bytes = Encoding.Default.GetBytes(password + salt);
            return Convert.ToBase64String(SHA512.HashData(bytes)); // 512 bit
        }
    }
}