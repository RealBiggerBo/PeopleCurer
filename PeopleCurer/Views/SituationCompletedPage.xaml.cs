using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

//[QueryProperty(nameof(Situation), nameof(Situation))]
[QueryProperty(nameof(DoReward), nameof(DoReward))]
public partial class SituationCompletedPage : ContentPage
{
    //private SituationViewModel? situation;
    //public SituationViewModel? Situation
    //{
    //    get => situation;
    //    set
    //    {
    //        if (value != situation)
    //        {
    //            if (value is null)
    //            {
    //                BindingContext = null;
    //                situation = null;
    //            }
    //            else
    //            {
    //                SituationViewModel? newSituation = new SituationViewModel(value.situation);

    //                BindingContext = newSituation;
    //                situation = newSituation;
    //            }

    //            OnPropertyChanged();
    //        }
    //    }
    //}

    public bool DoReward { get; set; }

    public SituationCompletedPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (!DoReward)
        {
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            //await Shell.Current.GoToAsync($"..//{nameof(RewardPage)}", new Dictionary<string, object>
            //{
            //    ["Lesson"] = new LessonViewModel(new Models.Lesson(string.Empty,string.Empty,false,Models.LessonType.Practice, Models.SaveFrequency.Never, 1000, [])),
            //});
        }
    }
}