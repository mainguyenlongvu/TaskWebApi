using TaskWebApi.Repositories.Entities;
using TaskWebApi.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using TaskWebApi.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaskWebApi.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseLoginModel> AuthenticatorAsync(RequestLoginModel model);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string Key = "KeyAuthentication";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseLoginModel> AuthenticatorAsync(RequestLoginModel model)
        {
            //var account =             //viết repository get UserEntity
            //if (account == null)
            //{
            // kiểm tra nếu user == null thì thorw ex
            //}
            var userRepository =  _unitOfWork.UserRepository;
            var user = await userRepository.GetByEmailAsync(model.Email);
            if(user == null)
            {
                throw new Exception("User is not found.");
            }

            var token = CreateJwtToken(user);
            var refreshToken = CreateRefreshToken(user);
            var result = new ResponseLoginModel
            {
                FullName = user.Name,
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken.Token
            };
            return result;
        }

        private RefreshTokens CreateRefreshToken(UserEntity account)
        {
            var randomByte = new byte[64];
            var token = RandomStringGenerator.GenerateRandomString(10); // Viết hàm tạo chuỗi random string
            var refreshToken = new RefreshTokens
            {
                UserId = account.Id,
                Expires = DateTime.Now.AddDays(1),
                IsActive = true,
                Token = token
            };
            
            // viết code insert refreshToken vào DB
            _unitOfWork.RefreshTokensRepository.AddRefreshTokensAsync(refreshToken);
            _unitOfWork.SaveChangesAsync();
            return refreshToken;
        }
        public class RandomStringGenerator //hàm tạo chuỗi random string
        {
            private static Random random = new Random();
            private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            public static string GenerateRandomString(int length)
            {
                char[] stringChars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                return new string(stringChars);
            }
        }

        private string CreateJwtToken(UserEntity account)
        {
            var tokenHanler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Key);
            var securityKey = new SymmetricSecurityKey(key);
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(ClaimTypes.Name, account.Name),
                    new System.Security.Claims.Claim(ClaimTypes.Email, "sdasdasd"),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credential
            };
            var token = tokenHanler.CreateToken(tokenDescription);
            return tokenHanler.WriteToken(token);
        }
    }
}
