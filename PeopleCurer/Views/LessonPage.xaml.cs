using PeopleCurer.CustomEventArgs;
using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(Lesson), nameof(Lesson))]
public partial class LessonPage : ContentPage
{
    private LessonViewModel? lesson;
    public LessonViewModel? Lesson
    {
        get => lesson;
        set
        {
            //unsubscribe if possible
            if (lesson != null)
                lesson.NextLessonPartEvent -= (sender, eventArgs) => SwipeToLessonPart(sender, eventArgs);

            if (value != lesson)
            {
                BindingContext = value;
                lesson = value;

                //subscripe again to event
                if(lesson != null)
                    lesson.NextLessonPartEvent += (sender, eventArgs) => SwipeToLessonPart(sender, eventArgs);

                OnPropertyChanged();
            }
        }
    }

    public LessonPage()
	{
		InitializeComponent();
	}

    private void SwipeToLessonPart(object? sender, IntEventArgs e)
    {
        CarouselView.ScrollTo(e.IntValue);
    }
}