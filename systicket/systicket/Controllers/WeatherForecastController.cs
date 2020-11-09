using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace systicket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Models.Person> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Models.Person
            {
                Id = 01,
                Name = "Allysson",
                CPF = "017.052.910-07",
                Email = "ayslan@email",
                isManager = true
            })
            .ToArray();
        }
    }
}
