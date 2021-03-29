using MovieAssistant.InferenceEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Question = ForwardChainingEngine.getInstance().searchQuestion("-",facts);

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
            string selectedAnswer = this.GetSelectedAnswer();
            if(selectedAnswer.Contains("<") || selectedAnswer.Contains(">"))
            {
                facts.Add(Question.Domain + selectedAnswer);
            }
            else
            {
                facts.Add(Question.Domain + "[" + this.GetSelectedAnswer() + "]");
            }

            facts = ForwardChainingEngine.getInstance().checkRules(facts);

            Question q;
            if ((q = ForwardChainingEngine.getInstance().searchQuestion(Question.Domain, facts))!= null)
            {
                Question = q;
            }
            else
            {
                Movies = ForwardChainingEngine.getInstance().searchMovies(facts);
                facts = new List<string>();
                Question = ForwardChainingEngine.getInstance().searchQuestion("-", facts);
                finished = true;
                NotifyPropertyChanged(nameof(Movies));
                NotifyPropertyChanged(nameof(ShowResults));
                NotifyPropertyChanged(nameof(ShowQuestions));
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

        public ObservableCollection<Movie> Movies
        {
            get
            {
                return movies;
            }
            set
            {
                movies = value;
                NotifyPropertyChanged(nameof(Movies));
            }
        }

        public bool ShowQuestions
        {
            get
            {
                return !finished;
            }
        }

        public bool ShowResults
        {
            get
            {
                return finished;
            }
        }

        public bool Finished
        {
            set
            {
                finished = value;
                NotifyPropertyChanged(nameof(ShowQuestions));
                NotifyPropertyChanged(nameof(ShowResults));
            }
        }
        #endregion

        #region Fields
        private Question question;
        private List<string> facts;
        private ObservableCollection<Movie> movies;
        private bool finished = false;
        #endregion

        #region Commands
        public Command RunEngineCommand { get; }
        #endregion
    }
}
