using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(ThoughtTestVM), nameof(ThoughtTestVM))]
public partial class ThoughtTestCompletedPage : ContentPage
{
    private ThoughtTestLessonViewModel? thoughtTestVM;
    public ThoughtTestLessonViewModel? ThoughtTestVM
    {
        get => thoughtTestVM;
        set
        {
            if (value != thoughtTestVM)
            {
                BindingContext = value;
                thoughtTestVM = value;

                OnPropertyChanged();
            }
        }
    }

    public ThoughtTestCompletedPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}