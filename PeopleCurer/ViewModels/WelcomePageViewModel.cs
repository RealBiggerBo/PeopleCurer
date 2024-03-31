using PeopleCurer.MVVMHelpers;
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
        public DelegateCommand NavigateToMainPageCMD { get; }

        public WelcomePageViewModel() 
        {
            NavigateToMainPageCMD = new DelegateCommand(async (obj) => 
            {
                await Shell.Current.GoToAsync(".."); 
            });
        }
    }
}
