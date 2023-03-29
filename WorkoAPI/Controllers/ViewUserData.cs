using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewUserData : ControllerBase
    {

        private readonly ILogger<ViewUserData> _logger;

        public ViewUserData(ILogger<ViewUserData> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "ViewUserData")]
        public IActionResult Post([FromForm]string login, [FromForm]string password)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                string returnString = "";
                User user = session.Query<User>().Where(x => x.Name == login && x.Password == password).First();
                returnString += user.Id;
                returnString += "\n" + user.Email;
                session.SaveChanges();
            }

            return Ok();
        }       
    }
}