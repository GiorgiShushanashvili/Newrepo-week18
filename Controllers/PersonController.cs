using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mixed.Models;
using Mixed.Services;
using Mixed.Validators;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mixed.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly IPersonInterface _persservice;
        private readonly Appsettings _appsettings;

        public PersonController(IPersonInterface iperservice,
            IOptions<Appsettings> appsettings)
        {
            _persservice = iperservice;
            _appsettings = appsettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginUser model)
        {
            var user = _persservice.Login(model);
            if (user == null)
                return BadRequest("username or password is incorrect");
            string tokenstring = GenerateToken(user);
            return Ok(new
            {
                user.Username,
                user.Role,
                Token = tokenstring

            });
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("adduser")]
        public ActionResult<Person> AddUser([FromBody] Person person)
        {
            var validator = new Personvalidator();
            var result = validator.Validate(person);
            List<string> errorlist = new();
            if (!result.IsValid)
            {
                foreach(var error in result.Errors)
                {
                    errorlist.Add(error.ErrorMessage);
                }
                return BadRequest(errorlist);
            }
            if (!User.IsInRole(Role.Admin))
                return Forbid();
            person.Password = Passwordhasher.hashPass(person.Password);
            if (_persservice.Adduser(person) == null)
                return BadRequest("already registered");
            try
            {
                _persservice.Adduser(person);
            }
            catch (DbUpdateException e)
            {
                return BadRequest($"{e.InnerException.Message}.\nLeaveThe Id Fields Empty");
            }
            return Created("",person);
                        
        }

        [HttpGet("getall")]
        public ActionResult<IEnumerable<Person>> GetAllPersons()
        {
            var persons = _persservice.GetAll();
            if (!persons.Any())
            {
                return NotFound("there no persons");
            }
            else
            {
                return persons;
            }
        }

        [HttpGet("filter")]
        public IActionResult FilterPerson([FromQuery]string City, double Salary)
        {
            var person = _persservice.Filter(City, Salary);
            if (person.Count == 0)
            { return NotFound("there no such person");}
            else
            {
                return Ok(person);
            }

        }
        [HttpGet("get/{id}")]
        public IActionResult GetParticipantById(int id)
        {
            var person = _persservice.Find(id);
            if(person == null)
            {
                return NotFound($"there is no person with id{id}");
            }
            else
            {
                return Ok(person);
            }
        }
        [Authorize(Roles =Role.Admin)]
        [HttpDelete("delete/{id}")]
        public ActionResult PersonDeleteById(int id)
        {
            if (!User.IsInRole(Role.Admin))
                return Forbid();
            try
            {
                _persservice.Delete(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound($"there is no participant with id{id}");
            }
            return Ok($"participant with id {id} is deleted");
            
        }

        [HttpPut("update/{PersonId}")]
        public IActionResult UpdatePersonById(Person person)
        {
            var validator = new Personvalidator();
            var result = validator.Validate(person);
            List<string> errorlist = new();
            if (!result.IsValid)
            {
               foreach(var error in result.Errors)
                {
                    errorlist.Add(error.ErrorMessage);
                }
                return BadRequest(errorlist);
            }
            person.PersonAddress.id = person.PersonId;
            try
            {
                _persservice.Update(person);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound($"there is no person with id{person.PersonAddress.id}");
            }
            return Ok(person);
        }
        

        private string GenerateToken(Person person)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, person.Username.ToString()),
                    new Claim(ClaimTypes.Role, person.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

    }

}

