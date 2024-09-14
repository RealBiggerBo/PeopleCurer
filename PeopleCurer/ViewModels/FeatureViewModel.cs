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

        public ColorTheme CourseColor { get => course.courseColor; }

        public LessonViewModel[] Lessons { get; }

        public bool IsActive
        {
            get => course.isActive;
            set
            {
                if(value != course.isActive)
                {
                    course.isActive = value;
                    VisitFeature?.RaiseCanExecuteChanged();
                    base.RaisePropertyChanged();
                }
            }
        }

        public CourseViewModel(Course course)
        {
            //set name and description
            this.course = course;

            //create behaviourExperimentVM vm 
            Lessons = new LessonViewModel[course.lessons.Length];
            for (int i = 0; i < course.lessons.Length; i++)
            {
                //Lessons[i] = new LessonViewModel(course.lessons[i]);
                Lessons[i] = LessonModelToVM(course.lessons[i]);
            }

            //Routing command
            base.VisitFeature = new DelegateCommand(async (obj) => await Shell.Current.GoToAsync(nameof(CoursePage),
                new Dictionary<string, object>
                {
                    ["Course"] = this
                }), (obj) => IsActive);
        }

        private static LessonViewModel LessonModelToVM(Lesson lesson)
        {
            if(lesson is NormalLesson normalLesson)
            {
                return new NormalLessonViewModel(normalLesson);
            }
            else if(lesson is SituationLesson situationLesson)
            {
                return new SituationLessonViewModel(situationLesson);
            }
            else if(lesson is ThoughtTestLesson thoughtTestLesson)
            {
                return new ThoughtTestLessonViewModel(thoughtTestLesson);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    public sealed class BehaviourExperimentContainerViewModel : FeatureViewModel
    {
        private readonly BehaviourExperimentContainer behaviourExperiment;

        public ObservableCollection<SituationLessonViewModel> Situations { get; }

        public bool CanBeVisited
        {
            get => VisitFeature?.CanExecute(null) ?? false;
        }

        public DelegateCommand AddSituation { get; }
        public DelegateCommand DeleteSituation { get; }

        public BehaviourExperimentContainerViewModel(BehaviourExperimentContainer behaviourExperiment)
        {
            this.behaviourExperiment = behaviourExperiment;

            Situations = [];
            for (int i = 0; i < (behaviourExperiment.situations?.Count ?? 0); i++)
            {
                SituationLessonViewModel vm = new SituationLessonViewModel(behaviourExperiment.situations![i]);
                Situations.Add(vm);
            }

            base.VisitFeature = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(BehaviourExperimentPage),
            new Dictionary<string, object>
            {
                ["BehaviourExperimentVM"] = this
            }),
            (obj) => behaviourExperiment.requiredCourseProgress == -1 || behaviourExperiment.requiredCourseProgress <= PreferenceManager.GetCourseProgress());
            VisitFeature.CanExecuteChanged += (o, e) => base.RaisePropertyChanged(nameof(CanBeVisited));

            AddSituation = new DelegateCommand((obj) =>
            {
                SituationLesson newSituation = new();
                //Model
                behaviourExperiment.situations ??= [];
                behaviourExperiment.situations.Add(newSituation);
                //VM
                SituationLessonViewModel vm = new(newSituation);
                Situations.Add(vm);

                //save changes
                ProgressUpdateManager.SaveTrainingData();

                //Go to next page and allow editing
                vm.GoToLessonPage.Execute(null);
            });
            DeleteSituation = new DelegateCommand((obj) =>
            {
                if (obj is SituationLessonViewModel situation)
                {
                    //Model
                    behaviourExperiment.situations?.Remove(situation.lesson);
                    //VM
                    Situations.Remove(situation);

                    //save changes
                    ProgressUpdateManager.SaveTrainingData();
                }
            });
        }
    }

    public sealed class ThoughtTestContainerViewModel : FeatureViewModel
    {
        public readonly ThoughtTestContainer thoughtTestContainer;

        public ObservableCollection<ThoughtTestLessonViewModel> ThoughtTests { get; }

        public bool CanBeVisited 
        {
            get => VisitFeature?.CanExecute(null) ?? false;
        }

        public DelegateCommand AddThoughtTest { get; }
        public DelegateCommand DeleteThoughtTest { get; }

        public ThoughtTestContainerViewModel(ThoughtTestContainer thoughtTestContainer)
        {
            this.thoughtTestContainer = thoughtTestContainer;

            ThoughtTests = [];
            for (int i = 0; i < (thoughtTestContainer.thoughtTests?.Count ?? 0); i++)
            {
                ThoughtTests.Add(new(thoughtTestContainer.thoughtTests![i]));
            }

            base.VisitFeature = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(ThoughtTestContainerPage),
                    new Dictionary<string, object>
                    {
                        ["ThoughtTestContainerVM"] = this
                    }),
            (obj) => thoughtTestContainer.requiredCourseProgress == -1 || thoughtTestContainer.requiredCourseProgress <= PreferenceManager.GetCourseProgress());
            VisitFeature.CanExecuteChanged += (o, e) => base.RaisePropertyChanged(nameof(CanBeVisited));

            AddThoughtTest = new DelegateCommand((obj) =>
            {
                ThoughtTestLesson newTest = new();
                //Model
                thoughtTestContainer.thoughtTests ??= [];
                thoughtTestContainer.thoughtTests.Add(newTest);
                //VM
                ThoughtTestLessonViewModel vm = new ThoughtTestLessonViewModel(newTest);
                ThoughtTests.Add(vm);

                //save changes
                ProgressUpdateManager.SaveTrainingData();

                //Go to next page and allow editing
                vm.GoToLessonPage.Execute(null);

            });

            DeleteThoughtTest = new DelegateCommand((obj) =>
            {
                if (obj is ThoughtTestLessonViewModel test)
                {
                    //Model
                    thoughtTestContainer.thoughtTests?.Remove(test.lesson);
                    //VM
                    ThoughtTests.Remove(test);

                    //save changes
                    ProgressUpdateManager.SaveTrainingData();
                }
            });
        }

        private void SaveThoughtTestExperiment(object? obj, EventArgs e)
        {
            //Save new data
            ProgressUpdateManager.SaveTrainingData();
        }
    }

    public sealed class RelaxationProcedureContainerViewModel : FeatureViewModel
    {
        public readonly RelaxationProcedureContainer relaxationProcedureContainer;

        public RelaxationProcedureViewModel[] RelaxationProcedures { get; }

        public bool CanBeVisited
        {
            get => VisitFeature?.CanExecute(null) ?? false;
        }

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
            VisitFeature.CanExecuteChanged += (o, e) => base.RaisePropertyChanged(nameof(CanBeVisited));
        }
    }

    public sealed class BodyScanContainerViewModel : FeatureViewModel
    {
        private readonly BodyScanContainer bodyScanContainer;

        public bool CanBeVisited
        {
            get => VisitFeature?.CanExecute(null) ?? false;
        }

        public BodyScanContainerViewModel(BodyScanContainer bodyScanContainer)
        {
            this.bodyScanContainer = bodyScanContainer;

            base.VisitFeature = new DelegateCommand(
                async (obj) => await Shell.Current.GoToAsync(nameof(BodyScanPage)),
                (obj) => bodyScanContainer.requiredCourseProgress == -1 || bodyScanContainer.requiredCourseProgress <= PreferenceManager.GetCourseProgress());
            VisitFeature.CanExecuteChanged += (o, e) => base.RaisePropertyChanged(nameof(CanBeVisited));

        }
    }

    public sealed class ResponseTrainingContainerViewModel : FeatureViewModel
    {
        private readonly ResponseTrainingContainer responseTrainingContainer;

        public bool CanBeVisited
        {
            get => VisitFeature?.CanExecute(null) ?? false;
        }

        public ResponseTrainingContainerViewModel(ResponseTrainingContainer responseTrainingContainer)
        {
            this.responseTrainingContainer = responseTrainingContainer;

            base.VisitFeature = new DelegateCommand(
                async (obj) =>
                {
                    if(responseTrainingContainer.responseTrainings is null)
                        return;
                    int chosenResponseTrainingIndex = Random.Shared.Next(0, responseTrainingContainer.responseTrainings.Length);

                    await Shell.Current.GoToAsync(nameof(ResponseTrainingPage),
                        new Dictionary<string, object>
                        {
                            ["ResponseTrainingVM"] = new ResponseTrainingViewModel(responseTrainingContainer.responseTrainings[chosenResponseTrainingIndex])
                        });
                },
                (obj) => responseTrainingContainer.requiredCourseProgress == -1 || responseTrainingContainer.requiredCourseProgress <= PreferenceManager.GetCourseProgress());
            VisitFeature.CanExecuteChanged += (o, e) => base.RaisePropertyChanged(nameof(CanBeVisited));
        }
    }
}