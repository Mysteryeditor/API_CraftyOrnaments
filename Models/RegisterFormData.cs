namespace API_CraftyOrnaments.Models
{
    public class RegisterFormData
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }

    
        public string? email { get; set; }
    
        public string? password { get; set; }

        public string? confirmPassword { get; set; }

        public DateTime? DOB { get; set; }

        public string? gender { get; set; }

        public long phoneNumber { get; set; }



    }
}
