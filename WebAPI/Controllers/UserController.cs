using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using WebAPI.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private AppDbContext db;
        private IConfiguration configuration;

        private object ERR = new
        {
            status = "400",
            message = "bad request"
        };

        public UserController(AppDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var token = Request.Headers["Authorization"];
            token = token.ToString().Substring(7);

            return Ok(GetUserByToken(token));
        }

        [HttpPost("Login")]
        public IActionResult Login(User userLogin)
        {
            var user = AuthenticatedUser(userLogin);

            if (user != null)
            {
                var token = generateJwtToken(user);
                var ret = new
                {
                    status = 200,
                    user = user,
                    token = token
                };

                return Ok(ret);
            }

            else
                return BadRequest("Username/Email or password wrong!");
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (validateRegisterUserForm(user))
            {
                var existUser = from _user in db.User select _user;
                var existUsername = from n in existUser select n.Username;
                if (existUsername.Contains(user.Username))
                {
                    return BadRequest("Username/Password already exist");
                }

                var existEmail = from e in existUser select e.Email;
                if (existUsername.Contains(user.Username))
                {
                    return BadRequest("Username/Password already exist");
                }

                user.Created_at = DateTime.Now;
                user.Password = Hashing.HashPassword(user.Password);
                db.User.Add(user);
                db.SaveChanges();
                return Ok(user);
            }

            else
                return BadRequest("Username/Password already exist");
        }

        private bool validateRegisterUserForm(User user)
        {
            string[] userAttr =
                {
                "Username",
                "Email",
                "Password"
            };

            foreach(var attr in userAttr)
            {
                if (user.GetType().GetProperty(attr).GetValue(user, null) == null)
                    return false;
            }

            return true;
        }

        public User AuthenticatedUser(User user_input)
        {
            var user = from _user in db.User where _user.Email == user_input.Email || _user.Username == user_input.Username select _user;

            if (user.FirstOrDefault() != null)
            {
                if (Hashing.ValidatePassword(user_input.Password, user.First().Password))
                    return user.First();
            }

            return null;
        }

        private string generateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                // issuer: Configuration["Jwt:Issuer"],
                // audience: Configuration["Jwt:Audience"],
                null,
                null,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        public User GetUserByToken(string token)
        {
            var jwtSecrTokenHandler = new JwtSecurityTokenHandler();
            var secrToken = jwtSecrTokenHandler.ReadToken(token) as JwtSecurityToken;

            var userId = secrToken?.Claims.First(claim => claim.Type == "sub").Value;

            return db.User.Find(Guid.Parse(userId));
        }
    }
}