using System.Threading.Tasks;
using LKOStest.Dtos;
using LKOStest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Interfaces
{
    public interface IUserService
    {
        User GetUserBy(string userId);
        User CreateUser(UserRequest user);
        User Login(string username, string password);
    }
}