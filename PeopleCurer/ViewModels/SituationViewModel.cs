using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using PeopleCurer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    //public sealed class SituationViewModel : NotifyableBaseObject
    //{
    //    public readonly Situation situation;

    //    public string SituationName 
    //    { 
    //        get => situation.situationName; 
    //        set
    //        {
    //            if(situation.situationName != value)
    //            {
    //                situation.situationName = value;
    //                base.RaisePropertyChanged();
    //            }
    //        }
    //    }

    //    public string SituationTime
    //    {
    //        get => situation.situationTime;
    //        set
    //        {
    //            if (situation.situationTime != value)
    //            {
    //                situation.situationTime = value;
    //                base.RaisePropertyChanged();
    //            }
    //        }
    //    }

    //    public ObservableCollection<SituationFearViewModel> SituationFears { get; }

    //    public SafetyBehaviourViewModel SituationSafetyBehaviour { get; }

    //    public int OverallFear 
    //    { 
    //        get => situation.overallFear; 
    //        set
    //        {
    //            if (situation.overallFear != value)
    //            {
    //                situation.overallFear = value;
    //                base.RaisePropertyChanged();
    //            }
    //        } 
    //    }

    //    public string Conclusion
    //    {
    //        get => situation.conclusion;
    //        set
    //        {
    //            if (situation.conclusion != value)
    //            {
    //                situation.conclusion = value;
    //                base.RaisePropertyChanged();
    //            }
    //        }
    //    }

    //    public bool IsFinished
    //    {
    //        get => situation.isCompleted;
    //        set
    //        {
    //            if(value != situation.isCompleted)
    //            {
    //                situation.isCompleted = value;
    //                base.RaisePropertyChanged();
    //            }
    //        }
    //    }

    //    public DelegateCommand GoToSituationPage { get; }
    //    public DelegateCommand FinishSituation { get; }

    //    public LessonViewModel situationLessonCreate;
    //    public LessonViewModel? situationLessonFinish;

    //    public SituationViewModel(Situation situation)
    //    {
    //        this.situation = situation;

    //        SituationFears = new ObservableCollection<SituationFearViewModel>();
    //        for (int i = 0; i < situation.situationFears.Count; i++)
    //        {
    //            SituationFears.Add(new SituationFearViewModel(situation.situationFears[i]));
    //        }

    //        SituationSafetyBehaviour = new(situation.situationSafetyBehaviour);

    //        NormalLesson situationLessonCreateModel = new("Verhaltensexperiment", "Erstelle ein Verhaltensexperiment.", false, LessonType.Practice, SaveFrequency.Never, 0, new SituationLessonPartCreate1(situation), new SituationLessonPartCreate2(situation), new SituationLessonPartCreate3(situation));
    //        //situationLessonCreate = new(situationLessonCreateModel);
    //        situationLessonCreate.UpdateLessonPartEvent += (obj, e) => UpdateDataFromLesson();

    //        GoToSituationPage = new DelegateCommand((obj) =>
    //        {
    //            if (!this.IsFinished)
    //            {
    //                situationLessonCreate.GoToLessonPage.Execute(ColorTheme.Green);
    //            }
    //            else
    //            {
    //                Shell.Current.GoToAsync(nameof(SituationCompletedPage),
    //                    new Dictionary<string, object>
    //                    {
    //                        ["Situation"] = this,
    //                        ["DoReward"] = false,
    //                    });
    //            }
    //        });
    //        FinishSituation = new DelegateCommand((obj) =>
    //        {
    //            NormalLesson situationLessonFinishModel = new("Verhaltensexperiment", "Beende ein Verhaltensexperiment.", false, LessonType.Practice, SaveFrequency.Never, 0, new SituationLessonPartFinish1(situation), new SituationLessonPartFinish2(situation), new SituationLessonPartFinish3(situation));
    //            situationLessonFinish = new NormalLessonViewModel(situationLessonFinishModel);
    //            situationLessonFinish.UpdateLessonPartEvent += (obj, e) => UpdateDataFromLesson();

    //            situationLessonFinish.GoToLessonPage.Execute(ColorTheme.Green);
    //        });
    //    }

    //    private void UpdateDataFromLesson()
    //    {
    //        base.RaisePropertyChanged(nameof(SituationName));

    //        //if (situationLessonFinish?.IsCompleted ?? false)
    //        //{
    //        //    IsFinished = true;
    //        //    base.RaisePropertyChanged(nameof(SituationFears));
    //        //    Shell.Current.GoToAsync(nameof(SituationCompletedPage),
    //        //        new Dictionary<string, object>
    //        //        {
    //        //            ["Situation"] = this,
    //        //            ["DoReward"] = true,
    //        //        });
    //        //}

    //        ProgressUpdateManager.SaveTrainingData();
    //    }
    //}

    public sealed class SituationFearViewModel : NotifyableBaseObject
    {
        public readonly SituationFear situationFear;

        public string FearName
        {
            get => situationFear.fearName;
            set
            {
                if(value != situationFear.fearName)
                {
                    situationFear.fearName = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public int FearProbability
        {
            get => situationFear.fearProbability;
            set
            {
                if(value != situationFear.fearProbability)
                {
                    situationFear.fearProbability = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public int ActualFearStrength
        {
            get => situationFear.actualFearStrength;
            set
            {
                if(situationFear.actualFearStrength != value)
                {
                    situationFear.actualFearStrength = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public SituationFearViewModel(SituationFear situationFear)
        {
            this.situationFear = situationFear;
        }
    }

    public sealed class SafetyBehaviourViewModel : NotifyableBaseObject
    {
        public readonly SafetyBehaviour safetyBehaviour;

        public string SafetyBehaviourName
        {
            get => safetyBehaviour.safetyBehaviourName;
            set
            {
                if (value != safetyBehaviour.safetyBehaviourName)
                {
                    safetyBehaviour.safetyBehaviourName = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public int SafetyBehaviourAmount
        {
            get => safetyBehaviour.safetyBehaviourAmount;
            set
            {
                if (value != safetyBehaviour.safetyBehaviourAmount)
                {
                    safetyBehaviour.safetyBehaviourAmount = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public SafetyBehaviourViewModel(SafetyBehaviour safetyBehaviour)
        {
            this.safetyBehaviour = safetyBehaviour;
        }
    }
}
