namespace PeopleCurer.Views;

public partial class BodyScanPage : ContentPage
{
	public BodyScanPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
}