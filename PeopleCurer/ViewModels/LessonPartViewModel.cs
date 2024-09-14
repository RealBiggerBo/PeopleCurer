using PeopleCurer.CustomEventArgs;
using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public abstract class LessonPartViewModel : NotifyableBaseObject
    {
        private bool canGoToNextLessonPart = true;
        public bool CanGoToNextLessonPart
        {
            get => canGoToNextLessonPart;
            set
            {
                if (value != canGoToNextLessonPart)
                {
                    canGoToNextLessonPart = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public virtual int GetAAnswerValue()
        {
            return 0;
        }
    }

    public sealed class InfoPageViewModel : LessonPartViewModel
    {
        private readonly InfoPage infoPage;

        public TextPartViewModel[] TextParts { get; }

        public InfoPageViewModel(InfoPage infoPage)
        {
            this.infoPage = infoPage;

            TextParts = new TextPartViewModel[infoPage.textParts.Length];
            for (int i = 0; i < TextParts.Length; i++)
            {
                TextParts[i] = GetTextPartViewModelFromModel(infoPage.textParts[i]);
            }
        }

        private static TextPartViewModel GetTextPartViewModelFromModel(in TextPart textPart)
        {
            if (textPart.GetType() == typeof(TextBlock))
            {
                return new TextBlockViewModel((TextBlock)textPart);
            }
            else if (textPart.GetType() == typeof(Enumeration))
            {
                return new EnumerationViewModel((Enumeration)textPart);
            }
            else if (textPart.GetType() == typeof(TextBox))
            {
                return new TextBoxViewModel((TextBox)textPart);
            }
            else if (textPart.GetType() == typeof(UserInputTextBlock))
            {
                return new UserInputTextBlockViewModel((UserInputTextBlock)textPart);
            }
            else if (textPart.GetType() == typeof(FearCircleDiagram))
            {
                return new FearCircleDiagramViewModel((FearCircleDiagram)textPart);
            }
            else if (textPart is ListText listText)
            {
                return new ListTextViewModel(listText);
            }
            else
                throw new ArgumentException("Error whilie trying to convert model to viewModel!");
        }
    }

    public sealed class QuestionViewModel : LessonPartViewModel
    {
        private readonly Question question;

        public string QuestionText { get => question.questionText; }

        public AnswerViewModel[] Answers { get; }
        public EventHandler? SelectedAnswerChanged;

        public QuestionViewModel(Question question)
        {
            this.question = question;

            bool canGoToNextLessonPart = false;

            Answers = new AnswerViewModel[question.answers.Length];
            for (int i = 0; i < Answers.Length; i++)
            {
                Answers[i] = new AnswerViewModel(question.answers[i]);
                Answers[i].AnswerChanged += AnswersChanged;

                canGoToNextLessonPart = canGoToNextLessonPart || Answers[i].IsChosen;
            }

            CanGoToNextLessonPart = canGoToNextLessonPart;
        }

        private void AnswersChanged(object? sender, AnswerEventArgs e)
        {
            bool val = false;
            for (int i = 0; i < Answers.Length; i++)
            {
                val = val || Answers[i].IsChosen;
            }
            CanGoToNextLessonPart = val;

            SelectedAnswerChanged?.Invoke(this, e);
        }

        public override int GetAAnswerValue()
        {
            for (int i = 0; i < Answers.Length; i++)
            {
                if (Answers[i].IsChosen) return Answers[i].IsCorrect ? 1 : 0;
            }
            return 0;
        }
    }

    public sealed class EvaluationViewModel(Evaluation evaluation) : LessonPartViewModel
    {
        private readonly Evaluation evaluation = evaluation;

        private ObservableCollection<QuestionViewModel> questionsToEvaluate = [];
        public ObservableCollection<QuestionViewModel> QuestionsToEvaluate
        {
            get => questionsToEvaluate;
            set
            {
                if (questionsToEvaluate != value)
                {
                    questionsToEvaluate = value;
                    base.RaisePropertyChanged();
                }
            }
        }
    }

    public sealed class SymptomCheckQuestionViewModel : LessonPartViewModel
    {
        private readonly SymptomCheckQuestion symptomCheckQuestion;

        public string Issue { get => symptomCheckQuestion.issue; }
        public string Description { get => symptomCheckQuestion.description; }

        public string LowText { get => symptomCheckQuestion.lowText; }
        public string HighText { get => symptomCheckQuestion.highText; }

        public int AnswerValue
        {
            get => symptomCheckQuestion.answerValue;
            set
            {
                if (value != symptomCheckQuestion.answerValue)
                {
                    symptomCheckQuestion.answerValue = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public Dictionary<DateOnly, int> Data
        {
            get => symptomCheckQuestion.data;
            set
            {
                if (value != symptomCheckQuestion.data)
                {
                    symptomCheckQuestion.data = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public SymptomCheckQuestionViewModel(SymptomCheckQuestion symptomCheckQuestion)
        {
            this.symptomCheckQuestion = symptomCheckQuestion;
        }
    }

    public sealed class ThoughtTestLessonPart1ViewModel(ThoughtTestLessonViewModel vm) : LessonPartViewModel
    {
        public ThoughtTestLessonViewModel ThoughtTestLessonVM { get; } = vm;
    }
    public sealed class ThoughtTestLessonPart2ViewModel : LessonPartViewModel
    {
        public ThoughtTestLessonViewModel ThoughtTestLessonVM { get; }

        public DelegateCommand AddEmotion { get; }
        public DelegateCommand DeleteEmotion { get; }

        public ThoughtTestLessonPart2ViewModel(ThoughtTestLessonViewModel vm)
        {
            ThoughtTestLessonVM = vm;

            AddEmotion = new DelegateCommand((obj) => ThoughtTestLessonVM.AddEmotion());
            DeleteEmotion = new DelegateCommand((obj) =>
            {
                if (obj is EmotionViewModel emotion)
                {
                    ThoughtTestLessonVM.RemoveEmotion(emotion);
                }
            });
        }
    }
    public sealed class ThoughtTestLessonPart3ViewModel : LessonPartViewModel
    {
        public ThoughtTestLessonViewModel ThoughtTestLessonVM { get; }

        public DelegateCommand AddAlternateThought { get; }
        public DelegateCommand DeleteAlternateThought { get; }

        public ThoughtTestLessonPart3ViewModel(ThoughtTestLessonViewModel vm)
        {
            ThoughtTestLessonVM = vm;

            AddAlternateThought = new DelegateCommand((obj) => ThoughtTestLessonVM.AddAlternateThought());
            DeleteAlternateThought = new DelegateCommand((obj) =>
            {
                if (obj is ThoughtViewModel thought)
                {
                    ThoughtTestLessonVM.RemoveAlternateThought(thought);
                }
            });
        }
    }
    public sealed class ThoughtTestLessonPart4ViewModel(ThoughtTestLessonViewModel vm) : LessonPartViewModel
    {
        public ThoughtTestLessonViewModel ThoughtTestLessonVM { get; } = vm;
    }

    public sealed class SituationLessonPartCreate1ViewModel(SituationLessonViewModel vm) : LessonPartViewModel
    {
        public SituationLessonViewModel SituationLessonVM { get; } = vm;
    }
    public sealed class SituationLessonPartCreate2ViewModel : LessonPartViewModel
    {
        public SituationLessonViewModel SituationLessonVM { get; }

        public DelegateCommand AddSituationFear { get; }
        public DelegateCommand DeleteSituationFear { get; }

        public SituationLessonPartCreate2ViewModel(SituationLessonViewModel vm)
        {
            this.SituationLessonVM = vm;

            AddSituationFear = new DelegateCommand((obj) => SituationLessonVM.AddSituationFear());
            DeleteSituationFear = new DelegateCommand((obj) =>
            {
                if (obj is SituationFearViewModel fear)
                {
                    SituationLessonVM.RemoveSituationFear(fear);
                }
            });
        }
    }
    public sealed class SituationLessonPartCreate3ViewModel(SituationLessonViewModel vm) : LessonPartViewModel
    {
        public SituationLessonViewModel SituationLessonVM { get; } = vm;
    }
    public sealed class SituationLessonPartFinish1ViewModel(SituationLessonViewModel vm) : LessonPartViewModel
    {
        public SituationLessonViewModel SituationLessonVM { get; } = vm;
    }
    public sealed class SituationLessonPartFinish2ViewModel(SituationLessonViewModel vm) : LessonPartViewModel
    {
        public SituationLessonViewModel SituationLessonVM { get; } = vm;
    }
    public sealed class SituationLessonPartFinish3ViewModel(SituationLessonViewModel vm) : LessonPartViewModel
    {
        public SituationLessonViewModel SituationLessonVM { get; } = vm;
    }
    public sealed class SituationLessonPartFinish4ViewModel(SituationLessonViewModel vm) : LessonPartViewModel
    {
        public SituationLessonViewModel SituationLessonVM { get; } = vm;
    }
}
