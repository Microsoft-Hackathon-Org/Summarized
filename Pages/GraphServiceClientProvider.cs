using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace MyProject.Helpers
{
    public static class GraphServiceHelper
    {
        public static async Task SendEmail(GraphServiceClient graphServiceClient, string email, string subject, string body)
        {
            var message = new Message
            {
                ToRecipients = new List<Recipient>
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = email
                        }
                    }
                },
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = body
                }
            };

            await graphServiceClient.Me.SendMail(message, true).Request().PostAsync();
        }
    }
}
