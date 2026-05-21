using BLL;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //standard routing til api/lægehus
    [Route("api/[controller]")]
    [ApiController]
    public class LægehusController : ControllerBase
    {
        private readonly ReceptManager _manager;
        public LægehusController()
        {
            //forbinder til BLL
            _manager = new ReceptManager();
        }

        //GET med ydernummer som parameter
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
