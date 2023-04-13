using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;
using HashLib;
using Raven.Client.Documents;
using System.Text.Json;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateUser : ControllerBase
    {

        //private readonly ILogger<CreateUser> _logger;

        /*public CreateUser(ILogger<CreateUser> logger)
        {
            _logger = logger;
        }*/

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Post([FromForm]string login, [FromForm]string password, [FromForm]string email)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //Check if user exists
            User? existant = null;
            try { existant = await session.Query<User>().Where(x => x.Name == login).FirstAsync(); } catch { }
            if (existant != null) { return Conflict(); }

            //Generate the password hash and create the user
            password = PasswordSecurity.hashMe(password);
            User user = new(Guid.NewGuid().ToString(), login, email, password);
            await session.StoreAsync(user);

            await session.SaveChangesAsync();
            return Ok(JsonSerializer.Serialize(new PublicUserData(user.Id, user.Name)));


        }       
    }
}