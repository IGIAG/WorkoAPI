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
        public class SelectGigAnswerForm
        {
            public string userId { get; set; }

            public string token { get; set; }

            public string gigId { get; set; }

            public string solutionId { get; set; }

            public SelectGigAnswerForm(string userId, string token, string gigId, string solutionId)
            {
                this.userId = userId;
                this.token = token;
                this.gigId = gigId;
                this.solutionId = solutionId;
            }
        }

        [HttpPost(Name = "SelectGigAnswer")]
        public async Task<IActionResult> Post(SelectGigAnswerForm form)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //User authentication.
            if (!Token.verify(form.userId, form.token)) { return Unauthorized(); }

            //Finding the gig.
            Gig? gig = await session.Query<Gig>().Where(x => x.id == form.gigId && x.active == true).FirstAsync();
            if (gig == null) { return NotFound("Gig doesen't exist!"); }

            //Only the gig author should be able to do this.
            if (form.userId != gig.authorUserId) { return Unauthorized(); }

            //Find the solution to be selected as best.
            Solution? solution = await session.Query<Solution>().Where(x => x.id == form.solutionId).FirstAsync();
            if (solution == null) { return NotFound("Solution doesen't exist! Ironic..."); }

            //Deactivate the gig and select that solution.
            gig.active = false;
            solution.isBest = true;

            //Find the solver and cash out the points.
            User solver = await session.Query<User>().Where(x => x.Id == solution.authorId).FirstAsync();
            solver.balance += gig.rewardPoints;

            //Logging the transaction asynchronously.
            _ = Task.Run(() => Logger.LogTransaction(gig.authorUserId, solver.Id, gig.rewardPoints));

            await session.SaveChangesAsync();
            return Ok();


        }       
    }
}