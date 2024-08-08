public interface IEmailService
{
	Task SendEmailAsync(string address, string subject, string message);
}
