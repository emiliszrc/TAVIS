using System.Threading.Tasks;
using LKOStest.Dtos;
using LKOStest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Interfaces
{
    public interface IUserService
    {
        User GetUserBy(int userId);
        User GetUserBy(string username);
        User CreateUser(UserRequest user);
    }
}