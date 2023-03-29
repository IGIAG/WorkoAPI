using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateUser : ControllerBase
    {

        private readonly ILogger<CreateUser> _logger;

        public CreateUser(ILogger<CreateUser> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateUser")]
        public IActionResult Post([FromForm]string login, [FromForm]string password, [FromForm]string email)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                User user = new User(Guid.NewGuid().ToString(),login,email,password);
                session.Store(user);
                session.SaveChanges();
            }

            return Ok();
        }       
    }
}