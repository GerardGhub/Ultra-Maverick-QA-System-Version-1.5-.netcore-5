using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerQAAPICORE.Controllers
{
  public class RouterLoggerController : Controller
  {
    [Obsolete]
    private readonly IHostingEnvironment _hostingEnvironment;

    [Obsolete]
    public RouterLoggerController(IHostingEnvironment hostingEnvironment)
    {
      this._hostingEnvironment = hostingEnvironment;
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost]
    [Route("api/routerlogger")]
    [Obsolete]
    public IActionResult Index()
    {
      string logMessage = null;
      using (StreamReader streamReader = new StreamReader(Request.Body, Encoding.ASCII))
      {
        logMessage = streamReader.ReadToEnd() + "\n";
      }
      string filePath = this._hostingEnvironment.ContentRootPath + "\\RouterLogger.txt";
      System.IO.File.AppendAllText(filePath, logMessage);
      return Ok();
    }

    //public IActionResult Index()
    //{
    //  return View();
    //}
  }
}
