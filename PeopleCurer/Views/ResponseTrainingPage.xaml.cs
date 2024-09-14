using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(ResponseTrainingVM),nameof(ResponseTrainingVM))]
public partial class ResponseTrainingPage : ContentPage
{
	private ResponseTrainingViewModel? responseTrainingVM;
	public ResponseTrainingViewModel? ResponseTrainingVM
    {
		get => responseTrainingVM;
		set
		{
			if(value != responseTrainingVM)
			{
                responseTrainingVM = value;

                BindingContext = value;
            }
		}
	}

	public ResponseTrainingPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
}