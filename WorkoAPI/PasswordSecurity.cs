using HashLib;

namespace WorkoAPI
{
    public static class PasswordSecurity
    {
        private static string frontAddStringFirst = "FuckOff";
        private static string endAddStringFirst = "LeaveOutPasswords";
        private static string frontAddStringLast = "dawdasdawdas";

        public static string hashMe(string password)
        {
            password = frontAddStringFirst + password + endAddStringFirst;
            password = HashFactory.Crypto.CreateSHA512().ComputeChars((frontAddStringLast + password).ToCharArray()).ToString();
            password = HashFactory.Crypto.CreateHAS160().ComputeChars(password.ToCharArray()).ToString();
            return password;
        }
    }
}
