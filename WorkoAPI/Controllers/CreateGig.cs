using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateGig : ControllerBase
    {

        private readonly ILogger<CreateGig> _logger;

        public CreateGig(ILogger<CreateGig> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateGig")]
        public IActionResult Post([FromForm]string userId, [FromForm]string token, [FromForm] string title, [FromForm]string description, [FromForm]int reward)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                try
                {
                    Token dbToken = session.Query<Token>().Where(x => x.tokenSecret == token && x.userId == userId).First();
                    if (Double.Parse(dbToken.expiryUnix) < DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) { session.Delete(dbToken); session.SaveChanges(); ; return Unauthorized(); }
                }
                catch
                {
                    return Unauthorized();
                }
                User user = session.Query<User>().Where(x => x.Id == userId).FirstOrDefault();
                if (user.balance < reward)
                {
                    return BadRequest("User too poor");
                }
                user.balance -= reward;
                Gig gig = new Gig(Guid.NewGuid().ToString(), title, description, userId, reward, true);
                session.Store(gig);
                session.SaveChanges();
                return Ok();
            }

            
        }       
    }
}