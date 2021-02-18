using System;

namespace MovieAssistant.Model
{
    internal class Contributor
    {
        public Contributor(string name, string job)
        {
            this.name = name;
            this.job = job;
        }

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Job
        {
            get
            {
                return job;
            }
            set
            {
                this.job = value;
            }
        }
        #endregion

        #region Fields
        private string name;
        private string job;
        #endregion
    }
}
