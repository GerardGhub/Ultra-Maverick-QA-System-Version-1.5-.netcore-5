using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerQAAPICORE.Controllers
{
  public class HomeController : Controller
  {
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("home/index")]
    public IActionResult Index()
    {
      return View();
    }
  }
}
