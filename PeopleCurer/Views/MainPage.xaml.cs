using Microsoft.Maui.Controls.Shapes;
using PeopleCurer.Services;
using PeopleCurer.ViewModels;
using PeopleCurer.Views;
using System.Diagnostics;

namespace PeopleCurer.Views
{
    public partial class MainPage : ContentPage
    {
        private const double startSize = 40;
        private const double endSize = 50;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void RadioButton_TrainingPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                await ScrollTo(rb, e.Value, 0);
            }
        }

        private async void RadioButton_MainPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                await ScrollTo(rb, e.Value, 1);
            }
        }

        private async void RadioButton_StatisticsPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                await ScrollTo(rb, e.Value, 2);
            }
        }

        private async void RadioButton_SettingsPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                await ScrollTo(rb, e.Value, 3);
            }
        }

        private async Task ScrollTo(RadioButton rb, bool getsChecked, int targetPageIndex)
        {
            if (getsChecked == true)
            {
                Animation anim = new((t) => { rb.WidthRequest = double.Lerp(startSize, endSize, t); rb.HeightRequest = double.Lerp(startSize, endSize, t); });
                rb.Animate("ScaleUpAnimation" + targetPageIndex, anim);

                if (targetPageIndex == 1)
                {
                    //UpdateCanGoToLesson SymptomCheck -> Button
                    ((MainPageViewModel)this.BindingContext).UpdateSymptomCheckLessonActiveStatus();
                }

                if (CarouselView is null)
                    return;
                await Task.Run(() => CarouselView.ScrollTo(targetPageIndex));
            }
            else
            {
                Animation anim = new((t) => { rb.WidthRequest = double.Lerp(endSize, startSize, t); rb.HeightRequest = double.Lerp(endSize, startSize, t); });
                rb.Animate("ScaleDownAnimation" + targetPageIndex, anim);
            }
        }

        private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            if(CarouselView.VisibleViews.Count == 0)
            {
                CarouselView.Position = 1;
            }
        }
    }
}
