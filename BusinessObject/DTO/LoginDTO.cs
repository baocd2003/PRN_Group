using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please enter your email.")]
        public string email { get; set; }
        [Required(ErrorMessage = "Please enter your password.")]
        public string password { get; set; }
    }
}
