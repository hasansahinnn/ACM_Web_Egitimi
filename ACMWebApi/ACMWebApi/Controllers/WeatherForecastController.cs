using ACMWebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private List<User> _users = new List<User>
        {
            new User { Id = 1, Ad = "hasan", Soyad = "sahin", KullaniciAdi = "acmweb1", Sifre = "1234", Auth = "User" },
            new User { Id = 2, Ad = "hasan", Soyad = "admin", KullaniciAdi = "acmweb2", Sifre = "4321", Auth = "Admin"  }
        };
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<WeatherForecast> Get(int pageNumber, string search)
        {

            var user = _users.Where(x => x.Ad.ToLower().Contains(search) || x.KullaniciAdi.ToLower().Contains(search)).Skip(10* (pageNumber-1)).Take(10).Select(x => new UserDTO
            {
                Ad = x.Ad,
                Soyad = x.Soyad
            });
               
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
