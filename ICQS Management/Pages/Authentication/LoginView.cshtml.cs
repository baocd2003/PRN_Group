using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Plugins;
using Repository;
using Repository.Interface;

namespace ICQS_Management.Pages.Authentication
{
    public class LoginViewModel : PageModel
    {
        private IAuthRepository  _authRepository = new AuthRepository();
        public LoginDTO? LoginDTO { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost(LoginDTO loginDTO)  
        {
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
                return RedirectToPage("/Account_Staff/Details", new { id = user.UserId });
            }
            return RedirectToPage("/Account/Details", new { id = user.UserId });
        }
    }
}
