using Microsoft.Maui.Controls.Shapes;
using PeopleCurer.ViewModels;
using PeopleCurer.Views;
using System.Diagnostics;

namespace PeopleCurer.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ((MainPageViewModel)this.BindingContext).GoToSymptomCheckLesson.RaiseCanExecuteChanged();

            base.OnAppearing();
        }

        private async void RadioButton_MainPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                await Task.Run(() => CarouselView.ScrollTo(0));
                //UpdateCanGoToLesson SymptomCheck -> Button
                ((MainPageViewModel)this.BindingContext).GoToSymptomCheckLesson.RaiseCanExecuteChanged();
            }
        }

        private async void RadioButton_TrainingPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
                await Task.Run(() => CarouselView.ScrollTo(1));
        }

        private async void RadioButton_StrengthsPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
                await Task.Run(() => CarouselView.ScrollTo(2));
        }

        private async void RadioButton_StatisticsPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
                await Task.Run(() => CarouselView.ScrollTo(3));
        }
    }

}
