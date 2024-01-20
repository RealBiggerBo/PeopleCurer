using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    class MainPageViewModel : NotifyableBaseObject
    {
        public TherapyPageViewModel[] TherapyPages { get; } = [new TherapyPageViewModel("Text"), new TherapyPageViewModel("Questions")];

        public MainPageViewModel()
        {
        }
    }

    enum Pages
    {
        Info,
        Questions
    }
}
