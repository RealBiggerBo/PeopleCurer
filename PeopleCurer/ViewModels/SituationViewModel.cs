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
    public sealed class SituationViewModel : NotifyableBaseObject
    {
        public readonly Situation situation;

        public string SituationName 
        { 
            get => situation.situationName; 
            set
            {
                if(situation.situationName != value)
                {
                    situation.situationName = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string SituationTime
        {
            get => situation.situationTime;
            set
            {
                if (situation.situationTime != value)
                {
                    situation.situationTime = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<SituationFearViewModel> SituationFears { get; }

        public ObservableCollection<SafetyBehaviourViewModel> SituationSafetyBehaviours { get; }

        public int OverallFear 
        { 
            get => situation.overallFear; 
            set
            {
                if (situation.overallFear != value)
                {
                    situation.overallFear = value;
                    base.RaisePropertyChanged();
                }
            } 
        }

        public string Conclusion
        {
            get => situation.conclusion;
            set
            {
                if (situation.conclusion != value)
                {
                    situation.conclusion = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public bool IsFinished
        {
            get => situation.isFinished;
            set
            {
                if(value != situation.isFinished)
                {
                    situation.isFinished = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand GoToSituationPage { get; }
        public DelegateCommand AddSituationFear { get; }
        public DelegateCommand DeleteSituationFear { get; }
        public DelegateCommand AddSafetyBehaviour { get; }
        public DelegateCommand DeleteSafetyBehaviour { get; }
        public DelegateCommand SaveSituation { get; }
        public DelegateCommand FinishSituation { get; }
        public DelegateCommand CompletedSituation { get; }

        //public EventHandler OnSituationFinishEditEvent;

        public SituationViewModel(Situation situation)
        {
            this.situation = situation;

            SituationFears = new ObservableCollection<SituationFearViewModel>();
            for (int i = 0; i < situation.situationFears.Count; i++)
            {
                SituationFears.Add(new SituationFearViewModel(situation.situationFears[i]));
            }

            SituationSafetyBehaviours = new ObservableCollection<SafetyBehaviourViewModel>();
            for (int i = 0; i < situation.situationSafetyBehaviours.Count; i++)
            {
                SituationSafetyBehaviours.Add(new SafetyBehaviourViewModel(situation.situationSafetyBehaviours[i]));
            }

            GoToSituationPage = new DelegateCommand((obj) =>
            {
                if (this.IsFinished == false)
                {
                    Shell.Current.GoToAsync(nameof(SituationEditPage),
                        new Dictionary<string, object>
                        {
                            ["Situation"] = this
                        });
                }
                else
                {
                    Shell.Current.GoToAsync(nameof(SituationCompletedPage),
                        new Dictionary<string, object>
                        {
                            ["Situation"] = this
                        });
                }
            });
            AddSituationFear = new DelegateCommand((obj) =>
            {
                SituationFear newFear = new SituationFear("Neue Angst", 50, 50);

                //Model
                situation.situationFears ??= [];
                situation.situationFears.Add(newFear);
                //VM
                SituationFears.Add(new SituationFearViewModel(newFear));
            });
            DeleteSituationFear = new DelegateCommand((obj) =>
            {
                if (obj is SituationFearViewModel fear)
                {
                    //Model
                    situation.situationFears?.Remove(fear.situationFear);
                    //VM
                    SituationFears.Remove(fear);
                }
            });
            AddSafetyBehaviour = new DelegateCommand((obj) =>
            {
                SafetyBehaviour newBehav = new SafetyBehaviour("Neues Sicherheitsverhalten", 50);

                //Model
                situation.situationSafetyBehaviours ??= [];
                situation.situationSafetyBehaviours.Add(newBehav);
                //VM
                SituationSafetyBehaviours.Add(new SafetyBehaviourViewModel(newBehav));
            });
            DeleteSafetyBehaviour = new DelegateCommand((obj) =>
            {
                if (obj is SafetyBehaviourViewModel behav)
                {
                    //Model
                    situation.situationSafetyBehaviours?.Remove(behav.safetyBehaviour);
                    //VM
                    SituationSafetyBehaviours.Remove(behav);
                }
            });
            SaveSituation = new DelegateCommand((obj) =>
            {
                if(!(obj is string s && s == "False"))
                    ProgressUpdateManager.UpdateTrainingData();
                Shell.Current.GoToAsync("..");
            });
            FinishSituation = new DelegateCommand((obj) =>
            {
                Shell.Current.GoToAsync(nameof(SituationFinishPage),
                        new Dictionary<string, object>
                        {
                            ["Situation"] = this
                        });
            });
            CompletedSituation = new DelegateCommand((obj) =>
            {
                this.IsFinished = true;
                ProgressUpdateManager.UpdateTrainingData();
                Shell.Current.GoToAsync("../" + nameof(SituationCompletedPage),
                    new Dictionary<string, object>
                    {
                        ["Situation"] = this
                    });
            });
        }
    }

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
