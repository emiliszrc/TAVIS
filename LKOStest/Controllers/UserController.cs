using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("{userId}")]
        public User Get(string userId)
        {
            return userService.GetUserBy(userId);
        }

        [HttpPost]
        public void Post([FromBody] UserRequest user)
        {
            userService.CreateUser(user);
        }

        [HttpGet]
        public User Login(string username, string password)
        {
            return userService.Login(username, password);
        }
    }
}
