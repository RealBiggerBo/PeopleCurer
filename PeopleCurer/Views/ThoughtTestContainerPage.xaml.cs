using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(ThoughtTestContainerVM), nameof(ThoughtTestContainerVM))]
public partial class ThoughtTestContainerPage : ContentPage
{
    private ThoughtTestContainerViewModel? thoughtTestContainerVM;
    public ThoughtTestContainerViewModel? ThoughtTestContainerVM
    {
        get => thoughtTestContainerVM;
        set
        {
            if (value != thoughtTestContainerVM)
            {
                BindingContext = value;
                thoughtTestContainerVM = value;

                OnPropertyChanged();
            }
        }
    }

    public ThoughtTestContainerPage()
	{
		InitializeComponent();
	}
}