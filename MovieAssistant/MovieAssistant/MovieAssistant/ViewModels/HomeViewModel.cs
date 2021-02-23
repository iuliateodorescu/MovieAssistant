using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MovieAssistant.ViewModels
{
    class HomeViewModel
    {
        public HomeViewModel()
        {
            Images = new List<Image>();

            var img1 = new Image { Aspect = Aspect.AspectFit };
            img1.Source = ImageSource.FromFile("someone_great_poster.jpg");

            var img2 = new Image { Aspect = Aspect.AspectFit };
            img2.Source = ImageSource.FromFile("the_edge_of_seventeen_poster.jpg");

            Images.Add(img1);
            Images.Add(img2);
        }

        #region Properties
        public List<Image> Images
        {
            get { return images; }
            set { this.images = value; }
        }

        #endregion

        #region Fields
        private List<Image> images;
        #endregion
    }
}
