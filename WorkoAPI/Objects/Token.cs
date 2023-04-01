using Raven.Client.Documents.Session;

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

        public static bool verify(string userId,string token){
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                try
                {
                    Token dbToken = session.Query<Token>().Where(x => x.tokenSecret == token && x.userId == userId).First();
                    if (Double.Parse(dbToken.expiryUnix) < DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) { session.Delete(dbToken); session.SaveChanges(); ; return false; }
                
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
