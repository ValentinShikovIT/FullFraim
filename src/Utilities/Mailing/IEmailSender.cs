using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Mailing
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string Sender,
            string SenderName,
            string To,
            string Subject,
            string HtmlContent,
            IEnumerable<EmailAttachment> Attachments = null);
    }
}
