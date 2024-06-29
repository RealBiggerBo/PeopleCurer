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

        public BehaviourExperimentViewModel? BehaviourExperiment { get; }
        public ThoughtTestContainerViewModel? ThoughtTestContainer { get; }
        public RelaxationProcedureContainerViewModel? RelaxationProcedureContainer { get; }

        public DelegateCommand GoToSymptomCheckLesson { get; }

        public float LevelProgress
        {
            get => RewardManager.GetCurrentLevelProgress();
        }

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

                this.BehaviourExperiment = vm.Features[0] as BehaviourExperimentViewModel;
                this.ThoughtTestContainer = vm.Features[1] as ThoughtTestContainerViewModel;
                this.RelaxationProcedureContainer = vm.Features[2] as RelaxationProcedureContainerViewModel;
            }

            if(SerializationManager.LoadStrengthsPage(out TherapyPage? strengthsPage, true))
            {
                TherapyPageViewModel vm = new TherapyPageViewModel(strengthsPage!);
                ProgressUpdateManager.SetStrengthsPage(vm);
                TherapyPages.Add(vm);
            }

            RewardManager.OnRewardXPChanged += UpdateLevelProgress;
            //UpdateLevelProgress(null, EventArgs.Empty);
        }

        private void UpdateLevelProgress(object? s, EventArgs e)
        {
            base.RaisePropertyChanged(nameof(LevelProgress));
        }
    }
}
