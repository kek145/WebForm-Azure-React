using System;
using System.IO;
using Azure.Storage.Blobs;
using WebFormAPI.SendEmail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebFormAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<UploadController> _logger;

        public UploadController(ILogger<UploadController> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file, [FromForm] string email)
        {
            if (Path.GetExtension(file.FileName) != ".docx")
                return BadRequest();
            else
            {
                var filePath = Path.Combine("uploads", Path.GetFileName(file.FileName));

                using (var stream = System.IO.File.Create(filePath))
                    await file.CopyToAsync(stream);

                try
                {
                    var blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=cloudblobstorageaccount;AccountKey=SOYSK3WhVgqu03g9ycrCBhfY5cZZV2qCl+8y3UW8Q0boDH0EFyUULhTC5nOvPjZGaN6gO3xBNaXb+AStTYFGgA==;EndpointSuffix=core.windows.net";
                    var blobStorageContainerName = "samples-workitems";

                    var blobContainer = new BlobContainerClient(blobConnectionString, blobStorageContainerName);

                    var blob = blobContainer.GetBlobClient(filePath);

                    var stream = System.IO.File.OpenRead(filePath);
                    await blob.UploadAsync(stream);

                    _emailService.SendEmail(email, "Site administration", "Data successfully uploaded to the cloud Azure!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return Ok();
        }
    }
}
