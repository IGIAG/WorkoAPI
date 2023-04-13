using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendMessage : ControllerBase
    {

        //private readonly ILogger<SendMessage> _logger;

        /*public SendMessage(ILogger<SendMessage> logger)
        {
            _logger = logger;
        }*/

        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> Post([FromForm] string userId, [FromForm] string token, [FromForm] string targetUserId, [FromForm] string title, [FromForm] string content)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //User authentication.
            if (!Token.verify(userId, token)) { return Unauthorized(); }

            await session.StoreAsync(new Message(userId, targetUserId, title, content, (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));

            await session.SaveChangesAsync();
            return Ok();


        }
    }
}