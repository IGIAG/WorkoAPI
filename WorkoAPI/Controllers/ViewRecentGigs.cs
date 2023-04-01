using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
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
        public async Task<IEnumerable<string>> Get([FromQuery]int n, [FromQuery]string tag)
        {
            using (IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                List<string> gigIds = new List<string>();
                List<Gig> gigs = new List<Gig>();

                //Format tags to array !!NOT USED AS RAVEN CANT HANDLE .Intersect() !!
                //IEnumerable<string> tagsEnumerable = tags.Split(' ');


                //Get N most recent gigs.(Use the "everything" tag to get all recent gigs)
                if (tag == "everything") { gigs = await session.Query<Gig>().Take(n).ToListAsync(); }
                else { gigs = await session.Query<Gig>().Where(x => x.tags.Contains(tag)).Take(n).ToListAsync(); }
                
                
                foreach (Gig g in gigs)
                {
                    //Format gigIds as IEnumerable
                    gigIds.Add(g.id);
                }
                session.SaveChangesAsync();
                return gigIds;
            }

            
        }       
    }
}