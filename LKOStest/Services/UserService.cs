using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Services
{
    public class UserService : IUserService
    {

        private TripContext tripContext;

        public UserService(TripContext tripContext)
        {
            this.tripContext = tripContext;
        }

        public User GetUserByUsername(string username)
        {
            var user = tripContext.Users
                .FirstOrDefault(u => u.Username == username);

            return user ?? throw new NotFoundException();
        }

        public User GetUserBy(string id)
        {
            var user = tripContext.Users
                .FirstOrDefault(u => u.Id == id);

            return user ?? throw new NotFoundException();
        }


        public User CreateUser(UserRequest user)
        {
            var userMapped = User.From(user);

            tripContext.Users.Add(userMapped);
            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create new user");
            }

            return GetUserBy(userMapped.Id);
        }
    }
}
