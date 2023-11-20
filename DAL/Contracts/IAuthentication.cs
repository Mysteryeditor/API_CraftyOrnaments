using System.ComponentModel.DataAnnotations;

namespace API_CraftyOrnaments.DAL.Contracts
{
    public interface IAuthentication
    {
        ResponseType<string> SetOtp(string? email);

        ResponseType<string> VerifyOtp(OtpVerifyModel otp);

        ResponseType<string> ChangePassword(UserCredentials newPassword);
    }
}
