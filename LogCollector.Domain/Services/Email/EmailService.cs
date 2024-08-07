﻿using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
	private readonly IConfiguration _configuration;

	public EmailService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task SendEmailAsync(string email, string subject, string message)
	{
		Console.WriteLine($"Sending email to {email} with subject {subject} and message {message}");
		//var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
		//{
		//	Port = int.Parse(_configuration["Smtp:Port"]),
		//	Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
		//	EnableSsl = true,
		//};

		//var mailMessage = new MailMessage
		//{
		//	From = new MailAddress(_configuration["Smtp:Username"]),
		//	Subject = subject,
		//	Body = message,
		//	IsBodyHtml = true,
		//};
		//mailMessage.To.Add(email);

		//await smtpClient.SendMailAsync(mailMessage);
	}
}