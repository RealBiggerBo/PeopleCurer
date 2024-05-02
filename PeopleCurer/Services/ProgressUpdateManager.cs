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
        private static TherapyPageViewModel? coursesPage;
        private static TherapyPageViewModel? symptomCheckPage;
        private static TherapyPageViewModel? trainingPage;
        private static TherapyPageViewModel? strengthsPage;

        public static void SetCoursesPage(TherapyPageViewModel page)
        {
            coursesPage = page;
        }
        public static void SetSymptomCheckPage(TherapyPageViewModel page)
        {
            symptomCheckPage = page;
        }
        public static void SetTrainingPage(TherapyPageViewModel page)
        {
            trainingPage = page;
        }
        public static void SetStrengthsPage(TherapyPageViewModel page)
        {
            strengthsPage = page;
        }

        public static void UpdateProgress(in LessonViewModel finishedLesson)
        {
            if (coursesPage is null)
                return;

            //Courses needs Saving if there is an TextBox or any other input form
            bool needsSaving = false;
            for (int i = 0; i < finishedLesson.LessonParts.Length; i++)
            {
                if (finishedLesson.LessonParts[i] is InfoPageViewModel infoPage)
                {
                    for (int j = 0; j < infoPage.TextParts.Length; j++)
                    {
                        if (infoPage.TextParts[j] is TextBoxViewModel)
                        {
                            needsSaving = true;
                            break;
                        }
                        else if (infoPage.TextParts[j] is FearCircleDiagramViewModel)
                        {
                            needsSaving = true;
                            break;
                        }
                        else if (infoPage.TextParts[j] is ListTextViewModel)
                        {
                            needsSaving = true;
                            break;
                        }
                    }
                    if (needsSaving)
                        break;
                }
            }

            //Check if next relaxationProcedureVM needs to be set active
            if (finishedLesson.IsActive)
            {
                finishedLesson.IsActive = true;

                //check whether next behaviourExperimentVM or course has to be set active
                for (int i = 0; i < coursesPage.Features.Length; i++)
                {
                    if (coursesPage.Features[i] is CourseViewModel course)
                    {
                        for (int j = 0; j < course.Lessons.Length; j++)
                        {
                            if (course.Lessons[j] == finishedLesson)
                            {
                                //check if finishedLessin is last behaviourExperimentVM
                                if(j+1 < course.Lessons.Length)
                                {
                                    //not last behaviourExperimentVM => enable next Situation
                                    if (!course.Lessons[j + 1].IsActive) //check whether update (serialization) is necessary
                                    {
                                        course.Lessons[j + 1].IsActive = true;
                                        SerializationManager.SaveCourses(coursesPage.GetPage());
                                    }
                                    else if(needsSaving) //save nonetheless if there was an component that required saving
                                        SerializationManager.SaveCourses(coursesPage.GetPage());
                                    return;
                                }
                                else
                                {
                                    //last behaviourExperimentVM => check if current course is last course
                                    if(i+1 < coursesPage.Features.Length)
                                    {
                                        //not last course => enable next course
                                        if (coursesPage.Features[i+1] is CourseViewModel)
                                        {
                                            if(!(coursesPage.Features[i + 1] as CourseViewModel)!.IsActive) //check whether update is necessary
                                            {
                                                (coursesPage.Features[i + 1] as CourseViewModel)!.IsActive = true;
                                                SerializationManager.SaveCourses(coursesPage.GetPage());
                                            }
                                            else if(needsSaving) //save nonetheless if there was an component that required saving
                                                SerializationManager.SaveCourses(coursesPage.GetPage());
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (needsSaving)
            {
                UpdateStrengthsData();
            }
        }

        public static void UpdateSymptomCheckData()
        {
            if (symptomCheckPage is null)
                return;
            SerializationManager.SaveSymptomCheckData(symptomCheckPage.GetPage());
        }
        public static void UpdateTrainingData()
        {
            if(trainingPage is null) 
                return;
            SerializationManager.SaveTrainingPage(trainingPage.GetPage());
        }
        public static void UpdateStrengthsData()
        {
            if (strengthsPage is null)
                return;
            SerializationManager.SaveStrengthsPage(strengthsPage.GetPage());
        }
    }
}
