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
        public class CreateUserForm
        {
            public string login { get; set; }

            public string password { get; set; }

            public string email { get; set; }
        }


        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Post(CreateUserForm form)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //Check if user exists
            User? existant = null;
            try { existant = await session.Query<User>().Where(x => x.Name == form.login).FirstAsync(); } catch { }
            if (existant != null) { return Conflict(); }

            //Generate the password hash and create the user
            form.password = PasswordSecurity.hashMe(form.password);
            User user = new(Guid.NewGuid().ToString(), form.login, form.email, form.password);
            await session.StoreAsync(user);

            await session.SaveChangesAsync();
            return Ok(JsonSerializer.Serialize(new PublicUserData(user.Id, user.Name)));


        }       
    }
}