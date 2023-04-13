using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;
using System.Text.Json;
using Raven.Client.Documents;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewGig : ControllerBase
    {

        //private readonly ILogger<ViewGig> _logger;

        /*public ViewGig(ILogger<ViewGig> logger)
        {
            _logger = logger;
        }*/

        [HttpGet(Name = "ViewGig")]
        public async Task<IActionResult> Get([FromQuery]string id)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //Try to find the gig
            Gig? gig = null;
            try { gig = await session.Query<Gig>().Where(x => x.id == id).FirstAsync(); }
            catch { return NotFound(); }

            _ = session.SaveChangesAsync();

            //Serialize gig as json
            return Ok(JsonSerializer.Serialize(gig));


        }       
    }
}