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
        public async Task<Customer> CustomerRegister(Customer customer)
       => await AuthenticationService.Instance.CustomerRegister(customer);
        public User Block(User user)
        => AuthenticationService.Instance.Block(user);
        public Task<List<User>> GetAllPaging(int pageNumber, int pageSize)
        => AuthenticationService.Instance.GetAllPaging(pageNumber, pageSize);

        public Task<User> GetById(Guid id)
        => AuthenticationService.Instance.GetById(id);  
    }
}
