using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateGig : ControllerBase
    {

        //private readonly ILogger<CreateGig> _logger;

        /*public CreateGig(ILogger<CreateGig> logger)
        {
            _logger = logger;
        }*/

        public class CreateGigForm
        {
            public string userId { get; set; }

            public string token { get; set; }

            public string title { get; set; }

            public string description { get; set; }

            public string tags { get; set; }

            public int reward { get; set; }

            public CreateGigForm(string userId, string token, string title, string description, string tags, int reward)
            {
                this.userId = userId;
                this.token = token;
                this.title = title;
                this.description = description;
                this.tags = tags;
                this.reward = reward;
            }
        }

        [HttpPost(Name = "CreateGig")]
        public async Task<IActionResult> Post(CreateGigForm form)
        {
            using IAsyncDocumentSession session = DocumentStoreHolder.Store.OpenAsyncSession();
            //User authentication.
            if (!Token.verify(form.userId, form.token)) { return Unauthorized(); }

            //Checking if the author has enough points
            User user = await session.Query<User>().Where(x => x.Id == form.userId).FirstOrDefaultAsync();
            if (user.balance < form.reward) { return BadRequest("User too poor"); }

            //Formatting the tags
            string[] tagsList = form.tags.Split(' ');


            //Moving the funds to a new gig
            user.balance -= form.reward;
            Gig gig = new(Guid.NewGuid().ToString(), form.title, form.description, form.userId, form.reward, tagsList, true);
            await session.StoreAsync(gig);

            await session.SaveChangesAsync();
            return Ok();


        }       
    }
}