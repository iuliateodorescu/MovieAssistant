using MovieAssistant.API;
using MovieAssistant.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MovieAssistant.ViewModel
{
    class MoviesViewModel  : INotifyPropertyChanged
    {
        public MoviesViewModel()
        {
            ApiConnector connector = new ApiConnector();

            movies = connector.getMoviesAsync("Titanic", "Movie");
            OnPropertyChanged(nameof(Source));
        }

        public ImageSource Source
        {
            get
            {
                return movies[2].Image;
            }
        }

        private List<Movie> movies;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
