using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Xml.Linq;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SubmitGigSolution : ControllerBase
    {
        //private readonly ILogger<SubmitGigSolution> _logger;

        /*public SubmitGigSolution(ILogger<SubmitGigSolution> logger)
        {
            _logger = logger;
        }*/


        public class SubmitGigSolutionForm
        {
            public string userId { get; set; }

            public string token { get; set; }

            public string gigId { get; set; }

            public string content { get; set; }

        }

        [HttpPost(Name = "SubmitGigSolution")]
        public async Task<IActionResult> Post(SubmitGigSolutionForm form)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //User authentication
            if (!Token.verify(form.userId, form.token)) { return Unauthorized(); }

            //Get the user and gig
            User user = await session.Query<User>().Where(x => x.Id == form.userId).FirstOrDefaultAsync();
            Gig? gig = await session.Query<Gig>().Where(x => x.id == form.gigId && x.active == true).FirstAsync();

            //Check if gig exists
            if (gig == null) { return NotFound("Gig doesen't exist!"); }

            //Create the solution
            Solution solution = new(Guid.NewGuid().ToString(), user.Id, gig.id, form.content);

            //Add solution to gig
            gig.solutions = gig.solutions.Append(solution.id);
            _ = session.StoreAsync(solution);

            await session.StoreAsync(gig);

            await session.SaveChangesAsync();
            return Ok();


        }
    }
}
