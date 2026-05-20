using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LægehusController : ControllerBase
    {
        private readonly ReceptManager _manager;
        public LægehusController()
        {
            _manager = new ReceptManager();
        }
        [HttpGet("{ydernummer}")]
        public IActionResult Login(string ydernummer)
        {
            var lægehus = _manager.HentLægehus(ydernummer);
            if (lægehus == null)
            {
                return NotFound("Uguldig ydernummer. Lægehus findes ikke.");
            }
            return Ok(lægehus);
        }
    }
}
