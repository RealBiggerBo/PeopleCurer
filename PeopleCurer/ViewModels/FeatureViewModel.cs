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
    public abstract class FeatureViewModel : NotifyableBaseObject
    {
        public virtual DelegateCommand? VisitFeature { get; protected set; }
    }

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
                if(value != course.isActive)
                {
                    course.isActive = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        //public DelegateCommand GoToCoursePage { get; }

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
            base.VisitFeature = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(CoursePage),
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
                Situations.Add(vm);
            }

            base.VisitFeature = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(BehaviourExperimentPage),
            new Dictionary<string, object>
            {
                ["BehaviourExperimentVM"] = this
            }),
            (obj) => behaviourExperiment.requiredCourseProgress == -1 || behaviourExperiment.requiredCourseProgress <= PreferenceManager.GetCourseProgress());

            AddSituation = new DelegateCommand((obj) =>
            {
                Situation newSituation = new Situation("neue Situation", "", [], [], 0, string.Empty, false);
                //Model
                behaviourExperiment.situations ??= [];
                behaviourExperiment.situations.Add(newSituation);
                //VM
                SituationViewModel vm = new SituationViewModel(newSituation);
                Situations.Add(vm);

                //save changes
                ProgressUpdateManager.UpdateTrainingData();

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

                    //save changes
                    ProgressUpdateManager.UpdateTrainingData();
                }
            });
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

            base.VisitFeature = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(ThoughtTestContainerPage),
                    new Dictionary<string, object>
                    {
                        ["ThoughtTestContainerVM"] = this
                    }),
            (obj) => thoughtTestContainer.requiredCourseProgress == -1 || thoughtTestContainer.requiredCourseProgress <= PreferenceManager.GetCourseProgress());

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

                //save changes
                ProgressUpdateManager.UpdateTrainingData();
            });

            DeleteSituation = new DelegateCommand((obj) =>
            {
                if (obj is ThoughtTestViewModel test)
                {
                    //Model
                    thoughtTestContainer.thoughtTests?.Remove(test.thoughtTest);
                    //VM
                    ThoughtTests.Remove(test);

                    //save changes
                    ProgressUpdateManager.UpdateTrainingData();
                }
            });
        }

        private void SaveThoughtTestExperiment(object? obj, EventArgs e)
        {
            //Save new data
            ProgressUpdateManager.UpdateTrainingData();
        }
    }

    public sealed class RelaxationProcedureContainerViewModel : FeatureViewModel
    {
        public readonly RelaxationProcedureContainer relaxationProcedureContainer;

        public RelaxationProcedureViewModel[] RelaxationProcedures { get; }

        public RelaxationProcedureContainerViewModel(RelaxationProcedureContainer relaxationProcedureContainer)
        {
            this.relaxationProcedureContainer = relaxationProcedureContainer;

            RelaxationProcedures = new RelaxationProcedureViewModel[relaxationProcedureContainer.relaxationProcedures?.Count ?? 0];
            for (int i = 0; i < (relaxationProcedureContainer.relaxationProcedures?.Count ?? 0); i++)
            {
                RelaxationProcedures[i] = new RelaxationProcedureViewModel(relaxationProcedureContainer.relaxationProcedures![i]);
            }

            base.VisitFeature = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(RelaxationProcedureContainerPage),
                    new Dictionary<string, object>
                    {
                        ["RelaxationProcedureContainerVM"] = this
                    }),
            (obj) => relaxationProcedureContainer.requiredCourseProgress == -1 || relaxationProcedureContainer.requiredCourseProgress <= PreferenceManager.GetCourseProgress());
        }
    }

    public sealed class StrengthsCourseViewModel : FeatureViewModel
    {
        public readonly StrengthsCourse strengthsCourse;

        public LessonViewModel StrengthsLesson { get; }
        public LessonViewModel SuccessMomentsLesson { get; }
        public LessonViewModel TrainingSuccessLesson { get; }

        public StrengthsCourseViewModel(StrengthsCourse strengthsCourse)
        {
            this.strengthsCourse = strengthsCourse;

            StrengthsLesson = new LessonViewModel(strengthsCourse.strengthsLesson);
            SuccessMomentsLesson = new LessonViewModel(strengthsCourse.successMomentsLesson);
            TrainingSuccessLesson = new LessonViewModel(strengthsCourse.trainingSuccessLesson);
        }
    }
}