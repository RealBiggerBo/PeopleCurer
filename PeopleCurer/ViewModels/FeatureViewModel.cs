using PeopleCurer.CustomEventArgs;
using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public abstract class FeatureViewModel : NotifyableBaseObject;

    public sealed class CourseViewModel : FeatureViewModel
    {
        private readonly Course course;

        public string CourseName { get => course.courseName; }
        public string Description { get => course.description; }

        public LessonViewModel[] Lessons { get; }

        public bool IsActive
        {
            get => course.isActive;
            set
            {
                if(value != IsActive)
                {
                    course.isActive = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand GoToCoursePage { get; }

        public CourseViewModel(Course course)
        {
            //set name and description
            this.course = course;

            //create behaviourExperimentVM vm 
            Lessons = new LessonViewModel[course.lessons.Length];
            for (int i = 0; i < course.lessons.Length; i++)
            {
                Lessons[i] = new LessonViewModel(course.lessons[i]);
            }

            //Routing command
            GoToCoursePage = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(CoursePage),
                new Dictionary<string, object>
                {
                    ["Course"] = this
                }));
        }
    }

    public sealed class BehaviourExperimentViewModel : FeatureViewModel
    {
        private readonly BehaviourExperiment behaviourExperiment;

        public DelegateCommand AddSituation { get; }

        [JsonInclude]
        public ObservableCollection<SituationViewModel> Situations { get; }

        public BehaviourExperimentViewModel(BehaviourExperiment behaviourExperiment)
        {
            this.behaviourExperiment = behaviourExperiment;

            Situations = new ObservableCollection<SituationViewModel>();
            for (int i = 0; i < (behaviourExperiment.situations?.Count ?? 0); i++)
            {
                Situations.Add(new SituationViewModel(behaviourExperiment.situations![i]));
            }

            AddSituation = new DelegateCommand((obj) =>
            {
                SituationViewModel vm = new SituationViewModel(new Situation("neue Situation", "", [], [], 0, string.Empty));
                Situations.Add(vm);

                //Go to next page and allow editing
            });
        }
    }
}