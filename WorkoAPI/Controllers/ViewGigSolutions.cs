using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<string> Get([FromQuery]string gigId)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                
                Gig gig = session.Query<Gig>().Where(x => x.id == gigId).First();

                List<string> solutionIds = gig.solutions.ToList();
                session.SaveChanges();
                return solutionIds;
            }

            
        }       
    }
}