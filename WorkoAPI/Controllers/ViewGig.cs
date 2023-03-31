using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;
using System.Text.Json;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewGig : ControllerBase
    {

        private readonly ILogger<ViewGig> _logger;

        public ViewGig(ILogger<ViewGig> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "ViewGig")]
        public IActionResult Get([FromQuery]string id)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                Gig gig = session.Query<Gig>().Where(x => x.id == id).First();
                session.SaveChanges();
                return Ok(JsonSerializer.Serialize(gig));
            }

            
        }       
    }
}