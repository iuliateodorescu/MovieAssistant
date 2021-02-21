﻿using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace MovieAssistant.Model
{
    internal class Movie
    {
        public Movie(string programType, string title, int year, string language, int runtime, string description, List<Contributor> contributors, byte[] imageBytes = null)
        {
            this.programType = programType;
            this.title = title;
            this.year = year;
            this.language = language;
            this.runtime = runtime;
            this.description = description;
            this.contributors = contributors;
            this.image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        #region Properties
        public string ProgramType
        {
            get
            {
                return programType;
            }
            set
            {
                programType = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
            }
        }

        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
            }
        }

        public int Runtime
        {
            get
            {
                return runtime;
            }
            set
            {
                runtime = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public List<Contributor> Contributors
        {
            get
            {
                return contributors;
            }
            set
            {
                contributors = value;
            }
        }

        public ImageSource Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }
        #endregion

        #region Fields
        private string programType;
        private string title;
        private int year;
        private string language;
        private int runtime;
        private ImageSource image;
        private string description;
        private List<Contributor> contributors;
        #endregion
    }
}
