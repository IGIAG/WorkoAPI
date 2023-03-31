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
        public IActionResult Post([FromForm]string userId, [FromForm]string token, [FromForm] string title, [FromForm]string description,[FromForm]string tags, [FromForm]int reward)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                if(!Token.verify(userId,token)){ return Unauthorized();}

                
                User user = session.Query<User>().Where(x => x.Id == userId).FirstOrDefault();
                if (user.balance < reward)
                {
                    return BadRequest("User too poor");
                }
                user.balance -= reward;
                IEnumerable<string> tagsEnumeralbe = tags.Split(' ');
                Gig gig = new Gig(Guid.NewGuid().ToString(), title, description, userId, reward,tagsEnumeralbe, true);
                session.Store(gig);
                session.SaveChanges();
                return Ok();
            }

            
        }       
    }
}