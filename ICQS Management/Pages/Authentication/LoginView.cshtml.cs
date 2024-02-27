using BusinessObject.DTO;
using BussinessObject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Plugins;
using Repository;
using Repository.Interface;

namespace ICQS_Management.Pages.Authentication
{
    public class LoginViewModel : PageModel
    {
        private IAuthRepository _authRepository = new AuthRepository();
        public LoginDTO? LoginDTO { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost(LoginDTO loginDTO)
        {
            var email = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AdminCredentials:Email").Value;
            var password = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AdminCredentials:Password").Value;
            if (loginDTO.email.Equals(email))
            {
                if (loginDTO.password.Equals(password))
                {
                    HttpContext.Session.SetString("LoggedEmail", loginDTO.email);
                    return RedirectToPage("/Admin_View/Index");
                }
            }
            var user = await _authRepository.Login(loginDTO);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return RedirectToPage();
            }
            else
            {
                HttpContext.Session.SetString("LoggedEmail", loginDTO.email);
            }
            //role-base
            var role = user.GetType().Name;

            if (role.Equals("Staff"))
            {
                return RedirectToPage("/Account_Staff/Index", new { id = user.UserId });
            }
            return RedirectToPage("/Account/Index", new { id = user.UserId });
        }
    }
}
