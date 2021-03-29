using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieAssistant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : Grid
    {
        public ObservableCollection<string> Items { get; set; }

        public ResultsPage()
        {
            InitializeComponent();
        }
    }
}
