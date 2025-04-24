using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ConnectChain.Helpers
{
    public interface IFcmService
    {
        public Task SendNotification(string deviceToken, string message,string title);
    }
    public class FcmService(HttpClient httpClient, IConfiguration config) : IFcmService
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly IConfiguration config = config;

        public async Task SendNotification(string deviceToken, string message, string title)
        {
            var serverKey = config["FCM_SERVER_KEY"];
            if (string.IsNullOrEmpty(serverKey))
            {
                throw new Exception("FCM Server Key is not configured.");
            }
            var payload = new
            {
                to = deviceToken,
                notification = new
                {
                    title,
                    body = message
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            request.Headers.Authorization = new AuthenticationHeaderValue("key", "=" + serverKey);
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
