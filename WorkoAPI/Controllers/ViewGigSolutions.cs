using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewGigSolutions : ControllerBase
    {

        private readonly ILogger<ViewGigSolutions> _logger;

        public ViewGigSolutions(ILogger<ViewGigSolutions> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "ViewGigSolutions")]
        public async Task<IEnumerable<string>> Get([FromQuery]string gigId)
        {
            using (IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                Gig? gig = null;
                //Try to find the gig
                try { gig = await session.Query<Gig>().Where(x => x.id == gigId).FirstAsync(); }
                catch { return null; }

                List<string> solutionIds = gig.solutions.ToList();
                session.SaveChangesAsync();
                return solutionIds;
            }

            
        }       
    }
}