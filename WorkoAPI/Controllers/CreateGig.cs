using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
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
        public async Task<IActionResult> Post([FromForm]string userId, [FromForm]string token, [FromForm] string title, [FromForm]string description,[FromForm]string tags, [FromForm]int reward)
        {
            using (IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                //User authentication.
                if(!Token.verify(userId,token)){ return Unauthorized();}

                //Checking if the author has enough points
                User user = await session.Query<User>().Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user.balance < reward) { return BadRequest("User too poor");}

                //Formatting the tags
                string[] tagsList = tags.Split(' ');


                //Moving the funds to a new gig
                user.balance -= reward;
                Gig gig = new Gig(Guid.NewGuid().ToString(), title, description, userId, reward,tagsList, true);
                await session.StoreAsync(gig);

                await session.SaveChangesAsync();
                return Ok();
            }

            
        }       
    }
}