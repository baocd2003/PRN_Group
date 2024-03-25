using BusinessObject.DTO;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Service
{
    public class AuthenticationService
    {
        private static AuthenticationService instance;
        public static readonly object instanceLock = new object();
        private readonly applicationDbContext _db = new applicationDbContext();
        public AuthenticationService()
        {

        }
        public static AuthenticationService Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance is null)
                    {
                        instance = new AuthenticationService();
                    }
                    return instance;
                }
            }
        }
        public async Task<User> Login(LoginDTO loginDTO)
        {
            using (applicationDbContext _dbnew = new applicationDbContext())
            {
                try
                {
                    var customer = await _dbnew.Users.FirstOrDefaultAsync(x => x.Email.Equals(loginDTO.email));
                    if (customer != null && customer.status.Equals("1"))
                    {
                        if (customer.Password.Equals(loginDTO.password))
                        {
                            return customer;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
                
        }
        
        public async Task<Customer> CustomerRegister(Customer customer)
        {
            try
            {
                customer.CustomerId = Guid.NewGuid();
                var customerParse = new Customer
                {
                    CustomerId = customer.CustomerId,
                    Email = customer.Email,
                    Name = customer.Name,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    UserId = customer.CustomerId,
                    status = "1"
                };
                await _db.Users.AddAsync(customerParse);
                await _db.SaveChangesAsync();
                return customerParse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public User Block(User user)
        {
            using(applicationDbContext _dbnew = new applicationDbContext())
            {
                var acc = _dbnew.Users.FirstOrDefault(x=>x.UserId == user.UserId);
                if (user != null)
                {
                    if (acc.status.Equals("1"))
                    {
                        acc.status = "0";
                    }
                    else
                    {
                        acc.status = "1";
                    }

                }
                _dbnew.SaveChanges();
                return acc;

            }
           
        }
        public async Task<List<User>> GetAllPaging(int pageNumber, int pageSize)
        {
            using (applicationDbContext _db = new applicationDbContext())
            {              
                return _db.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        public async Task<User> GetById(Guid id)
        {
            using (applicationDbContext _db = new applicationDbContext())
            {
                return await _db.Users.FirstOrDefaultAsync(x => x.UserId.Equals(id));
            }
        }

    }
}
