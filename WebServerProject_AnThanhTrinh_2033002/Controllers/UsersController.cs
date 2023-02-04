using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServerProject_AnThanhTrinh_2033002.Data;
using WebServerProject_AnThanhTrinh_2033002.Models;

namespace WebServerProject_AnThanhTrinh_2033002.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpPost]
        public ActionResult Post(Dictionary<string, string> credentials)
        {
            User user;

            if(credentials.Count == 2)
            {
                user = _context.Users.FirstOrDefault(x => x.Email == credentials["email"] && x.Password == credentials["password"]);
                if (user == null)
                    return BadRequest("Invalid credentials.");

                var newSession = new Session()
                {
                    Email = user.Email
                };

                _context.Sessions.Add(newSession);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                return Ok("Token: " + newSession.Token);
            }
            else if(credentials.Count == 3)
            {
                user = _context.Users.FirstOrDefault(x => x.Email == credentials["email"]);
                if (user == null)
                {
                    user = new User()
                    {
                        Name = credentials["name"],
                        Email = credentials["email"],
                        Password = credentials["password"]
                    };


                    _context.Users.Add(user);
                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }

                    return Ok(user);
                }

                return BadRequest("User with the email: " + credentials["email"] + " has already existed");
            }


            return BadRequest("Bad Request");

        }

    }
}
