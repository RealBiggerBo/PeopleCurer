using PeopleCurer.ViewModels;
using PeopleCurer.Views;
using System.Diagnostics;

namespace PeopleCurer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void RadioButton_CheckedChanged_Info(object sender, CheckedChangedEventArgs e)
        {
            CarouselView.ScrollTo(0);
        }

        private void RadioButton_CheckedChanged_Questions(object sender, CheckedChangedEventArgs e)
        {
            CarouselView.ScrollTo(1);
        }
    }

}
