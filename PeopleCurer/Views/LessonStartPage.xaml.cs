using PeopleCurer.Models;
using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(Lesson), nameof(Lesson))]
public partial class LessonStartPage : ContentPage
{
    private LessonViewModel? lesson;
    public LessonViewModel? Lesson
    {
        get => lesson;
        set
        {
            if (value != lesson)
            {
                BindingContext = value;
                lesson = value;

                OnPropertyChanged();
            }
        }
    }

    public LessonStartPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("../" + nameof(LessonPage), new Dictionary<string, object?>
        {
            ["Lesson"] = lesson
        }); 
    }
}