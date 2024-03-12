using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using System.Collections.ObjectModel;

namespace PeopleCurer.ViewModels
{
    class MainPageViewModel : NotifyableBaseObject
    {
        public ObservableCollection<TherapyPageViewModel> TherapyPages { get; }

        public MainPageViewModel()
        {
            //SerializationManager.RemoveCourses();

            TherapyPages = new ObservableCollection<TherapyPageViewModel>();

            if(SerializationManager.LoadCourses(out TherapyPage? page, true))
            {
                TherapyPageViewModel vm = new TherapyPageViewModel(page!);
                ProgressUpdateManager.SetCoursesPage(vm);
                TherapyPages.Add(vm);
            }

            TherapyPages.Add(new TherapyPageViewModel(new TherapyPage("Notes", [])));
        }
    }

    enum Pages
    {
        Info,
        Questions
    }
}
