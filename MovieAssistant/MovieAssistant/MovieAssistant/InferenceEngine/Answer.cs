using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MovieAssistant.InferenceEngine
{
    class Answer : INotifyPropertyChanged
    {
        public Answer(string answerText)
        {
            this.answerText = answerText;
        }

        #region Property
        public string AnswerText
        {
            get
            {
                return answerText;
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                NotifyPropertyChanged(nameof(IsChecked));
            }
        }
        #endregion

        #region Fields
        private string answerText;
        private bool isChecked;
        #endregion

        #region Property Changed Event
        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
