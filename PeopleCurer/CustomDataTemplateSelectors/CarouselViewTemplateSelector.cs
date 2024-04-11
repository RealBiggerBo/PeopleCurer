using PeopleCurer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.CustomDataTemplateSelectors
{
    sealed class CarouselViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MainPageTemplate { get; set; }
        public DataTemplate TrainingPageTemplate { get; set; }
        public DataTemplate StrengthsPageTemplate { get; set; }
        public DataTemplate StatisticsPageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is int index)
            {
                //int index = GetTherapyPageIndex(vm, carousel);
                switch (index)
                {
                    case 0:
                        return MainPageTemplate;
                    case 1:
                        return TrainingPageTemplate;
                    case 2:
                        return StrengthsPageTemplate;
                    case 3:
                        return StatisticsPageTemplate;
                    default:
                        break;
                }
                return MainPageTemplate;
            }
            return MainPageTemplate;
        }

        private static int GetTherapyPageIndex(TherapyPageViewModel therapyPageVM, CarouselView carouselView)
        {
            ObservableCollection<TherapyPageViewModel> pages = ((MainPageViewModel)carouselView.BindingContext).TherapyPages;

            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i] == therapyPageVM)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
