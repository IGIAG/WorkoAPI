namespace WorkoAPI
{
    public static class SecretStringBuilder
    {

        private static string chars = "abcdefghijklmnopqrstuwxyzABCDEFGHIJKLMNOPQRSTUWVXYZ1234567890!@#$%^&*()_+~`=-<>:/{}|";
        public static string getSecretString(int length)
        {
            string returnVal = "";
            for(int i = 0; i < length; i++)
            {
                returnVal += chars[Random.Shared.Next(chars.Length)];
            }
            return returnVal;

        }
    }
}
