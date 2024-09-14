using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(SituationLessonVM),nameof(SituationLessonVM))]
public partial class SituationSummaryPage : ContentPage
{
	private SituationLessonViewModel? situationLessonVM;
	public SituationLessonViewModel? SituationLessonVM
	{
		get => situationLessonVM;
		set
		{
			if(value != situationLessonVM)
			{
				situationLessonVM = value;
				BindingContext = value;

                OnPropertyChanged();
            }
		}
	}

	public SituationSummaryPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
}