using System;
using System.Collections.Generic;
using System.Text;

namespace MovieAssistant.InferenceEngine
{
    class Rule
    {
        public Rule(List<string> conditions, string action)
        {
            this.conditions = conditions;
            this.action = action;
        }

        internal bool isSatisfied(List<string> trueFacts)
        {
            foreach(string fact in conditions)
            {
                if (!trueFacts.Contains(fact))
                {
                    return false;
                }
            }

            return true;
        }

        #region Properties
        public string Action
        {
            get
            {
                return action;
            }
        }
        #endregion

        #region Fields
        private List<string> conditions;
        private string action;
        #endregion
    }
}
