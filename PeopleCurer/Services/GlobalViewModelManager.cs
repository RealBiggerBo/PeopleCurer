using PeopleCurer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    static class GlobalViewModelManager
    {
        private static BehaviourExperimentContainerViewModel behaviourExperimentViewModel;
        public static BehaviourExperimentContainerViewModel BehaviourExperimentViewModel { get; private set; }
    }
}
