using PeopleCurer.CustomEventArgs;
using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
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

        public ObservableCollection<SituationViewModel> Situations { get; }

        public DelegateCommand AddSituation { get; }
        public DelegateCommand DeleteSituation { get; }

        public BehaviourExperimentViewModel(BehaviourExperiment behaviourExperiment)
        {
            this.behaviourExperiment = behaviourExperiment;

            Situations = new ObservableCollection<SituationViewModel>();
            for (int i = 0; i < (behaviourExperiment.situations?.Count ?? 0); i++)
            {
                SituationViewModel vm = new SituationViewModel(behaviourExperiment.situations![i]);
                vm.OnSituationFinishEditEvent += SaveBehaviourExperiment;
                Situations.Add(vm);
            }

            AddSituation = new DelegateCommand((obj) =>
            {
                Situation newSituation = new Situation("neue Situation", "", [], [], 0, string.Empty, false);
                //Model
                behaviourExperiment.situations ??= [];
                behaviourExperiment.situations.Add(newSituation);
                //VM
                SituationViewModel vm = new SituationViewModel(newSituation);
                vm.OnSituationFinishEditEvent += SaveBehaviourExperiment;
                Situations.Add(vm);

                //Go to next page and allow editing
                Shell.Current.GoToAsync(nameof(SituationEditPage),
                    new Dictionary<string, object>
                    {
                        ["Situation"] = vm
                    });
            });

            DeleteSituation = new DelegateCommand((obj) =>
            {
                if (obj is SituationViewModel situation)
                {
                    //Model
                    behaviourExperiment.situations?.Remove(situation.situation);
                    //VM
                    Situations.Remove(situation);
                }
            });
        }

        private void SaveBehaviourExperiment(object? obj, EventArgs e)
        {
            //Save new data
            ProgressUpdateManager.UpdateTrainingData();
        }
    }

    public sealed class ThoughtTestContainerViewModel : FeatureViewModel
    {
        public readonly ThoughtTestContainer thoughtTestContainer;

        public ObservableCollection<ThoughtTestViewModel> ThoughtTests { get; }

        public DelegateCommand AddSituation { get; }
        public DelegateCommand DeleteSituation { get; }

        public ThoughtTestContainerViewModel(ThoughtTestContainer thoughtTestContainer)
        {
            this.thoughtTestContainer = thoughtTestContainer;

            ThoughtTests = new ObservableCollection<ThoughtTestViewModel>();
            for (int i = 0; i < (thoughtTestContainer.thoughtTests?.Count ?? 0); i++)
            {
                ThoughtTests.Add(new ThoughtTestViewModel(thoughtTestContainer.thoughtTests![i]));
            }

            AddSituation = new DelegateCommand((obj) =>
            {
                ThoughtTest newTest = new ThoughtTest("neuer Gedankentest", new Thought("neuer Gedanke",50), [], [], string.Empty, false);
                //Model
                thoughtTestContainer.thoughtTests ??= [];
                thoughtTestContainer.thoughtTests.Add(newTest);
                //VM
                ThoughtTestViewModel vm = new ThoughtTestViewModel(newTest);
                vm.OnTestFinishEditEvent += SaveThoughtTestExperiment;
                ThoughtTests.Add(vm);

                //Go to next page and allow editing
                Shell.Current.GoToAsync(nameof(ThoughtTestEditPage),
                    new Dictionary<string, object>
                    {
                        ["ThoughtTestVM"] = vm
                    });
            });

            DeleteSituation = new DelegateCommand((obj) =>
            {
                if (obj is ThoughtTestViewModel test)
                {
                    //Model
                    thoughtTestContainer.thoughtTests?.Remove(test.thoughtTest);
                    //VM
                    ThoughtTests.Remove(test);
                }
            });
        }

        private void SaveThoughtTestExperiment(object? obj, EventArgs e)
        {
            //Save new data
            ProgressUpdateManager.UpdateTrainingData();
        }
    }
}