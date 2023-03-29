namespace WorkoAPI.Objects
{
    public class Token
    {
        public string tokenSecret;
        public string userId;
        public string expiryUnix;

        public Token(string tokenSecret, string userId, string expiryUnix)
        {
            this.tokenSecret = tokenSecret;
            this.userId = userId;
            this.expiryUnix = expiryUnix;
        }
    }
}
