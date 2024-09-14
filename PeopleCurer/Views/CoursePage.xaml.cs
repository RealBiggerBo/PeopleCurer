using PeopleCurer.ViewModels;
using System.Diagnostics;

namespace PeopleCurer.Views;

[QueryProperty(nameof(Course), nameof(Course))]
public partial class CoursePage : ContentPage
{
	private CourseViewModel? course;
	public CourseViewModel? Course
	{
		get => course;
		set
		{
			if (value != course)
			{
				BindingContext = value;
				course = value;

				OnPropertyChanged();
			}
		}
	}

	public CoursePage()
	{
		InitializeComponent();
	}
}