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
}
