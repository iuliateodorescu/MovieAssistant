using MovieAssistant.InferenceEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieAssistant.ViewModel
{
    class QuestionsViewModel
    {
        public Question Question 
        { 
            get
            {
                return question;
            }
        }

        private Question question=new Question("This is a question","",new List<string>() { "a1","a2","a3" });
    }
}
