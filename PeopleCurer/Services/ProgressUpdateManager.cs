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

        public static void SetCoursesPage(TherapyPageViewModel page)
        {
            coursesPage = page;
        }
        public static void SetSymptomCheckPage(TherapyPageViewModel page)
        {
            symptomCheckPage = page;
        }

        public static void UpdateProgress(in LessonViewModel finishedLesson)
        {
            if (coursesPage is null)
                return;
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
                                    //not last behaviourExperimentVM => enable next Lesson
                                    if (!course.Lessons[j + 1].IsActive) //check whether update (serialization) is necessary
                                    {
                                        course.Lessons[j + 1].IsActive = true;
                                        SerializationManager.SaveCourses(coursesPage.GetPage());
                                    }
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
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                //Save Courses if there is an TextBox or SymptomCheckLesson
                for (int i = 0; i < finishedLesson.LessonParts.Length; i++)
                {
                    if (finishedLesson.LessonParts[i] is InfoPageViewModel infoPage)
                    {
                        for (int j = 0; j < infoPage.TextParts.Length; j++)
                        {
                            if (infoPage.TextParts[j] is TextBoxViewModel)
                            {
                                SerializationManager.SaveCourses(coursesPage.GetPage());
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static void UpdateSymptomCheckData()
        {
            if (symptomCheckPage is null)
                return;
            SerializationManager.SaveSymptomCheckData(symptomCheckPage.GetPage());
        }
    }
}
