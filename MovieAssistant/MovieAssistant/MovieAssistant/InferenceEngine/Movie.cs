using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

namespace MovieAssistant.InferenceEngine
{
    internal class Movie : INotifyPropertyChanged
    {
        public Movie(string programType, string title, int year, int duration, List<string> genres, List<string> actors, byte[] imageBytes = null)
        {
            this.ProgramType = programType;
            this.Title = title;
            this.Year = year;
            this.Duration = duration;
            this.Genres = genres;
            this.Actors = actors;
            this.Image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        #region Property Changed Event

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

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
                NotifyPropertyChanged(nameof(ProgramType));
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
                NotifyPropertyChanged(nameof(Title));
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
                NotifyPropertyChanged(nameof(Year));
            }
        }

        public int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                NotifyPropertyChanged(nameof(Duration));
            }
        }

        public List<string> Genres
        {
            get
            {
                return genres;
            }
            set
            {
                genres = value;
                NotifyPropertyChanged(nameof(Genres));
            }
        }

        public List<string> Actors
        {
            get
            {
                return actors;
            }
            set
            {
                actors = value;
                NotifyPropertyChanged(nameof(Actors));
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
                NotifyPropertyChanged(nameof(Image));
            }
        }
        #endregion

        #region Fields
        private string programType;
        private string title;
        private int year;
        private int duration;
        private ImageSource image;
        private List<string> genres;
        private List<string> actors;
        #endregion
    }
}
