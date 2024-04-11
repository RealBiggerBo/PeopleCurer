using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty("BehaviourExperimentVM", "BehaviourExperimentVM")]
public partial class BehaviourExperimentPage : ContentPage
{
    private BehaviourExperimentViewModel? behaviourExperimentVM;
    public BehaviourExperimentViewModel? BehaviourExperimentVM
    {
        get => behaviourExperimentVM;
        set
        {
            if (value != behaviourExperimentVM)
            {
                BindingContext = value;
                behaviourExperimentVM = value;

                OnPropertyChanged();
            }
        }
    }

    public BehaviourExperimentPage()
	{
        InitializeComponent();
	}
}