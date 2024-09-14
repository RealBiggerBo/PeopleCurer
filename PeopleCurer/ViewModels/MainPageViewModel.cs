using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using PeopleCurer.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace PeopleCurer.ViewModels
{
    class MainPageViewModel : NotifyableBaseObject
    {
        public ObservableCollection<TherapyPageViewModel> TherapyPages { get; }

        public CourseViewModel? Module1 { get; }
        public CourseViewModel? Module2 { get; }
        public CourseViewModel? Module3 { get; }
        public CourseViewModel? Module4 { get; }

        public LessonViewModel? SymptomCheckLesson { get; }

        public BehaviourExperimentContainerViewModel? BehaviourExperimentContainer { get; }
        public ThoughtTestContainerViewModel? ThoughtTestContainer { get; }
        public RelaxationProcedureContainerViewModel? RelaxationProcedureContainer { get; }
        public BodyScanContainerViewModel? BodyScanContainer { get; }
        public ResponseTrainingContainerViewModel? ResponseTrainingContainer { get; }

        public DelegateCommand GoToSymptomCheckLessonN { get; }
        public DelegateCommand GoToChatBotPageCmd { get; }

        public DelegateCommand SendDataCmd { get; }
        public DelegateCommand EditUserInputGoal { get; }
        public DelegateCommand OpenImprint { get; }

        public int Level
        {
            get => RewardManager.GetCurrentLevel();
        }
        public float LevelProgress
        {
            get => RewardManager.GetCurrentLevelProgress();
        }
        public string LevelProgressString
        {
            get => RewardManager.GetCurrentXP() + "/" + RewardManager.GetRequiredLevelUpXP();
        }

        private string userInputGoal = string.Empty;
        public string UserInputGoal
        {
            get => userInputGoal;
            set
            {
                if(value != userInputGoal)
                {
                    userInputGoal = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public MainPageViewModel()
        {
            ////
            SendDataCmd = new(async (obj) => 
            {
                if(await Shell.Current.DisplayAlert("Bist du sicher?", "Dies sendet alle deinen Fortschritt sowie jegliche Antworten per Mail. Der Prozess kann einige Zeit in Anspruch nehmen.", "Senden", "Abbrechen"))
                {
                    if(await Task.Run(() => DataSendManager.SendData()))
                    {
                        await Shell.Current.DisplayAlert("Erfolg!", "Deine Daten wurden erfolgreich gesendet", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Fehlschlag!", "Deine Daten wurden leider NICHT erfolgreich gesendet", "OK");
                    }
                }
            });
            ////
            EditUserInputGoal = new(async (obj) =>
            {
                if (Shell.Current is null)
                {
                    return;
                }

                string newVal = await Shell.Current.DisplayPromptAsync("Dein Ziel:", "Hier kannst du dein Ziel anpassen, wenn du möchtest.", "OK", "Abbrechen", "Dein Ziel...", initialValue: UserInputGoal);

                if (newVal != UserInputGoal)
                {
                    UserInputDataManager.SetData("Welcome4", newVal, true);
                }
            });
            OpenImprint = new(async (obj) =>
            {
                await Shell.Current.DisplayAlert("Impressum", "Hier könnte ihr Impressum stehen XD", "Zurück");
            });

            if(!PreferenceManager.GetWelcomePageCompletionStatus())
                Shell.Current.GoToAsync(nameof(WelcomePage));

            TherapyPages = [];

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
            if (SerializationManager.LoadSymptomCheckData(out NormalLesson? symptomCheckLesson, true))
            {

                //TherapyPageViewModel vm = new TherapyPageViewModel(symptomCheckLesson!);
                NormalLessonViewModel vm = new(symptomCheckLesson!);

                SymptomCheckLesson = vm;

                ProgressUpdateManager.SetSymptomCheckPage(symptomCheckLesson!);
                //TherapyPages.Add(vm);

                GoToSymptomCheckLessonN = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(LessonPage),
                    new Dictionary<string, object>
                    {
                        ["Lesson"] = vm
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
                GoToSymptomCheckLessonN = new DelegateCommand(null);
            }

            //Load Training TherapyPage
            if (SerializationManager.LoadTrainingPage(out TherapyPage? trainingPage, true))
            {
                TherapyPageViewModel vm = new(trainingPage!);
                ProgressUpdateManager.SetTrainingPage(vm);
                TherapyPages.Add(vm);

                this.ThoughtTestContainer = vm.Features[0] as ThoughtTestContainerViewModel;
                this.BodyScanContainer = vm.Features[1] as BodyScanContainerViewModel;
                this.BehaviourExperimentContainer = vm.Features[2] as BehaviourExperimentContainerViewModel;
                this.ResponseTrainingContainer = vm.Features[3] as ResponseTrainingContainerViewModel;
                this.RelaxationProcedureContainer = vm.Features[4] as RelaxationProcedureContainerViewModel;
            }

            GoToChatBotPageCmd = new(async(obj) =>
            {
                await Shell.Current.GoToAsync(nameof(ChatBotPage));
            });

            RewardManager.OnRewardXPChanged += UpdateLevelProgress;

            //Set initial value for UserInputGoal
            if(UserInputDataManager.GetUserInputData("Welcome4", out userInputGoal))
            {
                RaisePropertyChanged(nameof(UserInputGoal));
            }
            UserInputDataManager.userInputDataChangedEvent += UpdateUserInputData;
        }

        public void UpdateSymptomCheckLessonActiveStatus()
        {
            if(SymptomCheckLesson is not null)
            {
                SymptomCheckLesson.IsActive = DateOnly.FromDateTime(DateTime.UtcNow) > PreferenceManager.GetLastSymptomCheckDate();
            }
        }

        private void UpdateLevelProgress(object? s, EventArgs e)
        {
            base.RaisePropertyChanged(nameof(LevelProgress));
            base.RaisePropertyChanged(nameof(Level));
            base.RaisePropertyChanged(nameof(LevelProgressString));
        }

        private void UpdateUserInputData(object? s, EventArgs e)
        {
            if (!UserInputDataManager.GetUserInputData("Welcome4", out string text))
            {
                UserInputGoal = string.Empty;
            }
            UserInputGoal = text;
        }
    }
}
