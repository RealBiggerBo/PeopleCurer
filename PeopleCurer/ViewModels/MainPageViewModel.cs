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
        public DelegateCommand GoToBehaivourExperimentPage { get; }
        public DelegateCommand GoToThoughtTestPage { get; }
        public DelegateCommand GoToRelaxPage { get; }
        //public DelegateCommand GoToResponseTraining { get; }

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
            if (SerializationManager.LoadSymptomCheckData(out TherapyPage? symptomCheckPage,true))
            {
                TherapyPageViewModel vm = new TherapyPageViewModel(symptomCheckPage!);
                ProgressUpdateManager.SetSymptomCheckPage(vm);
                TherapyPages.Add(vm);

                GoToSymptomCheckLesson = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(LessonPage),
                    new Dictionary<string, object>
                    {
                        ["Lesson"] = ((CourseViewModel)vm.Features[0]).Lessons[0]
                    }),
                    (obj) =>
                    {
                        if(DateOnly.FromDateTime(DateTime.UtcNow) > PreferenceManager.GetLastSymptomCheckDate())
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

            //Create Training TherapyPage
            TherapyPage trainingPage = new TherapyPage("Trainingsbereich", "Herzlich wilkommen im Trainingsbereich. Wie im Sport sind theoretische Inhalte die Vorraussetzung, um die Übungen gut auszuführen. Doch die eigentlichen Ergebnisse werden Ergebnisse werden erst im Training erzielt.", GetTrainingPageFeatures());
            TherapyPages.Add(new TherapyPageViewModel(trainingPage));

            GoToBehaivourExperimentPage = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(BehaviourExperimentPage),
                new Dictionary<string, object>
                {
                    ["BehaviourExperimentVM"] = (BehaviourExperimentViewModel)TherapyPages[2].Features[0]
                }));
        }

        private Feature[] GetTrainingPageFeatures()
        {
            return [
                new BehaviourExperiment([new Situation("neue Situation", "", [], [], 0, string.Empty),new Situation("neue Situation2", "", [], [], 0, string.Empty)])
                ];
        }
    }

    enum Pages
    {
        Info,
        Questions
    }
}
