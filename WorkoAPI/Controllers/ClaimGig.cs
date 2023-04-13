using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SelectGigAnswer : ControllerBase
    {

        //private readonly ILogger<SelectGigAnswer> _logger;

        /*public SelectGigAnswer(ILogger<SelectGigAnswer> logger)
        {
            _logger = logger;
        }*/

        [HttpPost(Name = "SelectGigAnswer")]
        public async Task<IActionResult> Post([FromForm]string userId, [FromForm]string token, [FromForm]string gigId, [FromForm]string solutionId)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //User authentication.
            if (!Token.verify(userId, token)) { return Unauthorized(); }

            //Finding the gig.
            Gig? gig = await session.Query<Gig>().Where(x => x.id == gigId && x.active == true).FirstAsync();
            if (gig == null) { return NotFound("Gig doesen't exist!"); }

            //Only the gig author should be able to do this.
            if (userId != gig.authorUserId) { return Unauthorized(); }

            //Find the solution to be selected as best.
            Solution? solution = await session.Query<Solution>().Where(x => x.id == solutionId).FirstAsync();
            if (solution == null) { return NotFound("Solution doesen't exist! Ironic..."); }

            //Deactivate the gig and select that solution.
            gig.active = false;
            solution.isBest = true;

            //Find the solver and cash out the points.
            User solver = await session.Query<User>().Where(x => x.Id == solution.authorId).FirstAsync();
            solver.balance += gig.rewardPoints;

            //Logging the transaction asynchronously.
            _ = Task.Run(() => Logger.logTransaction(gig.authorUserId, solver.Id, gig.rewardPoints));

            await session.SaveChangesAsync();
            return Ok();


        }       
    }
}