using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Raven.Client.Documents.Session;
using System.Xml.Linq;
using WorkoAPI.Objects;

namespace WorkoAPI.Controllers
{

    [ApiController]
    [DisableRequestSizeLimit]
    [Route("[controller]")]
    public class UploadAttachment : ControllerBase
    {
        private readonly ILogger<UploadAttachment> _logger;

        public UploadAttachment(ILogger<UploadAttachment> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "UploadAttachment")]
        public IActionResult Post([FromForm] string userId, [FromForm] string token, IFormFile file)
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
                string mimeType = "unknown";
                string base64 = "missing";
                new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out mimeType);
                //Console.WriteLine(mimeType);
                using(var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(fileBytes);
                    base64 = base64String;
                }
                if(mimeType == "unknown" || base64 == "missing") { return BadRequest();}
                Attachment attachment = new Attachment(Guid.NewGuid().ToString(),mimeType,base64);
                session.Store(attachment);

                session.SaveChanges();
                return Ok();
            }


        }
    }
}
