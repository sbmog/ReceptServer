using BLL;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    //standard routing til api/recepter
    [Route("api/[controller]")]
    [ApiController]
    public class RecepterController : ControllerBase
    {
        private readonly ReceptManager _manager;
        public RecepterController()
        {
            //forbinder til BLL
            _manager = new ReceptManager();
        }
        [HttpPost]
        public IActionResult OpretRecept([FromBody] Recept nyRecept)
        {
            if (nyRecept ==null)
            {
                return BadRequest("Receptdata er tom eller ugyldig");
            }
            try
            {
                _manager.OpretRecept(nyRecept);
                return Ok("Recept oprettet");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fejl ved oprettelse af recept: {ex.Message}");
            }
        }
    }
}
