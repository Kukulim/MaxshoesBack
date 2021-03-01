using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MaxshoesBack.Services.EmailService
{
    public class MailgunEmailService : IEmailService
    {
        public MailgunEmailService(HttpClient mailgunHttpClient, MailConfigSection mailConfigSection, ILogger<MailgunEmailService> logger)
        {
            this.mailgunHttpClient = mailgunHttpClient;
            this.mailConfigSection = mailConfigSection;
            this.logger = logger;
        }

        public HttpClient mailgunHttpClient { get; }
        public MailConfigSection mailConfigSection { get; }
        public ILogger<MailgunEmailService> logger { get; }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            Dictionary<string, string> form = new Dictionary<string, string>
            {
                ["from"] = mailConfigSection.From,
                ["to"] = toEmail,
                ["subject"] = subject,
                ["html"] = content,
            };

            HttpResponseMessage response = await mailgunHttpClient.PostAsync($"v3/{mailConfigSection.Domain}/messages", new FormUrlEncodedContent(form));

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error when trying to send mail. mailFrom: {mailFrom}, emailTo: {emailTo}, body: {body}, subject: {subject}, response: {@response}", mailConfigSection.From, toEmail, content, subject, response);
            }
        }
    }
}