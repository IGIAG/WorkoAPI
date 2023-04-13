using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Reflection.Metadata;
using WorkoAPI.Objects;
using System.Text.Json;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateToken : ControllerBase
    {

        //private readonly ILogger<CreateToken> _logger;

        /*public CreateToken(ILogger<CreateToken> logger)
        {
            _logger = logger;
        }*/

        [HttpPost(Name = "CreateToken")]
        public async Task<IActionResult> Post([FromForm] string login, [FromForm] string password)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //Hashing the password
            password = PasswordSecurity.hashMe(password);

            //Check if user exists with that username and password hash.
            User? user = null;
            try { user = await session.Query<User>().Where(x => x.Name == login && x.Password == password).FirstAsync(); }
            catch { return Unauthorized(); }

            //Removing the old token
            try { Token oldToken = await session.Query<Token>().Where(x => x.userId == user.Id).FirstAsync(); session.Delete(oldToken); } catch { }

            //Generating the new token
            Token token = new(SecretStringBuilder.GetSecretString(48), user.Id, DateTime.UtcNow.AddDays(31).Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString());

            await session.StoreAsync(token);
            await session.SaveChangesAsync();
            //WARNING! IF YOU THINK YOUR TOKEN IS MESSED UP, IT'S VERY LIKELY THAT IT IS NOT. TRY UN-ESCAPING THE JSON(Add back the special chars).
            return Ok(JsonSerializer.Serialize(token));


        }
    }
}