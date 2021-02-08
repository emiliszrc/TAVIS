using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        private IConfiguration configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetByUsername([FromQuery] string username)
        {
            try
            {
                return Ok(userService.GetUserByUsername(username));
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{username}")]
        public IActionResult GetById(string id)
        {
            try
            {
                return Ok(userService.GetUserBy(id));
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserRequest userRequest)
        {
            try
            {
                var user = userService.CreateUser(userRequest);

                if (user == null)
                {
                    return StatusCode(500);
                }

                return Created($"{configuration.GetSection("Hostname").Value}/user/{user.Id}", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}
