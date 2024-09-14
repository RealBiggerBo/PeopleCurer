using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    internal class ChatBotPageViewModel : NotifyableBaseObject
    {
        public ObservableCollection<string> Messages { get; set; }

        public EventHandler? onMessageSendEvent;
        public EventHandler? onResponseReceiveEvent;

        public DelegateCommand SendCmd { get; }

        public ChatBotPageViewModel() 
        {
            Messages = [];

            SendCmd = new(async (obj) =>
            {
                if (obj is string message)
                {
                    onMessageSendEvent?.Invoke(null, EventArgs.Empty);

                    Messages.Add(message);

                    string response = await ChatbotManager.Talk(message);

                    Messages.Add(response);

                    onResponseReceiveEvent?.Invoke(null, EventArgs.Empty);
                }
            });
        }
    }
}
