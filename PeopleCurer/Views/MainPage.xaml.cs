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

        private void RadioButton_MainPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                CarouselView.ScrollTo(0);
                //Update SymptomCheck -> Button
                ((MainPageViewModel)this.BindingContext).GoToSymptomCheckLesson.RaiseCanExecuteChanged();
            }
        }

        private void RadioButton_TrainingPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
                CarouselView.ScrollTo(1);
        }

        private void RadioButton_StrengthsPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
                CarouselView.ScrollTo(2);
        }

        private void RadioButton_StatisticsPage_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(e.Value == true)
                CarouselView.ScrollTo(3);
        }
    }

}
