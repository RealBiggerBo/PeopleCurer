using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(ThoughtTestVM), nameof(ThoughtTestVM))]
public partial class ThoughtTestCompletedPage : ContentPage
{
    private ThoughtTestViewModel? thoughtTestVM;
    public ThoughtTestViewModel? ThoughtTestVM
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
}