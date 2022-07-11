using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace Sphere.Accounts.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IClusterClient _client;

        public AccountsController(
            ILogger<AccountsController> logger,
            IClusterClient clusterClient)
        {
            _logger = logger;
            _client = clusterClient;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
