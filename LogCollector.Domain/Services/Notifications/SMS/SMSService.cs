using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class SMSService : ISMSService
{
	private readonly IConfiguration _configuration;

	public SMSService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task SendSMSAsync(string phoneNumber, string message)
	{
		Console.WriteLine($"Sending SMS to {phoneNumber} with message {message}");
		await SendSMSWithTwilioAsync(phoneNumber, message);
	}

	private async Task SendSMSWithTwilioAsync(string phoneNumber, string message)
	{
		var accountSid = _configuration["Twilio:AccountSid"];
		var authToken = _configuration["Twilio:AuthToken"];
		var fromeNumber = _configuration["Twilio:FromNumber"];

		TwilioClient.Init(accountSid, authToken);

		var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
		{
			From = fromeNumber,
			Body = message
		};

		var msg = await MessageResource.CreateAsync(messageOptions);

		if (msg.ErrorCode != null)
		{
			throw new Exception($"Failed to send SMS. Error code: {msg.ErrorCode}");
		}
		else
		{
			Console.WriteLine($"SMS sent: {msg.Sid}");
		}
	}
}