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
    public sealed class ResponseTrainingViewModel : NotifyableBaseObject
    {
        private readonly ResponseTraining responseTraining;

        public string SituationDescription
        {
            get => responseTraining.situationDescription;
        }

        private bool isFinished = false;
        public bool IsFinished
        {
            get => isFinished;
            set
            {
                if(value != isFinished)
                {
                    isFinished = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Message> Messages { get; } = [];

        public DelegateCommand SendMessage { get; }

        public ResponseTrainingViewModel(ResponseTraining responseTraining)
        {
            this.responseTraining = responseTraining;

            SendMessage = new(async (obj) =>
            {
                if (!isFinished && obj is string msg)
                {
                    Messages.Add(new() { Content = msg, MessageType = MessageType.User });

                    await Task.Delay(1000);

                    Messages.Add(new() { Content = "Work in Progress", MessageType = MessageType.Bot });

                    if(Messages.Count > 10)
                    {
                        IsFinished = true;
                    }
                }
            });
        }
    }

    public class Message : NotifyableBaseObject
    {
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        User,
        Bot,
        System
    }
}
