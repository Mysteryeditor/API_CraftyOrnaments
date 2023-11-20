using API_CraftyOrnaments.DAL.Contracts;
using API_CraftyOrnaments.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;

namespace API_CraftyOrnaments.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly CraftyOrnamentsContext _context;
        private readonly IAuthentication _auth;
        public AuthenticationController(CraftyOrnamentsContext context, IAuthentication auth)
        {
            _auth = auth;
            _context = context;

        }

        [HttpPost]
        public async Task<ActionResult<RegisterFormData>> UserRegister([FromBody] RegisterFormData NewUser)
        {
            if (NewUser is not null)
            {
                AccountProfile accountProfile = new()
                {
                    FirstName = NewUser.firstName,
                    LastName = NewUser.lastName,
                    Email = NewUser.email,
                    Dob = NewUser.DOB,
                    Gender = NewUser.gender,
                    PhoneNumber = NewUser.phoneNumber,
                    RoleId = 2,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    LastLoggedIn = null,
                    IsDeleted = false
                };

                try
                {
                    string storedProcedureQuery = "EXEC dbo.SP_PasswordEncrypt @password";
                    var Password = await _context.PasswordBytes.FromSqlRaw(storedProcedureQuery, new SqlParameter("password", NewUser.password)).ToListAsync();
                    accountProfile.Password = Password[0].encPassword;
                    _context.AccountProfiles.Add(accountProfile);
                    await _context.SaveChangesAsync();
                }

                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        var emailExist = new { text = "EmailExists" };
                        return Conflict(emailExist);
                    }
                    else
                    {
                        return BadRequest("An error occurred during the update.");
                    }
                }
                return NewUser;
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public async Task<ActionResult> AuthenticateCredentials(UserCredentials usercreds)
        {
            string storedProcedureQuery = "EXEC dbo.RoleDecider @email,@password";
            var role = await _context.AuthenticationDetails.FromSqlRaw(storedProcedureQuery, new SqlParameter("email", usercreds.email),
             new SqlParameter("password", usercreds.password)).ToListAsync();

            if (role is null)
            {
                return NotFound();
            }

            else
            {

                switch (role[0].userID)
                {
                    case -1:
                        return NotFound("The Username or password is incorrect");

                    default:
                        AccountProfile? loggedInUser = _context.AccountProfiles.Find(role[0].userID);
                        if (loggedInUser == null)
                        {
                            return NotFound();
                        }
                        loggedInUser.LastLoggedIn = DateTime.Now;

                        AccountProfilesController accountController = new(_context);

                        await accountController.PutAccountProfile(role[0].userID, loggedInUser);

                        var JsonData = new { userId = role[0].userID, role = role[0].Roles };
                        return new JsonResult(JsonData);
                }
            }
        }
        [HttpGet]
        public ResponseType<string> OtpGenerator(string email)
        {
            var response = _auth.SetOtp(email);
            var returnResponse = new ResponseType<string>();

            if (response is null)
            {
                return returnResponse = new ResponseType<string>
                {
                    StatusCode = 404,
                    Message = "Email Does not Exist"
                };
            }
            else
            {
                return response;
            }
        }

        [HttpPost]
        public ResponseType<string> OtpVerifier(OtpVerifyModel otp)
        {
            var returnResponse = new ResponseType<string>();
            ResponseType<string>? isValidResponse=_auth.VerifyOtp(otp);
            if(isValidResponse is null)
            {
                returnResponse = new ResponseType<string>
                {
                    StatusCode = 404,
                    Message = "Wrong Otp"
                };
                return returnResponse;
            }

            else if (returnResponse.StatusCode == 400)
            {
                return isValidResponse;
            }

            else
            {
                return isValidResponse;

            }

        }

        [HttpPost]
        public ResponseType<string> PasswordChange(UserCredentials newPassword)
        {
            ResponseType<string> response=_auth.ChangePassword(newPassword);
            if (response is null)
            {
                response = new ResponseType<string> 
                {
                    StatusCode=400,
                    Message="Changing the Password Failed"
                };
            }
                return response;
        }
    }
}
