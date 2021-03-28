using MovieAssistant.InferenceEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MovieAssistant.ViewModel
{
    class QuestionsViewModel : INotifyPropertyChanged
    {
        public QuestionsViewModel()
        {
            facts = new List<string>();
            Question = ForwardChainingEngine.getInstance().searchQuestion("-",new List<string>());

            RunEngineCommand = new Command(() => RunEngine());
        }

        #region Property Changed Event
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        private void RunEngine()
        {
            facts.Add(Question.Domain + "[" + this.GetSelectedAnswer() + "]");
            facts = ForwardChainingEngine.getInstance().checkRules(facts);

            Question q;
            if ((q = ForwardChainingEngine.getInstance().searchQuestion(Question.Domain, facts))!= null)
            {
                Question = q;
            }
            else
            {
                Movie = ForwardChainingEngine.getInstance().searchMovies(facts);
            }
        }

        private string GetSelectedAnswer()
        {
            foreach (var answer in Question.Answers)
            {
                if (answer.IsChecked)
                    return answer.AnswerText;
            }

            return "";
        }
        #endregion

        #region Properties
        public Question Question
        {
            get
            {
                return question;
            }
            set
            {
                if (value != null)
                {
                    question = value;
                    if (question.Answers.Count > 1)
                    {
                        question.Answers[0].IsChecked = true;
                    }                   
                    NotifyPropertyChanged(nameof(Question));
                }
            }
        }

        public Movie Movie
        {
            get
            {
                return movie;
            }
            set
            {
                movie = value;
                NotifyPropertyChanged(nameof(Movie));
            }
        }
        #endregion

        #region Fields
        private Question question;
        private List<string> facts;
        private Movie movie;
        #endregion

        #region Commands
        public Command RunEngineCommand { get; }
        #endregion
    }
}
