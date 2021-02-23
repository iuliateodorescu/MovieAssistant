using MovieAssistant.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace MovieAssistant.API
{
    internal class ApiConnector
    {
        internal async void connect()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com/entertainment/match/?Title=Titanic&ProgramType=Movie"),
                    Headers =
                {
                    { "x-rapidapi-key", apiKey },
                    { "x-rapidapi-host", apiHost },
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    List<Movie> movies = parseMovieListJson(JObject.Parse(body));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("No Internet Connection");
            }
        }

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
            
            //Bitmap image

            return new Movie(programType, title, year, language, runtime, description, contributors);
        }

        #region Fields
        private string apiKey = "7666ee6291msh2893aa2cef00efap1bc99fjsncfa4e7c79b60";
        private string apiHost = "ivaee-internet-video-archive-entertainment-v1.p.rapidapi.com";
        #endregion
    }
}
