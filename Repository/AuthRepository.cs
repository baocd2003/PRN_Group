using BusinessObject.DTO;
using BussinessObject.Entity;
using DataAccessLayer.Service;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<User> Login(LoginDTO loginDTO)
        => await AuthenticationService.Instance.Login(loginDTO);

       
       
    }
}
