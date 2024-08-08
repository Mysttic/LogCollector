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

	public async Task SendSMSAsync(string message)
	{
		string phoneNumber = _configuration["Twilio:PhoneNumber"] ?? "0";
		string from = _configuration["Twilio:From"] ?? "LogCollector Alert";

		Console.WriteLine($"Sending SMS to {phoneNumber} from {from} with message {message}");
		await SendSMSWithTwilioAsync(phoneNumber, from, message);
	}

	private async Task SendSMSWithTwilioAsync(string phoneNumber, string from, string message)
	{
		var accountSid = _configuration["Twilio:AccountSid"];
		var authToken = _configuration["Twilio:AuthToken"];

		TwilioClient.Init(accountSid, authToken);

		var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
		{
			From = new PhoneNumber(from),
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
