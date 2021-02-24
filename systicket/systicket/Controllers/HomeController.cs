using Microsoft.AspNetCore.Mvc;

namespace systicket.Controllers
{
    [ApiController]    
    public class HomeController : ControllerBase
    { 
        public HomeController()
        {   
        }

        [HttpGet]
        [Route("")]
        [Route("api/[controller]")]
        [Route("api/[controller]/teste")]
        public string HomeIndex()
        {
            return "API Systicket pronta e rodando..";
        }
    }
}
