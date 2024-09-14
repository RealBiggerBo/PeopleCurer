using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public sealed class TherapyPageViewModel : NotifyableBaseObject
    {
        public readonly TherapyPage therapyPage;

        public string Name { get => therapyPage.name;}

        public string Description { get => therapyPage.description;}

        public FeatureViewModel[] Features { get; }

        public TherapyPageViewModel(TherapyPage therapyPage)
        {
            this.therapyPage = therapyPage;

            Features = new FeatureViewModel[therapyPage.features?.Length ?? 0];
            for (int i = 0; i < (therapyPage.features?.Length ?? 0); i++)
            {
                Features[i] = GetFeatureViewModelFromModel(therapyPage.features![i]);
            }
        }

        private FeatureViewModel GetFeatureViewModelFromModel(in Feature feature)
        {
            if (feature is Course course)
            {
                return new CourseViewModel(course);
            }
            else if(feature is BehaviourExperimentContainer behExp)
            {
                return new BehaviourExperimentContainerViewModel(behExp);
            }
            else if(feature is ThoughtTestContainer thoughtTest)
            {
                return new ThoughtTestContainerViewModel(thoughtTest);
            }
            else if(feature is RelaxationProcedureContainer relaxProcedure)
            {
                return new RelaxationProcedureContainerViewModel(relaxProcedure);
            }
            else if(feature is BodyScanContainer bodyScanContainer)
            {
                return new BodyScanContainerViewModel(bodyScanContainer);
            }
            else if(feature is ResponseTrainingContainer responseTrainingContainer)
            {
                return new ResponseTrainingContainerViewModel(responseTrainingContainer);
            }
            else
                throw new ArgumentException("Error whilie trying to convert model to viewModel!");
        }

        [Obsolete("Make readonly field public instead")]
        public TherapyPage GetPage() => therapyPage;
    }
}
