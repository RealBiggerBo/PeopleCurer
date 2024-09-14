using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty("BehaviourExperimentVM", "BehaviourExperimentVM")]
public partial class BehaviourExperimentPage : ContentPage
{
    private BehaviourExperimentContainerViewModel? behaviourExperimentVM;
    public BehaviourExperimentContainerViewModel? BehaviourExperimentVM
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

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        const string finish = "Abschließen";
        const string summary = "Zusammenfassung";
        const string edit = "Bearbeiten";
        const string delete = "Löschen";
        const string back = "Zurück";

        //if (sender is ImageButton button && button.BindingContext is SituationLessonViewModel situationVM)
        //{
        //    string result;
        //    if (situationVM.IsCompleted)
        //    {
        //        result = await DisplayActionSheet(situationVM.SituationName, back, null, summary, delete);
        //    }
        //    else
        //    {
        //        result = await DisplayActionSheet(situationVM.SituationName, back, null, finish, edit, delete);
        //    }

        //    switch (result)
        //    {
        //        case finish:
        //            situationVM.FinishSituation.Execute(null);
        //            break;
        //        case summary:
        //            situationVM.GoToSituationPage.Execute(null);
        //            break;
        //        case edit:
        //            situationVM.GoToSituationPage.Execute(null);
        //            break;
        //        case delete:
        //            behaviourExperimentVM?.DeleteSituation.Execute(situationVM);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}