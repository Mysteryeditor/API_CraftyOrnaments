using Microsoft.EntityFrameworkCore;

namespace API_CraftyOrnaments.Models
{
    [Keyless]
    public class PasswordByte
    {
        public byte[]? encPassword { get; set; }
    }
}
