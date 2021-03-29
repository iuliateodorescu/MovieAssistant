using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MovieAssistant.InferenceEngine
{
    class ForwardChainingEngine
    {
        private ForwardChainingEngine()
        {
            rules = readFileFromResources("rules.txt");
            movieFileLines = readFileFromResources("movies.txt");
        }

        internal static ForwardChainingEngine getInstance()
        {
            if (engine == null)
            {
                engine = new ForwardChainingEngine();
            }

            return engine;
        }

        #region Methods
        private string[] readFileFromResources(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(filename));
            string content;
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }
            }

            return content.Split('\n');
        }

        internal Question searchQuestion(string domain, List<string> facts)
        {
            int i = 0;
            List<string> trueFacts = new List<string>();
            trueFacts.AddRange(facts);
            trueFacts.Add("about[" + domain + "]");
            while (i < rules.Length)
            {
                if(rules[i].Contains("about[" + domain + "]"))
                {
                    Rule r = readRule(rules[i]);
                    if (r.isSatisfied(trueFacts))
                    {
                        return readQuestion(r.Action);
                    }
                }

                i++;
            }

            return null;
        }

        internal List<string> checkRules(List<string> trueFacts)
        {
            int i = 0;

            while (i < rules.Length)
            {
                Rule rule = readRule(rules[i]);
                if (rule.isSatisfied(trueFacts) && !trueFacts.Contains(rule.Action))
                {
                    trueFacts.Add(rule.Action);

                    i = 0;
                }
                else
                {
                    i++;
                }
            }

            return trueFacts;
        }

        internal ObservableCollection<Movie> searchMovies(List<string> facts)
        {
            int max = 0;
            List<string> selectedMovies = new List<string>();
            foreach (var movie in movieFileLines)
            {
                int factCounter = 0;
                foreach (var fact in facts)
                {
                    if (movie.Contains(fact))
                    {
                        factCounter++;
                    }
                }

                if (factCounter > max)
                {
                    max = factCounter;
                    selectedMovies.Clear();
                }

                if(factCounter == max && selectedMovies.Count<5)
                {
                    selectedMovies.Add(movie);
                }
            }

            ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
            MovieParser parser = new MovieParser();
            foreach(var selectedMovie in selectedMovies)
            {
                movies.Add(parser.parseMovieLine(selectedMovie));
            }

            return movies;
            //return new Movie("Movie", "Titanic", 1998, 123, new List<string>() { "drama" }, new List<string>() { "DiCaprio" });
        }

        private Question readQuestion(string action)
        {
            string questionDetails = valueBetween(action, "[", "]");
            string[] questionItems = questionDetails.Split('*');

            string question = questionItems[0];
            string domain = questionItems.Last();

            List<string> answers = new List<string>();

            for (int i = 1; i < questionItems.Length - 1; i++)
            {
                answers.Add(questionItems[i]);
            }

            return new Question(question, domain, answers);
        }

        private Rule readRule(string line)
        {
            string[] ruleParts = line.Split('_');
            string action = ruleParts.Last().Trim();

            List<string> conditions = new List<string>();
            foreach (var part in ruleParts)
            {
                if (part.Equals("THEN"))
                {
                    break;
                }
                else if (!part.Equals("IF") && !part.Equals("AND"))
                {
                    conditions.Add(part);
                }
            }

            return new Rule(conditions, action);
        }

        private string valueBetween(string value, string startString, string endString)
        {
            return value.Substring(value.IndexOf(startString) + 1, value.LastIndexOf(endString) - value.IndexOf(startString) - 1);
        }
        #endregion

        #region Fields
        private string[] rules;
        private string[] movieFileLines;
        private static ForwardChainingEngine engine = null;
        #endregion
    }
}
