using PeopleCurer.Models;
using PeopleCurer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    public static class ProgressUpdateManager
    {
        public static EventHandler? OnSymptomCheckQuestionCompleted { get; set; }

        private static TherapyPageViewModel? coursesPage;
        private static NormalLesson? symptomCheckLesson;
        private static TherapyPageViewModel? trainingPage;

        public static void SetCoursesPage(TherapyPageViewModel page)
        {
            coursesPage = page;
        }
        public static void SetSymptomCheckPage(NormalLesson lesson)
        {
            symptomCheckLesson = lesson;
        }
        public static void SetTrainingPage(TherapyPageViewModel page)
        {
            trainingPage = page;
        }

        public static void UpdateProgress(in LessonViewModel finishedLesson)
        {
            //check if lesson is in any course
            for (int i = 0; i < coursesPage?.Features.Length; i++)
            {
                if (coursesPage.Features[i] is CourseViewModel courseVM)
                {
                    for (int j = 0; j < courseVM.Lessons.Length; j++)
                    {
                        if(finishedLesson == courseVM.Lessons[j])
                        {
                            //found finished lesson
                            if(courseVM.Lessons.Length > j + 1)
                            {
                                //activate next lesson
                                courseVM.Lessons[j + 1].IsActive = true;
                                UpdateCoursesProgress(i + 1, j + 1);
                                SaveCourses();
                            }
                            else if(coursesPage.Features.Length > i + 1 && coursesPage.Features[i + 1] is CourseViewModel nextCourse)
                            {
                                //activate next course
                                nextCourse.IsActive = true;
                                UpdateCoursesProgress(i + 1, j + 1);
                                SaveCourses();
                            }

                            return;
                        }
                    }
                }
            }

            //TODO: check if lesson is training lesson
            for (int i = 0; i < trainingPage?.Features.Length; i++)
            {
                if (finishedLesson is SituationLessonViewModel situationVM &&
                    trainingPage.Features[i] is BehaviourExperimentContainerViewModel behaviourContainerVM)
                {
                    for (int j = 0; j < behaviourContainerVM.Situations.Count; j++)
                    {
                        if(situationVM == behaviourContainerVM.Situations[j])
                        {
                            SaveTrainingData();
                            return;
                        }
                    }
                }
                else if(finishedLesson is ThoughtTestLessonViewModel thoughtTestVM &&
                    trainingPage.Features[i] is ThoughtTestContainerViewModel thoughtTestContainerVM)
                {
                    for (int j = 0; j < thoughtTestContainerVM.ThoughtTests.Count; j++)
                    {
                        if (thoughtTestVM == thoughtTestContainerVM.ThoughtTests[j])
                        {
                            SaveTrainingData();
                            return;
                        }
                    }
                }
            }

            //TODO: check if lesson is symptomcheck




            //Courses needs Saving if there is an TextBox or any other input form
            //bool needsSaving = false;
            //for (int i = 0; i < finishedLesson.LessonParts.Length; i++)
            //{
            //    if (finishedLesson.LessonParts[i] is InfoPageViewModel infoPage)
            //    {
            //        for (int j = 0; j < infoPage.TextParts.Length; j++)
            //        {
            //            if (infoPage.TextParts[j] is TextBoxViewModel)
            //            {
            //                needsSaving = true;
            //                break;
            //            }
            //            else if (infoPage.TextParts[j] is FearCircleDiagramViewModel)
            //            {
            //                needsSaving = true;
            //                break;
            //            }
            //            else if (infoPage.TextParts[j] is ListTextViewModel)
            //            {
            //                needsSaving = true;
            //                break;
            //            }
            //        }
            //        if (needsSaving)
            //            break;
            //    }
            //    else if (finishedLesson.LessonParts[i] is QuestionViewModel questionPage)
            //    {
            //        needsSaving = true;
            //        break;
            //    }
            //}

            ////Check if next relaxationProcedureVM needs to be set active
            //if (finishedLesson.IsActive)
            //{
            //    //check whether next behaviourExperimentVM or course has to be set active
            //    for (int i = 0; i < coursesPage.Features.Length; i++)
            //    {
            //        if (coursesPage.Features[i] is CourseViewModel course)
            //        {
            //            for (int j = 0; j < course.Lessons.Length; j++)
            //            {
            //                if (course.Lessons[j] == finishedLesson)
            //                {
            //                    //check if finishedLesson is last behaviourExperimentVM
            //                    if(j+1 < course.Lessons.Length)
            //                    {
            //                        //not last behaviourExperimentVM => enable next ResponseTraingingVM
            //                        if (!course.Lessons[j + 1].IsActive) //check whether update (serialization) is necessary
            //                        {
            //                            course.Lessons[j + 1].IsActive = true;
            //                            UpdateCoursesProgress(i + 1, j + 1);
            //                            SerializationManager.SaveCourses(coursesPage.GetPage());
            //                        }
            //                        else if(needsSaving) //save nonetheless if there was an component that required saving
            //                            SerializationManager.SaveCourses(coursesPage.GetPage());
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        //last behaviourExperimentVM => check if current course is last course
            //                        if(i+1 < coursesPage.Features.Length)
            //                        {
            //                            //not last course => enable next course
            //                            if (coursesPage.Features[i+1] is CourseViewModel)
            //                            {
            //                                if(!(coursesPage.Features[i + 1] as CourseViewModel)!.IsActive) //check whether update is necessary
            //                                {
            //                                    (coursesPage.Features[i + 1] as CourseViewModel)!.IsActive = true;
            //                                    UpdateCoursesProgress(i + 1, j + 1);
            //                                    SerializationManager.SaveCourses(coursesPage.GetPage());
            //                                }
            //                                else if(needsSaving) //save nonetheless if there was an component that required saving
            //                                    SerializationManager.SaveCourses(coursesPage.GetPage());
            //                            }
            //                            return;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //if (needsSaving)
            //{
            //    UpdateStrengthsData();
            //}
        }

        private static void UpdateCoursesProgress(int currentCourseNumber, int lastCompletedLessonNumber)
        {
            if (PreferenceManager.UpdateCourseProgress(currentCourseNumber, lastCompletedLessonNumber))
            {
                for (int i = 0; i< trainingPage?.Features.Length; i++)
                {
                    trainingPage.Features[i].VisitFeature?.RaiseCanExecuteChanged();
                }
            }
        }

        public static void SaveCourses()
        {
            if (coursesPage is not null)
                SerializationManager.SaveCourses(coursesPage.therapyPage);
        }
        public static void SaveSymptomCheckData()
        {
            if (symptomCheckLesson is null)
                return;
            OnSymptomCheckQuestionCompleted?.Invoke(null,EventArgs.Empty);
            SerializationManager.SaveSymptomCheckData(symptomCheckLesson);
        }
        public static void SaveTrainingData()
        {
            if (trainingPage is null)
                return;
            SerializationManager.SaveTrainingPage(trainingPage.therapyPage);
        }
    }
}
