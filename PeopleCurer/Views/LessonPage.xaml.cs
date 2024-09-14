using Microsoft.Maui;
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
            if (value != lesson)
            {
                //unsubscribe if possible
                if (lesson != null)
                    lesson.UpdateLessonPartEvent -= (sender, eventArgs) => SwipeToLessonPart(sender, eventArgs);

                BindingContext = value;
                lesson = value;

                //subscribe again to event
                if (lesson != null)
                {
                    lesson.PrepareLesson();
                    lesson.UpdateLessonPartEvent += (sender, eventArgs) => SwipeToLessonPart(sender, eventArgs);
                }

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
        if (e.IntValue == lesson?.LessonParts.Length - 1)
        {
            nextButton.Text = "Abschlieﬂen";
        }
        else if (lesson?.LessonParts[e.IntValue + 1] is EvaluationViewModel)
        {
            nextButton.Text = "Auswerten";
        }
        else
        {
            nextButton.Text = "Weiter";
        }
    }
}