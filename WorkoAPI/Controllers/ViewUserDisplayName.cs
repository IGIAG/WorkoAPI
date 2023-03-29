using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewUserDisplayName : ControllerBase
    {

        private readonly ILogger<ViewUserDisplayName> _logger;

        public ViewUserDisplayName(ILogger<ViewUserDisplayName> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "ViewUserDisplayName")]
        public IActionResult Get([FromQuery]string? id, [FromQuery] string? username)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                if (username == null)
                {
                    User user = session.Query<User>().Where(x => x.Id == id).First();
                    session.SaveChanges();
                    return Ok(user.Name);
                }
                else if(id == null)
                {
                    User user = session.Query<User>().Where(x => x.Name == username).First();
                    session.SaveChanges();
                    return Ok(user.Id);
                }
                return BadRequest();
            }
        }       
    }
}