using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(this.Situation), nameof(this.Situation))]
public partial class SituationEditPage : ContentPage
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

    public SituationEditPage()
	{
		InitializeComponent();
	}
}