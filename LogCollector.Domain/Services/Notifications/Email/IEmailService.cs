public interface IEmailService
{
	Task SendEmailAsync(string message);
}
