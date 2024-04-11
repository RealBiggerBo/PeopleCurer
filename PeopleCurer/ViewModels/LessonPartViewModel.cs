using PeopleCurer.CustomEventArgs;
using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using System;
using System.Collections.Generic;
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
                if(value !=  canGoToNextLessonPart)
                {
                    canGoToNextLessonPart = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public virtual int GetAnswerValue()
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
            else if(textPart.GetType() == typeof(TextBox))
            {
                return new TextBoxViewModel((TextBox)textPart);
            }
            else
                throw new ArgumentException("Error whilie trying to convert model to viewModel!");
        }
    }

    public sealed class QuestionViewModel : LessonPartViewModel
    {
        private readonly Question question;

        public string Issue { get => question.issue; }

        public AnswerViewModel[] Answers { get; }

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
            for (int i = 0;i < Answers.Length; i++)
            {
                val = val || Answers[i].IsChosen;
            }
            CanGoToNextLessonPart = val;
        }

        public override int GetAnswerValue()
        {
            for (int i = 0; i<Answers.Length; i++) 
            {
                if (Answers[i].IsChosen) return Answers[i].AnswerValue;
            }
            return 0;
        }
    }

    public sealed class EvaluationViewModel : LessonPartViewModel
    {
        private readonly Evaluation evaluation;
        private const string errorMsg = "ERROR";

        private string evaluationResult;
        public string EvaluationResult 
        { 
            get => evaluationResult;
            private set
            { 
                if(evaluationResult != value)
                {
                    evaluationResult = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public EvaluationViewModel(Evaluation evaluation)
        {
            this.evaluation = evaluation;

            EvaluationResult = errorMsg;
        }

        public void SetEvaluationSum(int sum)
        {
            int currentEval = int.MaxValue;
            for (int i = 0; i < evaluation.evaluationResults.Count; i++) 
            {
                if(sum <= evaluation.evaluationResults.ElementAt(i).Key && currentEval > evaluation.evaluationResults.ElementAt(i).Key)
                {
                    currentEval = evaluation.evaluationResults.ElementAt(i).Key;
                }
            }

            //Failed to find a fitting evaluation
            if (currentEval == int.MaxValue)
            {
                EvaluationResult = errorMsg;
            }
            else
            {
                EvaluationResult = evaluation.evaluationResults[currentEval];
            }
        }
    }

    public sealed class SymptomCheckQuestionViewModel : LessonPartViewModel
    {
        private readonly SymptomCheckQuestion symptomCheckQuestion;

        public string Issue { get => symptomCheckQuestion.issue; }
        public string Description { get => symptomCheckQuestion.description; }

        public string LowText { get =>  symptomCheckQuestion.lowText; }
        public string HighText { get => symptomCheckQuestion.highText; }

        public int AnswerValue
        {
            get => symptomCheckQuestion.answerValue;
            set
            {
                if(value != symptomCheckQuestion.answerValue)
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
                if(value != symptomCheckQuestion.data)
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
}
