using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerQAAPICORE.Identity;
using TaskManagerQAAPICORE.ViewModels;

namespace TaskManagerQAAPICORE.ServiceContracts
{
  public interface IUsersService
  {
    Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel);
    Task<ApplicationUser> Register(SignUpViewModel signUpViewModel);
    Task<ApplicationUser> GetUserByEmail(string Email);
  }
}
