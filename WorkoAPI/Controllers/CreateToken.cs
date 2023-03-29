using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using System.Reflection.Metadata;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateToken : ControllerBase
    {

        private readonly ILogger<CreateToken> _logger;

        public CreateToken(ILogger<CreateToken> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateToken")]
        public IActionResult Post([FromForm] string login, [FromForm] string password)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                User user = session.Query<User>().Where(x => x.Name == login && x.Password == password).First();
                try
                {
                    Token oldToken = session.Query<Token>().Where(x => x.userId == user.Id).First();
                    session.Delete(oldToken);
                }
                catch
                {

                }
                Token token = new Token(SecretStringBuilder.getSecretString(48), user.Id, DateTime.UtcNow.AddDays(31).Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString());

                session.Store(token);
                session.SaveChanges();
                return Ok(token.tokenSecret);
            }

            
        }
    }
}