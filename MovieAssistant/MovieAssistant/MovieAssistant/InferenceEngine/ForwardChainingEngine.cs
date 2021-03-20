using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MovieAssistant.InferenceEngine
{
    class ForwardChainingEngine
    {
        private ForwardChainingEngine()
        {
            rules = File.ReadAllLines("rules.txt");
            movies = File.ReadAllLines("movies.txt");
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
        internal Question searchQuestion(string domain)
        {
            int i = 0;
            while (i < rules.Length)
            {
                if (rules[i].Contains("about[" + domain + "]"))
                {
                    return readQuestion(readRule(rules[i]).Action);
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

        internal Movie searchMovies(List<string> facts)
        {
            int max = 0;
            string selectedMovie = "";
            foreach (var movie in movies)
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
                    selectedMovie = movie;
                }
            }

            return new MovieParser().parseMovieLine(selectedMovie);
        }

        private Question readQuestion(string action)
        {
            string questionDetails = valueBetween(action, "[", "]");
            string[] questionItems = questionDetails.Split(' ');

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
            string[] ruleParts = line.Split(' ');
            string action = ruleParts.Last();

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
        private string[] movies;
        private static ForwardChainingEngine engine = null;
        #endregion
    }
}
