using System;
using SendGrid;
using System.IO;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SendEmailTriggerFunc
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([BlobTrigger("samples-workitems/{name}", Connection = "DefaultEndpointsProtocol=https;AccountName=cloudblobstorageaccount;AccountKey=SOYSK3WhVgqu03g9ycrCBhfY5cZZV2qCl+8y3UW8Q0boDH0EFyUULhTC5nOvPjZGaN6gO3xBNaXb+AStTYFGgA==;EndpointSuffix=core.windows.net")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob | Name:{name} | Size: {myBlob.Length} Bytes");

            try
            {
                var client = new SendGridClient("SENDGRID_API_KEY");
                var from = new EmailAddress("EMAIL FROM SEND", "Site administration");
                var subject = "Blob notification";
                var to = new EmailAddress("EMAIL TO REPLY", "Recipient Name");
                var body = $"Data successfully uploaded to the cloud Azure!";
                var message = MailHelper.CreateSingleEmail(from, to, subject, body, "");
                var response = client.SendEmailAsync(message).Result;

                log.LogInformation($"Sent email notification. Status code: {response.StatusCode}");
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}
