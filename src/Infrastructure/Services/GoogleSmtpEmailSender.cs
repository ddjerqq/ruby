using Application.Services;

namespace Infrastructure.Services;

public sealed class GoogleSmtpEmailSender : IEmailSender
{
    public bool SendGmail(string subject, string content, string[] recipients, string from)
    {
        if (recipients == null || recipients.Length == 0)
            throw new ArgumentException(null, nameof(recipients));

        // TODO get credentials from the env
        var gmailClient = new System.Net.Mail.SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new System.Net.NetworkCredential("******", "*****")
        };

        using var msg = new System.Net.Mail.MailMessage(from, recipients[0], subject, content);

        foreach (var recipient in recipients)
        {
            msg.To.Add(recipient);
        }

        try
        {
            gmailClient.Send(msg);
            return true;
        }
        catch (Exception)
        {
            // TODO: Handle the exception
            return false;
        }
    }
}