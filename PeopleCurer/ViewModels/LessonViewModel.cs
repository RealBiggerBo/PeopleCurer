using PeopleCurer.CustomEventArgs;
using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using PeopleCurer.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public class LessonViewModel : NotifyableBaseObject
    {
        private readonly Lesson lesson;

        public string LessonName { get => lesson.lessonName; }

        public LessonPartViewModel[] LessonParts { get; }

        public bool IsActive
        {
            get => lesson.isActive;
            set
            {
                if(value != IsActive)
                {
                    lesson.isActive = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand GoToLessonPage { get; }
        public DelegateCommand GoToNextLessonPart { get; }
        public event EventHandler<IntEventArgs>? NextLessonPartEvent;

        //members to evaluate behaviourExperimentVM
        public LessonPartViewModel CurrentLessonPart 
        { 
            get => LessonParts[CurrentLessonPartIndex];
            set 
            { 
                if(value != LessonParts[CurrentLessonPartIndex])
                {
                    LessonParts[CurrentLessonPartIndex] = value;
                    base.RaisePropertyChanged();
                    base.RaisePropertyChanged(nameof(LessonParts));
                }
            }
        }

        private int currentLessonPartIndex;
        private int CurrentLessonPartIndex
        {
            get => currentLessonPartIndex;
            set
            {
                if(value != currentLessonPartIndex)
                {
                    currentLessonPartIndex = value;
                    base.RaisePropertyChanged();
                    base.RaisePropertyChanged(nameof(CurrentLessonPart));
                }
            }
        }


        public LessonViewModel(Lesson lesson)
        {
            this.lesson = lesson;

            //create vm from model
            LessonParts = new LessonPartViewModel[lesson.lessonParts.Length];
            for (int i = 0; i < lesson.lessonParts.Length; i++)
            {
                LessonParts[i] = GetLessonPartViewModelFromModel(lesson.lessonParts[i]);
            }

            CurrentLessonPartIndex = 0;

            GoToLessonPage = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(LessonPage),
                new Dictionary<string, object>
                {
                    ["Lesson"] = this
                }));
            GoToNextLessonPart = new DelegateCommand(async (obj) => await GoToNextLessonPartPage());
        }

        private static LessonPartViewModel GetLessonPartViewModelFromModel(in LessonPart lessonPart)
        {
            if (lessonPart.GetType() == typeof(Question))
            {
                return new QuestionViewModel((Question)lessonPart);
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
            else
                throw new ArgumentException("Error whilie trying to convert model to viewModel!");
        }

        public async Task GoToNextLessonPartPage()
        {
            if (CurrentLessonPartIndex + 1 >= LessonParts.Length)
            {
                //Check whether in SymptomCheck
                if (CurrentLessonPart is SymptomCheckQuestionViewModel)
                {
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
                    ProgressUpdateManager.UpdateSymptomCheckData(); //saves data
                }
                else
                {
                    ProgressUpdateManager.UpdateProgress(this);

                    CurrentLessonPartIndex = 0;//resets view back to lessonPart 0
                                               //Go back to page before
                }
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                NextLessonPartEvent?.Invoke(this, new IntEventArgs(CurrentLessonPartIndex + 1));
                CurrentLessonPartIndex += 1;

                if (LessonParts[CurrentLessonPartIndex] is EvaluationViewModel evalVM)
                {
                    int sum = 0;
                    for (int i = 0; i < CurrentLessonPartIndex; i++)
                    {
                        sum += LessonParts[i].GetAnswerValue();
                    }

                    evalVM.SetEvaluationSum(sum);
                }
            }
        }
    }
}
