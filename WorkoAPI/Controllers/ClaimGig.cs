using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SelectGigAnswer : ControllerBase
    {

        private readonly ILogger<SelectGigAnswer> _logger;

        public SelectGigAnswer(ILogger<SelectGigAnswer> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "SelectGigAnswer")]
        public async Task<IActionResult> Post([FromForm]string userId, [FromForm]string token, [FromForm]string gigId, [FromForm]string solutionId)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                if(!Token.verify(userId,token)){ return Unauthorized();}
                
                
                Gig? gig = session.Query<Gig>().Where(x => x.id == gigId && x.active == true).First();
                if(gig == null) { return NotFound("Gig doesen't exist!"); }
                Solution? solution = session.Query<Solution>().Where(x => x.id == solutionId).First();
                if (solution == null) { return NotFound("Solution doesen't exist! Ironic..."); }
                gig.active = false;
                solution.isBest = true;

                User solver = session.Query<User>().Where(x => x.Id == solution.authorId).First();

                solver.balance += gig.rewardPoints;
                Logger.logTransaction(gig.authorUserId,solver.Id,gig.rewardPoints);

                session.SaveChanges();
                return Ok();
            }

            
        }       
    }
}