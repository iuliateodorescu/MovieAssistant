using System;
using System.Net.Http;

namespace MovieAssistant.API
{
    internal class ApiConnector
    {
        internal async void connect()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com/Images/%7Bfilepath%7D/Redirect?Redirect=True"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "x-rapidapi-key", "7666ee6291msh2893aa2cef00efap1bc99fjsncfa4e7c79b60" },
                    { "x-rapidapi-host", "ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }

            string a = "";
        }

    }
}
