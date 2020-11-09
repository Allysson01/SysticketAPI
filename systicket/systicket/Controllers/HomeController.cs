using Microsoft.AspNetCore.Mvc;

namespace systicket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    { 
        public HomeController()
        {   
        }

        [HttpGet]
        public string HomeIndex()
        {
            return "API Systicket pronta e rodando..";
        }
    }
}
