using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimGig : ControllerBase
    {

        private readonly ILogger<ClaimGig> _logger;

        public ClaimGig(ILogger<ClaimGig> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "ClaimGig")]
        public IActionResult Post([FromForm]string userId, [FromForm]string token, [FromForm]string gigId)
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
                
                Gig? gig = session.Query<Gig>().Where(x => x.id == gigId && x.active == true).First();
                if(gig == null) { return NotFound("Gig doesen't exist!"); }

                gig.active = false;
                user.balance += gig.rewardPoints;

                session.SaveChanges();
                return Ok();
            }

            
        }       
    }
}