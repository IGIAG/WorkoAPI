using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using System.Collections;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewRecentGigs : ControllerBase
    {

        private readonly ILogger<ViewRecentGigs> _logger;

        public ViewRecentGigs(ILogger<ViewRecentGigs> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "ViewRecentGigs")]
        public IEnumerable<string> Get([FromQuery]int n)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                List<string> gigIds = new List<string>();
                List<Gig> gigs = new List<Gig>();
                gigs = session.Query<Gig>().Take(n).ToList();
                foreach (Gig g in gigs)
                {
                    gigIds.Add(g.id);
                }
                session.SaveChanges();
                return gigIds;
            }

            
        }       
    }
}