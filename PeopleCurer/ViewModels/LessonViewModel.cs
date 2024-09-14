using PeopleCurer.CustomEventArgs;
using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using PeopleCurer.Views;
using System.Collections.ObjectModel;

namespace PeopleCurer.ViewModels
{
    public abstract class LessonViewModel : NotifyableBaseObject, IConvertible
    {
        public abstract string LessonName { get; }
        public abstract string LessonDescription { get; }

        public abstract ColorTheme ColorTheme { get; protected set; }

        public abstract LessonType LessonType { get; }

        public abstract LessonPartViewModel[] LessonParts { get; }

        public LessonPartViewModel CurrentLessonPart
        {
            get => LessonParts[CurrentLessonPartIndex];
            set
            {
                if (value != LessonParts[CurrentLessonPartIndex])
                {
                    LessonParts[CurrentLessonPartIndex] = value;
                    GoToNextLessonPart.RaiseCanExecuteChanged();
                    base.RaisePropertyChanged();
                    base.RaisePropertyChanged(nameof(LessonParts));
                }
            }
        }

        private int currentLessonPartIndex;
        protected int CurrentLessonPartIndex
        {
            get => currentLessonPartIndex;
            set
            {
                if (value != currentLessonPartIndex)
                {
                    currentLessonPartIndex = value;
                    GoToNextLessonPart.RaiseCanExecuteChanged();
                    base.RaisePropertyChanged();
                    base.RaisePropertyChanged(nameof(CurrentLessonPart));
                }
            }
        }

        public abstract bool IsActive { get; set; }

        public abstract bool IsCompleted { get; set; }

        public abstract DelegateCommand GoToLessonPage { get; }
        public DelegateCommand GoToNextLessonPart { get; }
        public DelegateCommand GoToPreviousLessonPart { get; }
        public event EventHandler<IntEventArgs>? UpdateLessonPartEvent;

        protected LessonViewModel()
        {
            GoToNextLessonPart = new(async (obj) => await GoToNextLessonPartPage(), (obj) => CurrentLessonPart.CanGoToNextLessonPart);
            GoToPreviousLessonPart = new(async (obj) => await GoToPreviousLessonPartPage());
        }

        protected void RaiseUpdateLessonPartEvent(IntEventArgs e)
        {
            UpdateLessonPartEvent?.Invoke(this, e);
        }

        public virtual async Task GoToNextLessonPartPage() { }
        public virtual async Task GoToPreviousLessonPartPage() { }

        public abstract void AddReward();

        public abstract void PrepareLesson();

        public abstract object ToType(Type conversionType, IFormatProvider? provider);
        public TypeCode GetTypeCode() => throw new NotImplementedException();
        public bool ToBoolean(IFormatProvider? provider) => throw new NotImplementedException();
        public byte ToByte(IFormatProvider? provider) => throw new NotImplementedException();
        public char ToChar(IFormatProvider? provider) => throw new NotImplementedException();
        public DateTime ToDateTime(IFormatProvider? provider) => throw new NotImplementedException();
        public decimal ToDecimal(IFormatProvider? provider) => throw new NotImplementedException();
        public double ToDouble(IFormatProvider? provider) => throw new NotImplementedException();
        public short ToInt16(IFormatProvider? provider) => throw new NotImplementedException();
        public int ToInt32(IFormatProvider? provider) => throw new NotImplementedException();
        public long ToInt64(IFormatProvider? provider) => throw new NotImplementedException();
        public sbyte ToSByte(IFormatProvider? provider) => throw new NotImplementedException();
        public float ToSingle(IFormatProvider? provider) => throw new NotImplementedException();
        public string ToString(IFormatProvider? provider) => throw new NotImplementedException();
        public ushort ToUInt16(IFormatProvider? provider) => throw new NotImplementedException();
        public uint ToUInt32(IFormatProvider? provider) => throw new NotImplementedException();
        public ulong ToUInt64(IFormatProvider? provider) => throw new NotImplementedException();
    }

