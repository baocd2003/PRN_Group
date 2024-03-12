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
                    HttpContext.Session.SetString("userRole", "admin");
                    return RedirectToPage("/AdminManagement/Admin_View/Index");
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
                HttpContext.Session.SetString("userId", user.UserId.ToString());
                HttpContext.Session.SetString("userRole", user.GetType().Name);
            }
            //role-base
            var role = user.GetType().Name;

            if (role.Equals("Staff"))
            {
                return RedirectToPage("/StaffManagement/Account_Staff/Details", new { id = user.UserId });
            }
            return RedirectToPage("/CustomerManagement/Account/Details", new { id = user.UserId });
        }
    }
}
