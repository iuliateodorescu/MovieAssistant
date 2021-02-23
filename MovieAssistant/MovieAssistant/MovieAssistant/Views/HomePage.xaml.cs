using Syncfusion.SfCarousel.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieAssistant.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            SfCarousel carousel = new SfCarousel()
            {
                ItemWidth = 170,
                ItemHeight = 250
            };

            ObservableCollection<SfCarouselItem> carouselItems = new ObservableCollection<SfCarouselItem>();

            carouselItems.Add(new SfCarouselItem() { ImageName = "someone_great_poster.jpg" });
            carouselItems.Add(new SfCarouselItem() { ImageName = "someone_great_poster.jpg" });
            carouselItems.Add(new SfCarouselItem() { ImageName = "someone_great_poster.jpg" });
            carouselItems.Add(new SfCarouselItem() { ImageName = "someone_great_poster.jpg" });
            carouselItems.Add(new SfCarouselItem() { ImageName = "someone_great_poster.jpg" });

            carousel.ItemsSource = carouselItems;

            this.Content = carousel;
        } 

    
    }
}