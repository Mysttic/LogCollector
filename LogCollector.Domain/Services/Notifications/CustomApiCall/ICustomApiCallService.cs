public interface ICustomApiCallService
{
	Task SendCustomApiCallAsync(string url, string authkey, string message);
}