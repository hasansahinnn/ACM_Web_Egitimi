using ACMWebApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ACMWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private List<User> _users = new List<User>
        {
            new User { Id = 1, Ad = "hasan", Soyad = "sahin", KullaniciAdi = "acmweb1", Sifre = "1234", Auth = "User" },
            new User { Id = 2, Ad = "web", Soyad = "admin", KullaniciAdi = "acmweb2", Sifre = "4321", Auth = "Admin"  }
        };
        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public BaseResponse<User> Authenticate(string kullaniciAdi, string sifre)
        {
            var user = _users.SingleOrDefault(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == sifre);

            // Kullanici bulunamadıysa null döner.
            if (user == null)
                return null;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secrettoken123******"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Auth),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            
            var token = new JwtSecurityToken(
                issuer: "userkey",
                audience: "userkey",
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
                );
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);

            user.Token = encodetoken;
            // Sifre null olarak gonderilir.
            user.Sifre = null;

            return new BaseResponse<User>(200, "Başarılı", user);
        }
        

    }
    public class BaseResponse<T>
    {
        public BaseResponse()
        {

        }
        public BaseResponse(int statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public BaseResponse<T> Success(T data)
        {
            var response = new BaseResponse<T>();
            response.StatusCode = 200;
            response.Message = "Başarılı";
            response.Data = data;
            return response;
        }
    }
}
