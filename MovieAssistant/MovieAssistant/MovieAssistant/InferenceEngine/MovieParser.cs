using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAssistant.InferenceEngine
{
    class MovieParser
    {
        #region Methods
        internal Movie parseMovieLine(string line)
        {
            string[] movieItems = line.Split(',');

            string programType = valueBetween(movieItems.FirstOrDefault(x => x.StartsWith("type")),"[","]");
            string title = valueBetween(movieItems.FirstOrDefault(x => x.StartsWith("title")), "[", "]");
            int year = int.Parse(valueBetween(movieItems.FirstOrDefault(x => x.StartsWith("year")), "[", "]"));
            int duration = int.Parse(valueBetween(movieItems.FirstOrDefault(x => x.StartsWith("time")), "[", "]"));
            List<string> genres = movieItems.Where(x => x.StartsWith("genre")).Select(x => valueBetween(x,"[","]")).ToList();
            List<string> actors = movieItems.Where(x => x.StartsWith("actor")).Select(x => valueBetween(x, "[", "]")).ToList();
            byte[] imageBytes = requestImage(valueBetween(movieItems.FirstOrDefault(x => x.StartsWith("image")), "[", "]"));

            return new Movie(programType, title, year, duration, genres, actors, null);
        }

        private string valueBetween(string value, string startString, string endString)
        {
            return value.Substring(value.IndexOf(startString) + 1, value.IndexOf(endString) - value.IndexOf(startString) - 1);
        }

        private byte[] requestImage(string imageUrl)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(imageUrl),
            };

            byte[] result;
            using (var response = Task.Run(async () => await client.SendAsync(request)).Result)
            {
                response.EnsureSuccessStatusCode();
                result = Task.Run(async () => await response.Content.ReadAsByteArrayAsync()).Result;
            }

            return result;
        }
        #endregion
    }
}
