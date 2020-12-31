using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return tripContext.Users
                .FirstOrDefault(user => user.Id == userId.ToString());
        }

        public User GetUserBy(string username)
        {
            return tripContext.Users
                .FirstOrDefault(user => user.Username == username);
        }

        public User CreateUser(UserRequest user)
        {
            var userMapped = User.From(user);

            tripContext.Users.Add(userMapped);
            tripContext.Organisations.FirstOrDefault(organisation => organisation.Id == user.OrganisationId).Users.Add(userMapped);
            tripContext.SaveChanges();
            return User.From(user);
        }

        public User Login(string username, string password)
        {
            var user = GetUserBy(username);
            if (user != null && user.Password == password && user.Username == username)
            {
                return user;
            }

            return null;
        }
    }
}
