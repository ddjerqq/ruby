namespace Application.Services;

public interface IEmailSender
{
    public bool SendGmail(string subject, string content, string[] recipients, string from);
}