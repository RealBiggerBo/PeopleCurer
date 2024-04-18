using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(Situation), nameof(Situation))]
public partial class SituationCompletedPage : ContentPage
{
    private SituationViewModel? situation;
    public SituationViewModel? Situation
    {
        get => situation;
        set
        {
            if (value != situation)
            {
                BindingContext = value;
                situation = value;

                OnPropertyChanged();
            }
        }
    }

    public SituationCompletedPage()
	{
		InitializeComponent();
	}
}