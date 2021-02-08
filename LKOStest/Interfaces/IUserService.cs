using System.Threading.Tasks;
using LKOStest.Dtos;
using LKOStest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Interfaces
{
    public interface IUserService
    {
        User GetUserBy(string userId);
        User GetUserByUsername(string username);
        User CreateUser(UserRequest user);
    }
}