using System;
using System.Collections.Generic;
using System.Text;

namespace MovieAssistant.InferenceEngine
{
    class Question
    {
        public Question(string questionText, string domain, List<string> answerTexts)
        {
            this.questionText = questionText;
            this.domain = domain;
            this.answers = new List<Answer>();

            foreach(var answerText in answerTexts)
            {
                this.answers.Add(new Answer(answerText));
            }
        }

        #region Properties
        public string QuestionText
        {
            get
            {
                return questionText;
            }
        }

        public List<Answer> Answers
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
        private List<Answer> answers;
        #endregion
    }
}
