using API_CraftyOrnaments.DAL.Contracts;
using API_CraftyOrnaments.Models;
using API_CraftyOrnaments.Shared;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using YourApiNamespace.Controllers;

namespace API_CraftyOrnaments.DAL.Repository
{
    public class Authentication : IAuthentication
    {
        private readonly CraftyOrnamentsContext _context;
        public Authentication(CraftyOrnamentsContext context)
        {
            _context = context;
        }
        public ResponseType<string>? SetOtp(string? email)
        {
            AccountProfile? profile = _context.AccountProfiles.FirstOrDefault(p => p.Email == email);
            if (profile is null)
            {
                var response = new ResponseType<string>
                {
                    StatusCode = 404,
                    Message = "Email Not Found",
                };

                return null;
            }
            else
            {
                string otpGenerated = OtpGenerator.GenerateOtp(6);
                Otp newOtpRecord = new Otp
                {
                    Email = email,
                    OtpValue = otpGenerated,
                    CreatedDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddMinutes(5),
                    UserId = profile.UserId
                };
                _context.Otps.Add(newOtpRecord);
                _context.SaveChanges();
                var response = new ResponseType<string>
                {
                    StatusCode = 200,
                    Message = "OTP generated Successfully"
                };

                EmailController sendEmail = new();
                EmailModel emailModel = new EmailModel
                {
                    body = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <title>Forgot Password - Crafty Ornaments</title>
</head>
<body>
    <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
        <tr>
            <td align=""center"" bgcolor=""#f5f5f5"" style=""padding: 40px 0 30px 0;"">
                <h1 style=""color: #333333;"">Crafty Ornaments</h1>
            </td>
        </tr>
        <tr>
            <td bgcolor=""#ffffff"" style=""padding: 40px 30px 40px 30px;"">
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                    <tr>
                        <td>
                            <p>Dear User,</p>
                            <p>You have requested a password reset for your Crafty Ornaments account.</p>
                            <p><strong>Your OTP: " + newOtpRecord.OtpValue + @"</strong></p>
                            <p>If you did not request this password reset, please ignore this email.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td bgcolor=""#f5f5f5"" style=""padding: 20px 0;"">
                <p style=""text-align: center;"">&copy; 2023 Crafty Ornaments</p>
            </td>
        </tr>
    </table>
</body>
</html>
",
                    to = newOtpRecord.Email,
                    subject="Forgot Password"
            };
                try
                {
                    sendEmail.SendEmailAsync(emailModel);
                    return response;
                }

                catch (Exception ex)
                {
                    response = new ResponseType<string>
                    {
                        StatusCode = 500,
                        Message = ex.Message,

                    };
                    return response ;

                }

        }
    }
    ResponseType<string>? IAuthentication.VerifyOtp(OtpVerifyModel otp)
    {
        ResponseType<string> response = new();
        Otp? verifiedOtpRecord = _context.Otps.FirstOrDefault(p => p.Email == otp.email && p.OtpValue == otp.otp.ToString());
        if (verifiedOtpRecord is null)
        {
            return null;

        }

        else if (verifiedOtpRecord.ExpiryDate < DateTime.Now)
        {
            response = new ResponseType<string>
            {
                StatusCode = 400,
                Message = "Invalid or Expired Otp"
            };
        }

        else
        {

            response = new ResponseType<string>
            {
                StatusCode = 200,
                Message = "Verified Successfully",
                Data = otp.email
            };

        }
        _context.Remove(verifiedOtpRecord);
        _context.SaveChanges();
        return response;
    }

    ResponseType<string>? IAuthentication.ChangePassword(UserCredentials newPassword)
        {
        ResponseType<string> response = new ResponseType<string>
        {
            StatusCode = 200,
            Message = "Password Changed Successfully"
        };
        AccountProfile? profile = _context.AccountProfiles.FirstOrDefault(user => user.Email == newPassword.email);
        if (profile is not null)
        {

            string storedProcedureQuery = "EXEC dbo.SP_PasswordEncrypt @password";
            var Password = _context.PasswordBytes.FromSqlRaw(storedProcedureQuery, new SqlParameter("password", newPassword.password)).ToList();
            profile.Password = Password[0].encPassword;
            _context.Entry(profile).State = EntityState.Modified;
            _context.SaveChanges();
            return response;

        }
        else
        {
            return null;
        }
    }
}
}
