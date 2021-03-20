using System;
using System.Collections.Generic;
using System.Text;

namespace MovieAssistant.InferenceEngine
{
    class Question
    {
        public Question(string questionText, string domain, List<string> answers)
        {
            this.questionText = questionText;
            this.domain = domain;
            this.answers = answers;
        }

        #region Properties
        public string QuestionText
        {
            get
            {
                return questionText;
            }
        }

        public List<string> Answers
        {
            get
            {
                return answers;
            }
        }

        public string Domain
        {
            get
            {
                return domain;
            }
        }
        #endregion

        #region Fields
        private string questionText;
        private string domain;
        private List<string> answers;
        #endregion
    }
}
