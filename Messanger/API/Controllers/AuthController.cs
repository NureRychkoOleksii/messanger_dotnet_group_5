using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using BLL.Abstractions.Interfaces;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDTO credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Name) || string.IsNullOrWhiteSpace(credentials.Password))
            {
                return BadRequest("Invalid values");
            }

            var userExists = await _userService.UserExists((user) =>
                user.Nickname == credentials.Name && user.Password == credentials.Password);

            if (!userExists)
            {
                return BadRequest("There is no such a user");
            }
            var user = await _userService.GetUser(user =>
                    user.Nickname == credentials.Name && user.Password == credentials.Password);
                return Ok(user);
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterDTO credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Name) || string.IsNullOrWhiteSpace(credentials.Password) || string.IsNullOrWhiteSpace(credentials.Email))
            {
                return BadRequest("Invalid values");
            }

            var userExists = await _userService.UserExists((user) =>
                user.Nickname == credentials.Name && user.Password == credentials.Password);

            if (userExists)
            {
                return BadRequest("Your account is already registered");
            }
            await _userService.CreateUser(new User(){Email = credentials.Email, Nickname = credentials.Name, Password = credentials.Password});
            return Ok("Successful!");
        }
    }
}
