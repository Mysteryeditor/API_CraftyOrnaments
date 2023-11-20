namespace API_CraftyOrnaments.Shared
{
    public class OtpGenerator
    {
        private static Random random = new Random();

        public static string GenerateOtp(int length)
        {
            int min = (int)Math.Pow(10, length - 1);
            int max = (int)Math.Pow(10, length) - 1;

            int otp = random.Next(min, max + 1);

            return otp.ToString().PadLeft(length, '0');
        }
    }

}