    public class NormalLessonViewModel : LessonViewModel
    {
        private readonly NormalLesson lesson;

        public override string LessonName { get => lesson.lessonName; }

        public override string LessonDescription { get => lesson.lessonDescription; }

        public override ColorTheme ColorTheme 
        {
            get => lesson.colorTheme;
            protected set
            {
                if (lesson.colorTheme != value)
                {
                    lesson.colorTheme = value;

                    base.RaisePropertyChanged();
                }
            }
        }

        public override LessonPartViewModel[] LessonParts { get; }

        public override bool IsActive
        {
            get => lesson.isActive;
            set
            {
                if(value != IsActive)
                {
                    lesson.isActive = value;
                    this.GoToLessonPage.RaiseCanExecuteChanged();
                    base.RaisePropertyChanged();
                }
            }
        }

        public override bool IsCompleted
        {
            get => lesson.isCompleted;
            set
            {
                if (value != lesson.isCompleted)
                {
                    lesson.isCompleted = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public override LessonType LessonType { get => lesson.lessonType; }

        public override DelegateCommand GoToLessonPage { get; }

        public NormalLessonViewModel(NormalLesson lesson)
        {
            this.lesson = lesson;

            //create vm from model
            LessonParts = new LessonPartViewModel[lesson.lessonParts.Length];
            for (int i = 0; i < lesson.lessonParts.Length; i++)
            {
                LessonParts[i] = GetLessonPartViewModelFromModel(lesson.lessonParts[i]);
            }

            CurrentLessonPartIndex = 0;

            GoToLessonPage = new DelegateCommand(async (obj) =>
            {
                if (obj is ColorTheme theme)
                {
                    this.ColorTheme = theme;
                }

                if (this.lesson.showStartPage)
                {
                    await Shell.Current.GoToAsync(nameof(LessonStartPage), new Dictionary<string, object>
                    {
                        [nameof(LessonStartPage.Lesson)] = this,
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync(nameof(LessonPage), new Dictionary<string, object>
                    {
                        [nameof(LessonPage.Lesson)] = this
                    });
                }
            }, (obj) => IsActive);

            //GoToNextLessonPart = new (async (obj) => await GoToNextLessonPartPage(), (obj) => CurrentLessonPart.CanGoToNextLessonPart);
            //GoToPreviousLessonPart = new(async (obj) => await GoToPreviousLessonPartPage());
        }

        private LessonPartViewModel GetLessonPartViewModelFromModel(in LessonPart lessonPart)
        {
            if (lessonPart.GetType() == typeof(Question))
            {
                QuestionViewModel vm = new QuestionViewModel((Question)lessonPart);

                vm.SelectedAnswerChanged += (obj, e) => GoToNextLessonPart.RaiseCanExecuteChanged();

                return vm;
            }
            else if (lessonPart.GetType() == typeof(Evaluation))
            {
                return new EvaluationViewModel((Evaluation)lessonPart);
            }
            else if(lessonPart.GetType() == typeof(InfoPage))
            {
                return new InfoPageViewModel((InfoPage)lessonPart);
            }
            else if(lessonPart.GetType() == typeof(SymptomCheckQuestion))
            {
                return new SymptomCheckQuestionViewModel((SymptomCheckQuestion)lessonPart);
            }
            //else if(lessonPart is ThoughtTestLessonPart1 p1)
            //{
            //    return new ThoughtTestLessonPart1ViewModel(p1);
            //}
            //else if (lessonPart is ThoughtTestLessonPart2 p2)
            //{
            //    return new ThoughtTestLessonPart2ViewModel(p2);
            //}
            //else if (lessonPart is ThoughtTestLessonPart3 p3)
            //{
            //    return new ThoughtTestLessonPart3ViewModel(p3);
            //}
            //else if (lessonPart is ThoughtTestLessonPart4 p4)
            //{
            //    return new ThoughtTestLessonPart4ViewModel(p4);
            //}
            //else if(lessonPart is SituationLessonPartCreate1 spc1)
            //{
            //    return new SituationLessonPartCreate1ViewModel(spc1);
            //}
            //else if (lessonPart is SituationLessonPartCreate2 spc2)
            //{
            //    return new SituationLessonPartCreate2ViewModel(spc2);
            //}
            //else if (lessonPart is SituationLessonPartCreate3 spc3)
            //{
            //    return new SituationLessonPartCreate3ViewModel(spc3);
            //}
            //else if (lessonPart is SituationLessonPartFinish1 spf1)
            //{
            //    return new SituationLessonPartFinish1ViewModel(spf1);
            //}
            //else if (lessonPart is SituationLessonPartFinish2 spf2)
            //{
            //    return new SituationLessonPartFinish2ViewModel(spf2);
            //}
            //else if (lessonPart is SituationLessonPartFinish3 spf3)
            //{
            //    return new SituationLessonPartFinish3ViewModel(spf3);
            //}
            else
                throw new ArgumentException("Error whilie trying to convert model to viewModel!");
        }

        public override async Task GoToNextLessonPartPage()
        {
            if (CurrentLessonPartIndex + 1 >= LessonParts.Length)
            {
                //Check whether in SymptomCheck
                if (CurrentLessonPart is SymptomCheckQuestionViewModel)
                {
                    await Shell.Current.GoToAsync($"..//{nameof(RewardPage)}", new Dictionary<string, object>
                    {
                        ["Lesson"] = this,
                    });

                    for (int i = 0; i < LessonParts.Length; i++)
                    {
                        if (LessonParts[i] is SymptomCheckQuestionViewModel symptomCheckQuestionVM)
                        {
                            //Add new entry to data
                            symptomCheckQuestionVM.Data ??= new Dictionary<DateOnly, int>();

                            DateOnly key = DateOnly.FromDateTime(DateTime.UtcNow);
                            int val = symptomCheckQuestionVM.AnswerValue;

                            if (symptomCheckQuestionVM.Data.ContainsKey(key))
                            {
                                symptomCheckQuestionVM.Data[key] = val;
                            }
                            else
                            {
                                symptomCheckQuestionVM.Data.Add(key, val);
                            }

                            //Reset vm
                            symptomCheckQuestionVM.AnswerValue = 50;
                        }
                    }

                    //save current date as last symptomCheck Date
                    PreferenceManager.UpdateLastSymptomCheckDate();
                    ProgressUpdateManager.SaveSymptomCheckData(); //saves data
                }
                else
                {
                    if (!IsCompleted)
                    {
                        IsCompleted = true;

                        if (lesson.lessonReward > 0)
                        {
                            await Shell.Current.GoToAsync($"..//{nameof(RewardPage)}", new Dictionary<string, object>
                            {
                                ["Lesson"] = this,
                            });
                        }
                        else
                        {
                            await Shell.Current.GoToAsync("..");
                        }

                        if (lesson.saveFrequency == SaveFrequency.OnCompletion)
                        {
                            ProgressUpdateManager.UpdateProgress(this);
                        }
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                }

                if (lesson.saveFrequency == SaveFrequency.Always)
                {
                    ProgressUpdateManager.UpdateProgress(this);
                }

                //TODO: -add save values to every module
                //      -add reference to next lesson?
            }
            else
            {
                CurrentLessonPartIndex += 1;
            }

            base.RaiseUpdateLessonPartEvent(new IntEventArgs(CurrentLessonPartIndex));
            //base.UpdateLessonPartEvent?.Invoke(this, new IntEventArgs(CurrentLessonPartIndex));
        }
        public override async Task GoToPreviousLessonPartPage()
        {
            if (CurrentLessonPartIndex <= 0)
            {
                //Go back to previous shell page
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                CurrentLessonPartIndex -= 1;

                base.RaiseUpdateLessonPartEvent(new IntEventArgs(CurrentLessonPartIndex));
                //UpdateLessonPartEvent?.Invoke(this, new IntEventArgs(CurrentLessonPartIndex));
            }
        }

        public override void AddReward()
        {
            RewardManager.AddRewardXP(lesson.lessonReward);
        }

        public override void PrepareLesson()
        {
            CurrentLessonPartIndex = 0;

            for (int i = 0; i < LessonParts.Length; i++)
            {
                if (LessonParts[i] is EvaluationViewModel evalVM)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (LessonParts[j] is QuestionViewModel question)
                        {
                            evalVM.QuestionsToEvaluate.Add(question);
                        }
                    }
                }
            }
        }

        public override object ToType(Type conversionType, IFormatProvider? provider)
        {
            return (object)this;
        }
    }

    public class SituationLessonViewModel : LessonViewModel
    {
        public readonly SituationLesson lesson;

        public override string LessonName { get => "Verhaltensexperiment"; }
        public override string LessonDescription { get => "BESCHREIBUNG HARD CODE"; }

        public override ColorTheme ColorTheme
        {
            get => ColorTheme.Green;
            protected set { }
        }

        public override LessonType LessonType
        {
            get => LessonType.Practice;
        }

        private readonly LessonPartViewModel[] editLessonParts;
        private readonly LessonPartViewModel[] finishLessonParts;
        private LessonPartViewModel[] lessonParts;
        public override LessonPartViewModel[] LessonParts { get => lessonParts; }

        public override bool IsActive
        {
            get => lesson.isActive;
            set
            {
                if (value != lesson.isActive)
                {
                    lesson.isActive = value;
                    this.GoToLessonPage.RaiseCanExecuteChanged();
                    base.RaisePropertyChanged();
                }
            }
        }

        public override bool IsCompleted
        {
            get => lesson.isCompleted;
            set
            {
                if (value != lesson.isCompleted)
                {
                    lesson.isCompleted = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string SituationName
        {
            get => lesson.situationName;
            set
            {
                if (value != lesson.situationName) 
                { 
                    lesson.situationName = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        public string SituationTime
        {
            get => lesson.situationTime;
            set
            {
                if (value != lesson.situationTime)
                {
                    lesson.situationTime = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public int OverallFear
        {
            get => lesson.overallFear;
            set
            {
                if (lesson.overallFear != value)
                {
                    lesson.overallFear = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string Conclusion
        {
            get => lesson.conclusion;
            set
            {
                if (lesson.conclusion != value)
                {
                    lesson.conclusion = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<SituationFearViewModel> situationFears;
        public ObservableCollection<SituationFearViewModel> SituationFears
        {
            get => situationFears;
        }

        private SafetyBehaviourViewModel safetyBehaviour;
        public SafetyBehaviourViewModel SafetyBehaviour
        {
            get => safetyBehaviour;
        }

        public override DelegateCommand GoToLessonPage { get; }

        public SituationLessonViewModel(SituationLesson lesson)
        {
            this.lesson = lesson;

            editLessonParts = [
                new SituationLessonPartCreate1ViewModel(this),
                new SituationLessonPartCreate2ViewModel(this),
                new SituationLessonPartCreate3ViewModel(this),
            ];
            finishLessonParts = [
                new SituationLessonPartFinish1ViewModel(this),
                new SituationLessonPartFinish2ViewModel(this),
                new SituationLessonPartFinish3ViewModel(this),
                new SituationLessonPartFinish4ViewModel(this),
            ];

            lessonParts = editLessonParts;
            base.RaisePropertyChanged(nameof(LessonParts));

            situationFears = [];
            for (int i = 0; i < lesson.situationFears.Count; i++)
            {
                SituationFears.Add(new SituationFearViewModel(lesson.situationFears[i]));
            }

            safetyBehaviour = new(lesson.situationSafetyBehaviour);

            GoToLessonPage = new DelegateCommand(async (obj) =>
            {
                if (obj is ColorTheme theme)
                {
                    this.ColorTheme = theme;
                }

                const string finish = "Abschließen";
                const string summary = "Zusammenfassung";
                const string edit = "Bearbeiten";
                const string delete = "Löschen";
                const string back = "Zurück";

                if (lesson.isOngoing)
                {
                    string result;
                    if (IsCompleted)
                    {
                        result = await Shell.Current.DisplayActionSheet(SituationName, back, null, summary, delete);
                    }
                    else
                    {
                        result = await Shell.Current.DisplayActionSheet(SituationName, back, null, finish, edit, delete);
                    }

                    switch (result)
                    {
                        case finish:
                            lessonParts = finishLessonParts;
                            base.RaisePropertyChanged(nameof(LessonParts));
                            await Shell.Current.GoToAsync(nameof(LessonPage), new Dictionary<string, object>
                            {
                                [nameof(LessonPage.Lesson)] = this
                            });
                            break;
                        case summary:
                            //TODO: go to extra SituationSummaryPage
                            await Shell.Current.GoToAsync(nameof(SituationSummaryPage), new Dictionary<string, object>
                            {
                                [nameof(SituationSummaryPage.SituationLessonVM)] = this
                            });
                            break;
                        case edit:
                            lessonParts = editLessonParts;
                            base.RaisePropertyChanged(nameof(LessonParts));

                            await Shell.Current.GoToAsync(nameof(LessonPage), new Dictionary<string, object>
                            {
                                [nameof(LessonPage.Lesson)] = this
                            });
                            break;
                        case delete:
                            //TODO: ...
                            //behaviourExperimentVM?.DeleteSituation.Execute(situationVM);
                            break;
                        default:
                            break;
                    } 
                }
                else
                {
                    lesson.isOngoing = true;
                    await Shell.Current.GoToAsync(nameof(LessonPage), new Dictionary<string, object>
                    {
                        [nameof(LessonPage.Lesson)] = this
                    });
                }
            }, (obj) => IsActive);
        }

        public void AddSituationFear()
        {

/* Nicht gemergte Änderung aus Projekt "PeopleCurer (net8.0-windows10.0.19041.0)"
Vor:
            Emotion newFear = new Emotion("Neue Angst", 50, 50);
Nach:
            SituationFear newFear = new Emotion("Neue Angst", 50, 50);
*/
            SituationFear newFear = new SituationFear("Neue Angst", 50, 50);

            //Model
            lesson.situationFears ??= [];
            lesson.situationFears.Add(newFear);
            //VM
            SituationFears.Add(new SituationFearViewModel(newFear));
        }
        public void RemoveSituationFear(SituationFearViewModel fear)
        {
            //Model
            lesson.situationFears?.Remove(fear.situationFear);
            //VM
            SituationFears.Remove(fear);
        }

        public override void AddReward()
        {
            RewardManager.AddRewardXP(1000);
        }

        public override async Task GoToNextLessonPartPage()
        {
            if (CurrentLessonPartIndex + 1 >= LessonParts.Length)
            {
                //Check whether in SymptomCheck
                if (CurrentLessonPart is SymptomCheckQuestionViewModel)
                {
                    await Shell.Current.GoToAsync($"..//{nameof(RewardPage)}", new Dictionary<string, object>
                    {
                        ["Lesson"] = this,
                    });

                    for (int i = 0; i < LessonParts.Length; i++)
                    {
                        if (LessonParts[i] is SymptomCheckQuestionViewModel symptomCheckQuestionVM)
                        {
                            //Add new entry to data
                            symptomCheckQuestionVM.Data ??= new Dictionary<DateOnly, int>();

                            DateOnly key = DateOnly.FromDateTime(DateTime.UtcNow);
                            int val = symptomCheckQuestionVM.AnswerValue;

                            if (symptomCheckQuestionVM.Data.ContainsKey(key))
                            {
                                symptomCheckQuestionVM.Data[key] = val;
                            }
                            else
                            {
                                symptomCheckQuestionVM.Data.Add(key, val);
                            }

                            //Reset vm
                            symptomCheckQuestionVM.AnswerValue = 50;
                        }
                    }

                    //save current date as last symptomCheck Date
                    PreferenceManager.UpdateLastSymptomCheckDate();
                    ProgressUpdateManager.SaveSymptomCheckData(); //saves data
                }
                else
                {
                    if (!IsCompleted && lessonParts == finishLessonParts)
                    {
                        IsCompleted = true;

                        if (true)
                        {
                            await Shell.Current.GoToAsync($"..//{nameof(RewardPage)}", new Dictionary<string, object>
                            {
                                ["Lesson"] = this,
                            });
                        }
                        else
                        {
                            await Shell.Current.GoToAsync("..");
                        }

                        if (false)
                        {
                            ProgressUpdateManager.UpdateProgress(this);
                        }
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                }

                if (true)
                {
                    ProgressUpdateManager.UpdateProgress(this);
                }

                //TODO: -add save values to every module
                //      -add reference to next lesson?
            }
            else
            {
                CurrentLessonPartIndex += 1;
            }

            base.RaiseUpdateLessonPartEvent(new IntEventArgs(CurrentLessonPartIndex));
            //base.UpdateLessonPartEvent?.Invoke(this, new IntEventArgs(CurrentLessonPartIndex));
        }
        public override async Task GoToPreviousLessonPartPage()
        {
            if (CurrentLessonPartIndex <= 0)
            {
                //Go back to previous shell page
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                CurrentLessonPartIndex -= 1;

                base.RaiseUpdateLessonPartEvent(new IntEventArgs(CurrentLessonPartIndex));
                //UpdateLessonPartEvent?.Invoke(this, new IntEventArgs(CurrentLessonPartIndex));
            }
        }

        public override object ToType(Type conversionType, IFormatProvider? provider)
        {
            return (object)this;
        }

        public override void PrepareLesson()
        {
            CurrentLessonPartIndex = 0;
        }
    }

    public class ThoughtTestLessonViewModel : LessonViewModel
    {
        public readonly ThoughtTestLesson lesson;

        public override string LessonName { get => "Gedankentest"; }
        public override string LessonDescription { get => "BESCHREIBUNG HARD CODE"; }

        public override ColorTheme ColorTheme
        {
            get => ColorTheme.Blue;
            protected set { }
        }

        public override LessonType LessonType
        {
            get => LessonType.Practice;
        }

        public override LessonPartViewModel[] LessonParts
        {
            get => [
                new ThoughtTestLessonPart1ViewModel(this),
                new ThoughtTestLessonPart2ViewModel(this),
                new ThoughtTestLessonPart3ViewModel(this),
                new ThoughtTestLessonPart4ViewModel(this)];
        }

        public override bool IsActive
        {
            get => lesson.isActive;
            set
            {
                if (value != lesson.isActive)
                {
                    lesson.isActive = value;
                    this.GoToLessonPage.RaiseCanExecuteChanged();
                    base.RaisePropertyChanged();
                }
            }
        }

        public override bool IsCompleted
        {
            get => lesson.isCompleted;
            set
            {
                if (value != lesson.isCompleted)
                {
                    lesson.isCompleted = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string ThoughtTestName
        {
            get => lesson.thoughtTestName;
            set
            {
                if(lesson.thoughtTestName != value)
                {
                    lesson.thoughtTestName = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        public string SituationDescription
        {
            get => lesson.situationDescription;
            set
            {
                if (lesson.situationDescription != value)
                {
                    lesson.situationDescription = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ThoughtViewModel MainThought { get; }

        public ObservableCollection<ThoughtViewModel> AlternateThoughts { get; }

        public ObservableCollection<EmotionViewModel> Emotions { get; }

        public string Conclusion
        {
            get => lesson.conclusion;
            set
            {
                if (lesson.conclusion != value)
                {
                    lesson.conclusion = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public List<Thought> ChartData
        {
            get => GetChartData();
        }

        public override DelegateCommand GoToLessonPage { get; }

        public ThoughtTestLessonViewModel(ThoughtTestLesson lesson)
        {
            this.lesson = lesson;

            MainThought = new(lesson.mainThought);

            AlternateThoughts = [];
            for (int i = 0; i < lesson.alternateThoughts.Count; i++)
            {
                AlternateThoughts.Add(new(lesson.alternateThoughts[i]));
            }

            Emotions = [];
            for (int i = 0; i < lesson.emotions.Count; i++)
            {
                Emotions.Add(new(lesson.emotions[i]));
            }

            GoToLessonPage = new DelegateCommand(async (obj) =>
            {
                if (obj is ColorTheme theme)
                {
                    this.ColorTheme = theme;
                }

                if (IsCompleted)
                {
                    const string finish = "Abschließen";
                    const string summary = "Zusammenfassung";
                    const string edit = "Bearbeiten";
                    const string delete = "Löschen";
                    const string back = "Zurück";

                    string result = await Shell.Current.DisplayActionSheet(ThoughtTestName, back, null, summary, delete);

                    switch (result)
                    {
                        case finish:
                            base.RaisePropertyChanged(nameof(LessonParts));
                            await Shell.Current.GoToAsync(nameof(LessonPage), new Dictionary<string, object>
                            {
                                [nameof(LessonPage.Lesson)] = this
                            });
                            break;
                        case summary:
                            //TODO: go to extra SituationSummaryPage
                            await Shell.Current.GoToAsync(nameof(ThoughtTestCompletedPage), new Dictionary<string, object>
                            {
                                [nameof(ThoughtTestCompletedPage.ThoughtTestVM)] = this
                            });
                            break;
                        case edit:
                            base.RaisePropertyChanged(nameof(LessonParts));

                            await Shell.Current.GoToAsync(nameof(LessonPage), new Dictionary<string, object>
                            {
                                [nameof(LessonPage.Lesson)] = this
                            });
                            break;
                        case delete:
                            //TODO: ...
                            //behaviourExperimentVM?.DeleteSituation.Execute(situationVM);
                            break;
                        default:
                            break;
                    } 
                }
                else
                {
                    await Shell.Current.GoToAsync(nameof(LessonPage), new Dictionary<string, object>
                    {
                        [nameof(LessonPage.Lesson)] = this
                    });
                }
            }, (obj) => IsActive);
        }

        private List<Thought> GetChartData()
        {
            List<Thought> data = [];

            data.Add(MainThought.thought);

            for (int i = 0; i < AlternateThoughts.Count; i++)
            {
                data.Add(AlternateThoughts[i].thought);
            }

            return data;
        }

        public void AddEmotion()
        {

/* Nicht gemergte Änderung aus Projekt "PeopleCurer (net8.0-windows10.0.19041.0)"
Vor:
            Emotion newThought = new Emotion("Neue Emotion", 50, 50);
Nach:
            SituationFear newThought = new Emotion("Neue Emotion", 50, 50);
*/
            Emotion newEmotion = new Emotion("Neue Emotion");

            //Model
            lesson.emotions ??= [];
            lesson.emotions.Add(newEmotion);
            //VM
            Emotions.Add(new EmotionViewModel(newEmotion));
        }
        public void RemoveEmotion(EmotionViewModel emotion)
        {
            //Model
            lesson.emotions?.Remove(emotion.emotion);
            //VM
            Emotions.Remove(emotion);
        }
        public void AddAlternateThought()
        {
            Thought newThought = new("Neuer Gedanke", 50);

            //Model
            lesson.alternateThoughts ??= [];
            lesson.alternateThoughts.Add(newThought);
            //VM
            AlternateThoughts.Add(new(newThought));
        }
        public void RemoveAlternateThought(ThoughtViewModel thought)
        {
            //Model
            lesson.alternateThoughts?.Remove(thought.thought);
            //VM
            AlternateThoughts.Remove(thought);
        }

        public override void AddReward()
        {
            RewardManager.AddRewardXP(1000);
        }

        public override async Task GoToNextLessonPartPage()
        {
            if (CurrentLessonPartIndex + 1 >= LessonParts.Length)
            {
                //Check whether in SymptomCheck
                if (CurrentLessonPart is SymptomCheckQuestionViewModel)
                {
                    await Shell.Current.GoToAsync($"..//{nameof(RewardPage)}", new Dictionary<string, object>
                    {
                        ["Lesson"] = this,
                    });

                    for (int i = 0; i < LessonParts.Length; i++)
                    {
                        if (LessonParts[i] is SymptomCheckQuestionViewModel symptomCheckQuestionVM)
                        {
                            //Add new entry to data
                            symptomCheckQuestionVM.Data ??= new Dictionary<DateOnly, int>();

                            DateOnly key = DateOnly.FromDateTime(DateTime.UtcNow);
                            int val = symptomCheckQuestionVM.AnswerValue;

                            if (symptomCheckQuestionVM.Data.ContainsKey(key))
                            {
                                symptomCheckQuestionVM.Data[key] = val;
                            }
                            else
                            {
                                symptomCheckQuestionVM.Data.Add(key, val);
                            }

                            //Reset vm
                            symptomCheckQuestionVM.AnswerValue = 50;
                        }
                    }

                    //save current date as last symptomCheck Date
                    PreferenceManager.UpdateLastSymptomCheckDate();
                    ProgressUpdateManager.SaveSymptomCheckData(); //saves data
                }
                else
                {
                    if (!IsCompleted)
                    {
                        IsCompleted = true;

                        if (true)
                        {
                            await Shell.Current.GoToAsync($"..//{nameof(RewardPage)}", new Dictionary<string, object>
                            {
                                ["Lesson"] = this,
                            });
                        }
                        else
                        {
                            await Shell.Current.GoToAsync("..");
                        }

                        if (false)
                        {
                            ProgressUpdateManager.UpdateProgress(this);
                        }
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                }

                if (true)
                {
                    ProgressUpdateManager.UpdateProgress(this);
                }

                //TODO: -add save values to every module
                //      -add reference to next lesson?
            }
            else
            {
                if (LessonParts[CurrentLessonPartIndex + 1] is ThoughtTestLessonPart4ViewModel p4VM)
                {
                    base.RaisePropertyChanged(nameof(ChartData));
                }

                CurrentLessonPartIndex += 1;
            }

            base.RaiseUpdateLessonPartEvent(new IntEventArgs(CurrentLessonPartIndex));
            //base.UpdateLessonPartEvent?.Invoke(this, new IntEventArgs(CurrentLessonPartIndex));
        }
        public override async Task GoToPreviousLessonPartPage()
        {
            if (CurrentLessonPartIndex <= 0)
            {
                //Go back to previous shell page
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                CurrentLessonPartIndex -= 1;

                base.RaiseUpdateLessonPartEvent(new IntEventArgs(CurrentLessonPartIndex));
                //UpdateLessonPartEvent?.Invoke(this, new IntEventArgs(CurrentLessonPartIndex));
            }
        }

        public override object ToType(Type conversionType, IFormatProvider? provider)
        {
            return (object)this;
        }

        public override void PrepareLesson()
        {
            CurrentLessonPartIndex = 0;
        }
    }
}
