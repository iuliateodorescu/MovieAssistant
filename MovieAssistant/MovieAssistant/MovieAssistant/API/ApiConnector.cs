using MovieAssistant.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieAssistant.API
{
    internal class ApiConnector
    {
        #region Methods
        internal List<Movie> getMoviesAsync(string title, string programType)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com/entertainment/match/?Title=" + title + "&ProgramType=" + programType),
                    Headers =
                {
                    { "x-rapidapi-key", apiKey },
                    { "x-rapidapi-host", apiHost },
                },
                };
                using (var response = Task.Run(async () => await client.SendAsync(request)).Result)
                {
                    response.EnsureSuccessStatusCode();
                    string body = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;

                    return parseMovieListJson(JObject.Parse(body));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No Internet Connection");

                return null;
            }
        }

        #region Json Parser Methods
        private List<Movie> parseMovieListJson(JObject movieListJson)
        {
            List<Movie> movieList = new List<Movie>();

            foreach (var movieJson in movieListJson.Value<IEnumerable<JToken>>("ProgramMatches"))
            {
                movieList.Add(parseMovieJson(movieJson));
            }

            return movieList;
        }

        private Movie parseMovieJson(JToken movieJson)
        {
            string programType = movieJson.Value<string>("ProgramType");
            string title = movieJson.Value<string>("Title");
            int year = movieJson.Value<int>("Year");
            string language = movieJson.Value<string>("OriginalLanguage");
            int runtime = movieJson.Value<int>("Runtime");
            string description = "";

            try
            {
                description = movieJson.Value<JToken>("Descriptions").First.Value<string>("Description");
            }
            catch (Exception) { }

            List<Contributor> contributors = new List<Contributor>();

            try
            {
                foreach (var contributor in movieJson.Value<JToken>("Contributors"))
                {
                    contributors.Add(new Contributor(contributor.Value<string>("PersonName"), contributor.Value<string>("Job")));
                }
            }
            catch (Exception) { }

            byte[] imageBytes = null;
            try
            {
                string image_filepath = movieJson.Value<JToken>("Summary").Value<JToken>("Image").Value<string>("FilePath");
                imageBytes = requestImageFromService(image_filepath);
            }
            catch (Exception) { }

            return new Movie(programType, title, year, language, runtime, description, contributors, imageBytes);
        }

        private byte[] requestImageFromService(string filepath)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com/Images/" + filepath + "/Redirect?Redirect=True"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "x-rapidapi-key", "7666ee6291msh2893aa2cef00efap1bc99fjsncfa4e7c79b60" },
                    { "x-rapidapi-host", "ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com" },
                },
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

        #endregion

        #region Fields
        private string apiKey = "7666ee6291msh2893aa2cef00efap1bc99fjsncfa4e7c79b60";
        private string apiHost = "ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com";
        #endregion
    }
}
