using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Text.Json;
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
        public async Task<IActionResult> Get([FromQuery]string? id, [FromQuery] string? username)
        {
            using (IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                User? user = null;
                //Try to find the user in DB
                try { user = await session.Query<User>().Where(x => x.Id == id || x.Name == username).FirstAsync(); }
                catch { return NotFound(); }
                PublicUserData userData = new PublicUserData(user.Id, user.Name);
                return Ok(JsonSerializer.Serialize(userData));
            }
        }       
    }
}