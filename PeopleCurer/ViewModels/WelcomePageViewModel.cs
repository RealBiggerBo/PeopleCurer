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
    public class WelcomePageViewModel
    {
        public string Welcome1 { get; set; }
        public string Welcome2 { get; set; }
        public string Welcome3 { get; set; }
        public string Welcome4 { get; set; }

        public DelegateCommand NavigateToMainPageCMD { get; }

        public WelcomePageViewModel() 
        {
            Welcome1 = string.Empty;
            Welcome2 = string.Empty;
            Welcome3 = string.Empty;
            Welcome4 = string.Empty;

            NavigateToMainPageCMD = new DelegateCommand(async (obj) => 
            {
                //save user input data
                UserInputDataManager.SetData(nameof(Welcome1), Welcome1, false);
                UserInputDataManager.SetData(nameof(Welcome2), Welcome2, false);
                UserInputDataManager.SetData(nameof(Welcome3), Welcome3, false);
                UserInputDataManager.SetData(nameof(Welcome4), Welcome4, true); //only save at last change

                //save status to preference manager
                PreferenceManager.CompleteWelcomePage();

                //switch view
                await Shell.Current.GoToAsync(".."); 
            });
        }
    }
}
