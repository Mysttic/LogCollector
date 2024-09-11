using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;

	public EmailService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task SendEmailAsync(string address, string subject, string message)
	{
		Console.WriteLine($"Sending email to {address} with subject {subject} and message {message}");
		await SendEmailWithSendGridAsync(address, subject, message);
	}

	private async Task SendEmailWithSendGridAsync(string email, string subject, string message)
	{
		var apiKey = _configuration["SendGrid:ApiKey"];
		var fromEmail = _configuration["SendGrid:FromEmail"];
		var fromName = _configuration["SendGrid:FromName"];

		var client = new SendGridClient(apiKey);
		var from = new EmailAddress(fromEmail, fromName);
		var to = new EmailAddress(email);
		var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);

		var response = await client.SendEmailAsync(msg);

		if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
		{
			throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
		}
	}
}