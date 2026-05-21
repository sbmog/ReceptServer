using BLL;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ApotekUI.Controllers
{
    public class ReceptController : Controller
    {
        private readonly ReceptManager _manager;
        public ReceptController()
        {
            _manager = new ReceptManager();
        }

        //Tom søge side
        [HttpGet]
        public IActionResult Index(string cprNummer)
        {
            if (string.IsNullOrEmpty(cprNummer))
            {
                return View(new List<Recept>());
            }

            var recepter = _manager.HentRecepterPåCpr(cprNummer);

            //cprNummeret sendes tilbage til Viewet, så det huskes ved navigering tilbage.
            ViewBag.LastCpr = cprNummer;

            return View(recepter);
        }

        //viser data for en specifik recept + udeleveringsmuligheder
        [HttpGet]
        public IActionResult Udlevering (int id)
        {
            var recept = _manager.HentRecept(id);
            if (recept == null)
            {
                return NotFound("Recepten blev ikke fundet.");
            }
            return View(recept);
        }

        [HttpPost]
        public IActionResult ForetagUdlevering(int receptId, int ordinationId)
        {
            try
            {
                var recept = _manager.HentRecept(receptId);
                var ordination = recept?.Ordinationer.FirstOrDefault(o => o.Id == ordinationId);
                if (ordination != null)
                {
                    _manager.ForetagUdlevering(ordination);
                    TempData["Besked"] = "Udlevering foretaget.";
                }
            }
            catch (Exception ex)
            {
                TempData["Fejl"] = $"Fejl ved udlevering: {ex.Message}";
            }
            return RedirectToAction("Udlevering", new { id = receptId });
        }
    }
}
