using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using PeopleCurer.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PeopleCurer.ViewModels
{
    class MainPageViewModel : NotifyableBaseObject
    {
        public ObservableCollection<TherapyPageViewModel> TherapyPages { get; }

        public DelegateCommand GoToThoughtsPage { get; }
        public DelegateCommand GoToSymptomCheckLesson { get; }

        public MainPageViewModel()
        {
            Shell.Current.GoToAsync(nameof(WelcomePage));

            //SerializationManager.RemoveCourses();

            TherapyPages = new ObservableCollection<TherapyPageViewModel>();

            //Load '4 Module'
            if (SerializationManager.LoadCourses(out TherapyPage? page, true))
            {
                TherapyPageViewModel vm = new TherapyPageViewModel(page!);
                ProgressUpdateManager.SetCoursesPage(vm);
                TherapyPages.Add(vm);

                GoToThoughtsPage = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(CoursePage),
                    new Dictionary<string, object>
                    {
                        ["Course"] = vm.Features[0]
                    }));
            }
            else
            {
                GoToThoughtsPage = new DelegateCommand(null);
            }

            //Load Symptomcheckfragen
            if (SerializationManager.LoadSymptomCheckData(out TherapyPage? symptomCheckPage))
            {
                TherapyPageViewModel vm = new TherapyPageViewModel(symptomCheckPage!);
                TherapyPages.Add(vm);

                GoToSymptomCheckLesson = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(LessonPage),
                    new Dictionary<string, object>
                    {
                        ["Lesson"] = ((CourseViewModel)vm.Features[0]).Lessons[0]
                    }),
                    (obj) =>
                    {
                        if(DateTime.Now.Date > PreferenceManager.GetLastSymptomCheckDate().Date)
                        {
                            return true;
                        }
                        return false;
                    });
            }
            else
            {
                GoToSymptomCheckLesson = new DelegateCommand(null);
            }

            //Create Statistics TherapyPage
            TherapyPage statisticsPage = new TherapyPage("Statistiken", "Statisken aus Symptomchecks", GetStatistics());
            TherapyPages.Add(new TherapyPageViewModel(statisticsPage));
        }

        private Statistics[] GetStatistics()
        {
            return [new Statistics("A", "CDC", "1")];
        }
    }

    enum Pages
    {
        Info,
        Questions
    }
}
