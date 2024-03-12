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
        private readonly TherapyPage therapyPage;

        public string Name { get => therapyPage.name;}

        public FeatureViewModel[] Features { get; }

        public TherapyPageViewModel(TherapyPage therapyPage)
        {
            this.therapyPage = therapyPage;

            Features = new FeatureViewModel[therapyPage.features.Length];
            for (int i = 0; i < therapyPage.features.Length; i++)
            {
                Features[i] = GetFeatureViewModelFromModel(therapyPage.features[i]);
            }
        }

        private FeatureViewModel GetFeatureViewModelFromModel(in Feature feature)
        {
            if (feature.GetType() == typeof(Course))
            {
                return new CourseViewModel((Course)feature);
            }
            else
                throw new ArgumentException("Error whilie trying to convert model to viewModel!");
        }



        public TherapyPage GetPage() => therapyPage;
    }
}
