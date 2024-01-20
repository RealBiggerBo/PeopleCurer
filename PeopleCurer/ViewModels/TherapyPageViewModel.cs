using PeopleCurer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    class TherapyPageViewModel
    {
        private readonly TherapyPage therapyPage;

        public string Name { get => therapyPage.name;}

        public TherapyPageViewModel(string name)
        {
            therapyPage = new TherapyPage(name);
        }
    }
}
