using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieGeneratorConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> fileLines = requestTop250Movies();
            fileLines.AddRange(requestTop250TVs());

            File.WriteAllLines("movies.txt", fileLines);
        }

        internal static List<string> requestTop250Movies()
        {
            return requestMoviesFromIMDb("https://imdb-api.com/en/API/Top250Movies/k_rk09y1kf");
        }

        internal static List<string> requestTop250TVs()
        {
            return requestMoviesFromIMDb("https://imdb-api.com/en/API/Top250TVs/k_rk09y1kf");
        }

        internal static List<string> requestMostPopularMovies()
        {
            return requestMoviesFromIMDb("https://imdb-api.com/en/API/MostPopularMovies/k_rk09y1kf");
        }

        internal static List<string> requestMostPopularTVShows()
        {
            return requestMoviesFromIMDb("https://imdb-api.com/en/API/MostPopularTVs/k_rk09y1kf");
        }

        private static List<string> requestMoviesFromIMDb(string url)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };

            List<string> movieList;
            using (var response = Task.Run(async () => await client.SendAsync(request)).Result)
            {
                response.EnsureSuccessStatusCode();
                string result = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                movieList = parseJsonMovieList(result);
            }

            return movieList;
        }

        internal static List<string> parseJsonMovieList(string jsonString)
        {
            JObject movieListJObject = JObject.Parse(jsonString);
            List<string> movieList = new List<string>();

            int i = 1;
            foreach (var movieJson in movieListJObject.Value<IEnumerable<JToken>>("items"))
            {
                string movieLine = getMovieInfoFromIMDb(movieJson.Value<string>("id"));
                Console.WriteLine(i + " " + ExtractStringBetween(movieLine, "title[", "]"));
                movieList.Add(movieLine);
                i++;
            }

            return movieList;
        }

        private static string getMovieInfoFromIMDb(string IMDb_Id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://www.imdb.com/title/" + IMDb_Id),
            };

            string movieLine;
            using (var response = Task.Run(async () => await client.SendAsync(request)).Result)
            {
                response.EnsureSuccessStatusCode();
                string result = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;

                string runtime = extractMinutesFromHtmlResponse(result);
                string json = extractMovieJsonFromHtmlResponse(result);
                JObject movieObject = JObject.Parse(json);

                movieLine = parseMovieJson(movieObject, runtime);
            }

            return movieLine;
        }

        private static string parseDurationFrom(string minutes, string type)
        {
            int minutesValue = int.Parse(minutes);
            if (type.Equals("Movie"))
            {
                if (minutesValue < 90)
                {
                    return "<90";
                }
                else if (minutesValue <= 120)
                {
                    return "[90,120]";
                }
                else
                {
                    return ">120";
                }
            }
            else if (type.Equals("TVSeries"))
            {
                if (minutesValue < 20)
                {
                    return "<20";
                }
                else if (minutesValue <= 40)
                {
                    return "[20,40]";
                }
                else if (minutesValue <= 60)
                {
                    return "[40,60]";
                }
                else
                {
                    return ">60";
                }
            }

            return "";
        }

        private static string ExtractStringBetween(string STR, string FirstString, string LastString)
        {
            string stringStart = STR.Substring(STR.IndexOf(FirstString) + FirstString.Length);

            return stringStart.Substring(0, stringStart.IndexOf(LastString));
        }

        private static string extractMovieJsonFromHtmlResponse(string response)
        {
            string jsonStartScriptTag = "<script type=\"application/ld+json\">";
            string movieJsonStart = response.Substring(response.IndexOf(jsonStartScriptTag) + jsonStartScriptTag.Length);

            return movieJsonStart.Substring(0, movieJsonStart.IndexOf("</script>"));
        }

        private static string extractMinutesFromHtmlResponse(string response)
        {
            string jsonStartScriptTag = "<time datetime=\"PT";
            if (response.Contains(jsonStartScriptTag))
            {
                string movieJsonStart = response.Substring(response.IndexOf(jsonStartScriptTag) + jsonStartScriptTag.Length);

                return movieJsonStart.Substring(0, movieJsonStart.IndexOf("M\""));
            }

            return "";
        }

        private static string parseMovieJson(JObject movieJObject, string runtime)
        {
            string movieLine = "";
            string programType = movieJObject.Value<string>("@type");
            string title = movieJObject.Value<string>("name");
            string year = movieJObject.Value<string>("datePublished").Split('-')[0];
            string language = movieJObject.Value<string>("inLanguage");
            string description = movieJObject.Value<string>("description");

            string duration = "";
            if (runtime != "")
            {
                duration = parseDurationFrom(runtime, programType);
            }

            string director = "";
            try
            {
                director = movieJObject.Value<JToken>("director").Value<string>("name");
            }
            catch
            {
                director = "";
            }

            movieLine += "type[" + programType + "],";
            movieLine += "title[" + title + "],";
            movieLine += "year[" + year + "],";
            movieLine += "language[" + language + "],";
            movieLine += "description[" + description + "],";
            movieLine += "director[" + director + "],";
            movieLine += "time[" + runtime + "],";
            movieLine += "duration" + duration + ",";

            foreach (var actorJson in movieJObject.Value<IEnumerable<JToken>>("actor"))
            {
                string actor = "";
                try
                {
                    actor = actorJson.Value<string>("name");
                }
                catch
                {
                    actor = "";
                }

                movieLine += "actor[" + actor + "],";
            }

            foreach (var genreValue in movieJObject.Value<IEnumerable<JToken>>("genre").Values())
            {
                movieLine += "genre[" + genreValue.ToString() + "],";
            }

            string imageUrl = movieJObject.Value<string>("image");

            movieLine += "image[" + imageUrl + "]";

            return movieLine;
        }

        private static string requestImage(string imageUrl)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(imageUrl),
            };

            string result;
            using (var response = Task.Run(async () => await client.SendAsync(request)).Result)
            {
                response.EnsureSuccessStatusCode();
                result = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
            }

            return result;
        }

    }
}
