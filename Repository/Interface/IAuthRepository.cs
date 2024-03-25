using BusinessObject.DTO;
using BussinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IAuthRepository
    {
        public Task<User> Login(LoginDTO loginDTO);
        public Task<Customer> CustomerRegister(Customer customer);
        public User Block(User user);
        public Task<List<User>> GetAllPaging(int pageNumber, int pageSize);
        public Task<User> GetById(Guid id);


    }
}
