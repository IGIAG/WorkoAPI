﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Post([FromForm] string userId, [FromForm] string token, [FromForm] string gigId, [FromForm]string content)
        {
            using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
            {
                try
                {
                    Token dbToken = session.Query<Token>().Where(x => x.tokenSecret == token && x.userId == userId).First();
                    if (Double.Parse(dbToken.expiryUnix) < DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds) { session.Delete(dbToken); session.SaveChanges(); ; return Unauthorized(); }
                }
                catch
                {
                    return Unauthorized();
                }

                User user = session.Query<User>().Where(x => x.Id == userId).FirstOrDefault();
                Gig? gig = session.Query<Gig>().Where(x => x.id == gigId && x.active == true).First();
                if (gig == null) { return NotFound("Gig doesen't exist!"); }

                Solution solution = new Solution(Guid.NewGuid().ToString(), user.Id, gig.id, content);

                gig.solutions = gig.solutions.Append(solution.id);
                session.Store(gig);
                session.Store(solution);

                session.SaveChanges();
                return Ok();
            }


        }
    }
}