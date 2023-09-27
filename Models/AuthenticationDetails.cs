using Microsoft.EntityFrameworkCore;

namespace API_CraftyOrnaments.Models
{
    [Keyless]
    public class AuthenticationDetails
    {

        public int userID { get; set; }

        public string? Roles { get; set; }
    }
}
