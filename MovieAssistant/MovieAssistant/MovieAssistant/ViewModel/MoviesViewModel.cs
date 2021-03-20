using MovieAssistant.InferenceEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MovieAssistant.ViewModel
{
    class MoviesViewModel : INotifyPropertyChanged
    {
        private MoviesViewModel()
        {
            facts = new List<string>();
            Question = ForwardChainingEngine.getInstance().searchQuestion("-");
        }

        #region Property Changed Event
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        public void RunEngine()
        {
            facts.Add(Question.Domain + selectedAnswer);
            facts = ForwardChainingEngine.getInstance().checkRules(facts);
            Question = ForwardChainingEngine.getInstance().searchQuestion(Question.Domain);

            if(Question == null)
            {
                Movie = ForwardChainingEngine.getInstance().searchMovies(facts);
            }
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
                    selectedAnswer = question.Answers[0];
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

        public string SelectedAnswer
        {
            get
            {
                return selectedAnswer;
            }
            set
            {
                selectedAnswer = value;
            }
        }
        #endregion

        #region Fields
        private Question question;
        private string selectedAnswer;
        private List<string> facts;
        private Movie movie;
        #endregion
    }
}
