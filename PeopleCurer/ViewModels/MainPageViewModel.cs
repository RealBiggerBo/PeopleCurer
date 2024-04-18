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

        public CourseViewModel? Module1 { get; }
        public CourseViewModel? Module2 { get; }
        public CourseViewModel? Module3 { get; }
        public CourseViewModel? Module4 { get; }

        public DelegateCommand GoToSymptomCheckLesson { get; }
        public DelegateCommand GoToBehaivourExperimentPage { get; }
        public DelegateCommand GoToThoughtTestPage { get; }
        public DelegateCommand GoToRelaxPage { get; }
        //public DelegateCommand GoToResponseTraining { get; }

        public MainPageViewModel()
        {
            if(!PreferenceManager.GetWelcomePageCompletionStatus())
                Shell.Current.GoToAsync(nameof(WelcomePage));

            TherapyPages = new ObservableCollection<TherapyPageViewModel>();

            //Load '4 Module'
            if (SerializationManager.LoadCourses(out TherapyPage? page, true))
            {
                TherapyPageViewModel vm = new TherapyPageViewModel(page!);
                ProgressUpdateManager.SetCoursesPage(vm);
                TherapyPages.Add(vm);

                Module1 = vm.Features[0] as CourseViewModel;
                Module2 = vm.Features[1] as CourseViewModel;
                Module3 = vm.Features[2] as CourseViewModel;
                Module4 = vm.Features[3] as CourseViewModel;
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

            //Load Training TherapyPage
            if (SerializationManager.LoadTrainingPage(out TherapyPage? trainingPage, true))
            {
                TherapyPageViewModel vm = new TherapyPageViewModel(trainingPage!);
                ProgressUpdateManager.SetTrainingPage(vm);
                TherapyPages.Add(vm);

                GoToBehaivourExperimentPage = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(BehaviourExperimentPage),
                    new Dictionary<string, object>
                    {
                        ["BehaviourExperimentVM"] = (BehaviourExperimentViewModel)TherapyPages[2].Features[0]
                    }));
                GoToThoughtTestPage = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(ThoughtTestContainerPage),
                    new Dictionary<string, object>
                    {
                        ["ThoughtTestContainerVM"] = ((ThoughtTestContainerViewModel)vm.Features[1])
                    }));
    /// <summary>
    /// 
    /// TODO: 
    /// create lessonparts for kognitive umstrukturierung etc
    ///     
    /// 
    /// 
    /// Audio und Video????
    /// 
    /// 
    /// Meine Stärken uns Meilensteine
    /// 
    /// </summary>
            }
        }
    }
}
