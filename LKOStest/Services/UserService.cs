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

        public User GetUserBy(int userId)
        {
            var user = tripContext.Users
                .FirstOrDefault(user => user.Id == userId.ToString());

            if (user == null)
            {
                throw new NotFoundException();
            }

            return user;
        }


        public User GetUserBy(string username)
        {
            var user = tripContext.Users
                .FirstOrDefault(user => user.Username == username);

            if (user == null)
            {
                throw new NotFoundException();
            }

            return user;
        }


        public User CreateUser(UserRequest user)
        {
            var userMapped = User.From(user);

            tripContext.Users.Add(userMapped);
            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create new user");
            }

            return GetUserBy(int.Parse(userMapped.Id));
        }
    }
}
