﻿using BusinessObject.DTO;
using BussinessObject.Entity;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var customer = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(loginDTO.email));
                if (customer != null)
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
        public async Task<Customer> CustomerRegister(Customer customer)
        {
            try
            {
                customer.Id = Guid.NewGuid();
                var customerParse = new Customer
                {
                    Id = customer.Id,
                    Email = customer.Email,
                    Name = customer.Name,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    UserId = customer.Id
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
    }
}
