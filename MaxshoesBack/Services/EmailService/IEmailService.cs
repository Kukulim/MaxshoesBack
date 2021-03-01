using System.Threading.Tasks;

namespace MaxshoesBack.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
}