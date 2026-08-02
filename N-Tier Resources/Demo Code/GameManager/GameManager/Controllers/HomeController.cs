using GameManager.DAL;
using GameManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameMngrService _service;

        public HomeController(ILogger<HomeController> logger, GameMngrService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            _service.EnsureSeedData();

            var viewModels = _service.GetCharacterOverviews();
            return View(viewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // Ta Da! '~'

    }
}
