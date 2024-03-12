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
    public sealed class AnswerViewModel : NotifyableBaseObject
    {
        private readonly Answer answer;

        public event EventHandler<AnswerEventArgs>? AnswerChanged;

        public string AnswerText { get => answer.answerText; }
        public int AnswerValue { get => answer.answerValue; }
        public bool IsChosen
        {
            get => answer.isChosen;
            set
            {
                if (value != answer.isChosen)
                {
                    answer.isChosen = value;
                    AnswerChanged?.Invoke(this, new AnswerEventArgs(value));
                    base.RaisePropertyChanged();
                }
            }
        }

        public AnswerViewModel(Answer answer)
        {
            this.answer = answer;
        }

    }
}
