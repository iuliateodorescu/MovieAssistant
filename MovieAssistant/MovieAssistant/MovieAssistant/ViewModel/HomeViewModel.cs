using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MovieAssistant.ViewModel
{
    class HomeViewModel
    {
        public HomeViewModel()
        {
            Images = new List<ImageSource>();

            //var img1 = new Image { Aspect = Aspect.AspectFit };
            var img1 = ImageSource.FromFile("someone_great_poster.jpg");

            //var img2 = new Image { Aspect = Aspect.AspectFit };
            var img2 = ImageSource.FromFile("the_edge_of_seventeen_poster.jpg");

            Images.Add(img1);
            Images.Add(img2);
        }

        #region Properties
        public List<ImageSource> Images
        {
            get { return images; }
            set { this.images = value; }
        }

        #endregion

        #region Fields
        private List<ImageSource> images;
        #endregion
    }
}
