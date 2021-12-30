using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerQAAPICORE.Identity;
using TaskManagerQAAPICORE.ServiceContracts;
using TaskManagerQAAPICORE.ViewModels;

namespace TaskManagerQAAPICORE.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [ApiController]
  public class AccountController : Controller
  {
   
    private readonly IUsersService _usersService;
    private readonly IAntiforgery _antiforgery;
    private readonly ApplicationSignInManager _applicationSignInManager;
    private readonly ApplicationDbContext db;
    private readonly ApplicationUserManager applicationUserManager;

    //public AccountController(IUsersService usersService, IAntiforgery antiforgery, ApplicationSignInManager applicationSignInManager, ApplicationDbContext db, ApplicationUserManager applicationUserManager)
    //{
    //  this._usersService = usersService;
    //  this._antiforgery = antiforgery;
    //  this._applicationSignInManager = applicationSignInManager;
    //  this.db = db;
    //  this.applicationUserManager = applicationUserManager;
    //}

    public AccountController(IUsersService usersService, ApplicationSignInManager applicationSignManager, IAntiforgery antiforgery, ApplicationDbContext db, ApplicationUserManager applicationUserManager)
    {
      this._usersService = usersService;
      this._applicationSignInManager = applicationSignManager;
      this._antiforgery = antiforgery;
      this.db = db;
      this.applicationUserManager = applicationUserManager;
    }

    [HttpPost]
    [Route("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginViewModel loginViewModel)
    {
      if (loginViewModel.Username != null || loginViewModel.Password != null)
      {
        var user = await _usersService.Authenticate(loginViewModel);
        if (user == null)
          return BadRequest(new { message = "Username or password is incorrect" });

        HttpContext.User = await _applicationSignInManager.CreateUserPrincipalAsync(user);
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
        Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

        return Ok(user);
      }
      else
      {
        return BadRequest(new { message = "Username or password is incorrect" });
      }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] SignUpViewModel signUpViewModel)
    {
      var user = await _usersService.Register(signUpViewModel);
      if (user == null)
        return BadRequest(new { message = "Invalid Data" });

      HttpContext.User = await _applicationSignInManager.CreateUserPrincipalAsync(user);
      var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
      Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
      Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

      return Ok(user);
    }

    [Route("api/getUserByEmail/{Email}")]
    public async Task<IActionResult> GetUserByEmail(string Email)
    {
      var user = await _usersService.GetUserByEmail(Email);
      return Ok(user);
    }

    [Route("api/getallemployees")]
    public async Task<IActionResult> GetAllEmployees()
    {
      List<ApplicationUser> users = this.db.Users.ToList();
      List<ApplicationUser> employeeUsers = new List<ApplicationUser>();

      foreach (var item in users)
      {
        if ((await this.applicationUserManager.IsInRoleAsync(item, "Employee")))
        {
          employeeUsers.Add(item);
        }
      }
      return Ok(employeeUsers);
    }


    //public IActionResult Index()
    //{
    //  return View();
    //}
  }
}
