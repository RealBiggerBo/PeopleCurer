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

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        const string back = "Zurück";
        const string summary = "Zusammenfassung";
        const string delete = "Löschen";

        if (sender is ImageButton button && button.BindingContext is ThoughtTestLessonViewModel testVM)
        {
            string result = await DisplayActionSheet(testVM.ThoughtTestName, back, null, summary, delete);
            switch (result)
            {
                case summary:
                    testVM.GoToLessonPage.Execute(null);
                    break;
                case delete:
                    thoughtTestContainerVM?.DeleteThoughtTest.Execute(testVM);
                    break;
                default:
                    break;
            }
        }
    }
}