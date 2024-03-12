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

        public static void SetCoursesPage(TherapyPageViewModel page)
        {
            coursesPage = page;
        }

        public static void UpdateProgress(in LessonViewModel finishedLesson)
        {
            if (coursesPage is null)
                return;
            if (finishedLesson.IsActive)
            {
                finishedLesson.IsActive = true;

                for (int i = 0; i < coursesPage.Features.Length; i++)
                {
                    if (coursesPage.Features[i] is CourseViewModel course)
                    {
                        for (int j = 0; j < course.Lessons.Length; j++)
                        {
                            if (course.Lessons[j] == finishedLesson)
                            {
                                //check if finishedLessin is last lesson
                                if(j+1 < course.Lessons.Length)
                                {
                                    //not last lesson => enable next Lesson
                                    if (!course.Lessons[j + 1].IsActive) //check whether update (serialization) is necessary
                                    {
                                        course.Lessons[j + 1].IsActive = true;
                                        SerializationManager.SaveCourses(coursesPage.GetPage());
                                    }
                                    return;
                                }
                                else
                                {
                                    //last lesson => check if current course is last course
                                    if(i+1 < coursesPage.Features.Length)
                                    {
                                        //not last course => enable next course
                                        if (coursesPage.Features[i+1] is CourseViewModel)
                                        {
                                            if(!(coursesPage.Features[i + 1] as CourseViewModel)!.IsActive)
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
            }
        }
    }
}
