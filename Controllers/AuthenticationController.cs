using API_CraftyOrnaments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace API_CraftyOrnaments.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Authorize(Roles="Admin")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly CraftyOrnamentsContext _context;

        public AuthenticationController(CraftyOrnamentsContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<RoleDeciderResult>>> AuthenticateCredentials(string email, string password)
        {

            string storedProcedureQuery = "EXEC dbo.RoleDecider @email,@password";
            var role = await _context.AuthenticationDetails.FromSqlRaw(storedProcedureQuery, new SqlParameter("email", email),
             new SqlParameter("password", password)).ToListAsync();

            if (role is null)
            {
                return NotFound();
            }

            else
            {
                //implement something to store in .net's understanding
                return Ok(role);
                //return role;
            }


        }
        [HttpPost]
        public async Task<ActionResult<RegisterFormData>> UserRegister([FromBody] RegisterFormData NewUser)
        {
            if (NewUser is not null)
            {
                AccountProfile accountProfile = new AccountProfile();
                accountProfile.FirstName = NewUser.firstName;
                accountProfile.LastName = NewUser.lastName;
                accountProfile.Email = NewUser.email;
                accountProfile.Dob = NewUser.DOB;
                accountProfile.Gender = NewUser.gender;
                accountProfile.PhoneNumber = NewUser.phoneNumber;
                accountProfile.RoleId = 2;
                accountProfile.CreatedDate = DateTime.Now;
                accountProfile.ModifiedDate = DateTime.Now;
                accountProfile.LastLoggedIn = null;
                accountProfile.IsDeleted = false;

                try
                {
                    string storedProcedureQuery = "EXEC dbo.SP_PasswordEncrypt @password";
                    var Password = await _context.PasswordBytes.FromSqlRaw(storedProcedureQuery, new SqlParameter("password", NewUser.password)).ToListAsync();
                    accountProfile.Password = Password[0].encPassword;
                    _context.AccountProfiles.Add(accountProfile);
                    await _context.SaveChangesAsync();
                }

                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                    //return StatusCode(ex.st)
                }



                //if(NewUser.profilePic is not null)
                //{
                //    //try
                //    //{
                //    //    byte[] profilePic = Convert.FromBase64String(NewUser.profilePic);

                //    //}
                //    //catch(Exception ex)
                //    //{
                //    //    return BadRequest(ex.Message);
                //    //}

                //}
                //accountProfile.ProfilePic = Convert.FromBase64String(NewUser.profilePic);


                return NewUser;
            }
            else
            {
                return BadRequest();
            }

        }


    }
}
