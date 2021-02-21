using MovieAssistant.ViewModel;
using Xamarin.Forms;

namespace MovieAssistant.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MoviesViewModel VM = new MoviesViewModel();
            this.BindingContext = VM;
        }
    }
}
