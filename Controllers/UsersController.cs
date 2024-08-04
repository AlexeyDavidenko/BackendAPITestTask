using Microsoft.AspNetCore.Mvc;
using BackendAPIDevelopmentTask.Data;
using BackendAPIDevelopmentTask.Models;
using SimpleAuthentication.JwtBearer;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using BackendAPIDevelopmentTask.ViewModels;
using System.Text;

namespace BackendAPIDevelopmentTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IJwtBearerService _jwtService;

        public UsersController(AppDBContext context, IJwtBearerService jwtBearerService)
        {
            _context = context;
            _jwtService = jwtBearerService;
        }

        // GET: api/Users
        [HttpPost("login")]
        [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [SwaggerOperation(description: "Insert permissions in the scope property (for example: 'profile people:admin')")]
        public ActionResult<LoginResponse> Login(LoginViewModel loginRequest, DateTime? expiration = null)
        {
            // Check for login rights...
            var dbUser = GetUser(loginRequest);
            if (dbUser != null)
            {
                //check password
                if (dbUser.Password == loginRequest.Password)
                {
                    // Add custom claims (optional).
                    var claims = new List<Claim>
                    {
                        new("roles", dbUser.Role.ToString())
                    };

                    var token = _jwtService.CreateToken(loginRequest.Username, claims, absoluteExpiration: expiration);
                    return new LoginResponse(token);
                }                
            }
            return BadRequest("Wrong Login or Password");            
        }

        [HttpPost("validate")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult<User> Validate(string token, bool validateLifetime = true)
        {
            if (_jwtService.TryValidateToken(token, validateLifetime, out var claimsPrincipal))
            {
                var user = _context.Users.FirstOrDefault(x => x.UserName == claimsPrincipal.Identity!.Name);
                if (user == null)
                {
                    return BadRequest("No such user in DB");
                }
                return user;
            }

            return BadRequest();
        }

        [HttpPost("refresh")]
        [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public ActionResult<LoginResponse> Refresh(string token, bool validateLifetime = true, DateTime? expiration = null)
        {
            var newToken = _jwtService.RefreshToken(token, validateLifetime, expiration);
            return new LoginResponse(newToken);
        }

        [HttpPost("register")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public ActionResult<string> Register(UserViewModel user)
        {
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                
                if (IsUserExists(user))
                {
                    message = $"User {user.Username} already exists";
                    return BadRequest(message);
                }

                try
                {
                    _context.Users.Add(new User
                    {
                        UserName = user.Username,
                        Password = user.Password,
                        Role = user.IsAdmin ? UserRole.Admin : UserRole.User

                    });
                    _context.SaveChanges();

                    message = $"User {user.Username} created";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return BadRequest(message);
                }

                return message;
            }
            else
            {
                return BadRequest();
            }

        }
        private bool IsUserExists(LoginViewModel userModel)
        {
            return GetUser(userModel) != null;
        }

        private User? GetUser(LoginViewModel userModel)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == userModel.Username);
        }
    }
}

public record class LoginResponse(string Token);
