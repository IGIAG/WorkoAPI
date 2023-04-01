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
        private readonly ILogger<SubmitGigSolution> _logger;

        public SubmitGigSolution(ILogger<SubmitGigSolution> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "SubmitGigSolution")]
        public async Task<IActionResult> Post([FromForm] string userId, [FromForm] string token, [FromForm] string gigId, [FromForm]string content)
        {
            using (IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                //User authentication
                if(!Token.verify(userId,token)){ return Unauthorized();}

                //Get the user and gig
                User user = await session.Query<User>().Where(x => x.Id == userId).FirstOrDefaultAsync();
                Gig? gig = await session.Query<Gig>().Where(x => x.id == gigId && x.active == true).FirstAsync();

                //Check if gig exists
                if (gig == null) { return NotFound("Gig doesen't exist!"); }

                //Create the solution
                Solution solution = new Solution(Guid.NewGuid().ToString(), user.Id, gig.id, content);

                //Add solution to gig
                gig.solutions = gig.solutions.Append(solution.id);
                session.StoreAsync(solution);
                
                await session.StoreAsync(gig);

                await session.SaveChangesAsync();
                return Ok();
            }


        }
    }
}
