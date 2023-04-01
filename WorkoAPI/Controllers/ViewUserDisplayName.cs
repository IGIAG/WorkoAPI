using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
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
        public async Task<IActionResult> Get([FromQuery]string? id, [FromQuery] string? username)
        {
            using (IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                //Check if we are looking for username or id
                if (username == null)
                {
                    User? user = null;
                    //Try to find the user in DB
                    try { user = await session.Query<User>().Where(x => x.Id == id).FirstAsync(); }
                    catch { return NotFound(); }

                    await session.SaveChangesAsync();
                    return Ok(user.Name);
                }
                else if(id == null)
                {
                    //Try to find the user in DB
                    User? user = null;
                    try { user = await session.Query<User>().Where(x => x.Name == username).FirstAsync(); }
                    catch { return NotFound(); }

                    await session.SaveChangesAsync();
                    return Ok(user.Id);
                }
                return BadRequest();
            }
        }       
    }
}