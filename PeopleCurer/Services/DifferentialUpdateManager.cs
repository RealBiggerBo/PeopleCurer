using PeopleCurer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    public static class DifferentialUpdateManager
    {
        public static void UpdateModifiedVersion()
        {
            //Check creation time to only update if necesarry


            if(SerializationManager.LoadUnmodifiedCourses(out TherapyPage? unmodifiedCourse))
            {
                if(SerializationManager.LoadCourses(out TherapyPage? modifiedCourses,false))
                {
                    //get new Courses
                    TherapyPage newModifiedCourses = DifferentialUpdate(unmodifiedCourse!, modifiedCourses!);
                    //Serialize
                    SerializationManager.SaveCourses(newModifiedCourses);
                    Trace.WriteLine("Merged modified and unmodified versions.");
                }
                else
                {
                    //unmodified exists, modified not => save unmodified as modified
                    SerializationManager.SaveCourses(unmodifiedCourse!);
                    Trace.WriteLine("No modified version. Created new file.");
                }
            }
        }

        private static TherapyPage DifferentialUpdate(TherapyPage unmodified, TherapyPage modified)
        {
            TherapyPage merged = new TherapyPage(unmodified.name,unmodified.description, MergeFeatures(unmodified.features, modified.features));
            return merged;
        }

        private static Feature[] MergeFeatures(Feature[] unmodified, Feature[] modified)
        {
            Feature[] merged = new Feature[unmodified.Length];
            for (int i = 0; i < unmodified.Length; i++)
            {
                if (unmodified[i] is Course unmodifiedCourse)
                {
                    bool foundCourse = false;

                    for (int j = 0; j < modified.Length; j++)
                    {
                        if (modified[j] is Course modifiedCourse)
                        {
                            if(modifiedCourse.courseName == unmodifiedCourse.courseName)
                            {
                                if (modifiedCourse.isActive)
                                {
                                    merged[i] = new Course(
                                        unmodifiedCourse.courseName,
                                        unmodifiedCourse.description,
                                        MergeLessons(unmodifiedCourse.lessons, modifiedCourse.lessons)
                                        )
                                    {
                                        isActive = unmodifiedCourse.isActive || modifiedCourse.isActive
                                    };
                                }
                                else
                                {
                                    merged[i] = unmodifiedCourse;
                                }
                                foundCourse = true;
                                break;
                            }
                        }
                    }

                    if(!foundCourse)
                        merged[i] = unmodified[i];
                }
                else
                {
                    merged[i] = unmodified[i];
                }
            }

            return merged;
        }

        private static Lesson[] MergeLessons(Lesson[] unmodified, Lesson[] modified)
        {
            Lesson[] merged = new Lesson[unmodified.Length];
            for (int i = 0; i < unmodified.Length; i++)
            {
                bool foundLesson = false;

                for (int j = 0; j < modified.Length; j++)
                {
                    if (modified[j].lessonName == unmodified[i].lessonName)
                    {
                        if (modified[j].isActive)
                        {
                            merged[i] = new Lesson(
                                unmodified[i].lessonName, MergeLessonParts(unmodified[i].lessonParts, modified[j].lessonParts))
                            {
                                isActive = unmodified[i].isActive || modified[j].isActive
                            };
                        }
                        else
                        {
                            merged[i] = unmodified[i];
                        }
                        foundLesson = true;
                        break;
                    }
                }

                if (!foundLesson)
                    merged[i] = unmodified[i];
            }

            return merged;
        }

        private static LessonPart[] MergeLessonParts(LessonPart[] unmodified, LessonPart[] modified)
        {
            LessonPart[] merged = new LessonPart[unmodified.Length];
            for (int i = 0; i < unmodified.Length; i++)
            {
                if (unmodified[i] is Question unmodifiedQuestion)
                {
                    bool foundQuestion = false;

                    for (int j = 0; j < modified.Length; j++)
                    {
                        if (modified[j] is Question modifiedQuestion)
                        {
                            if (modifiedQuestion.issue == unmodifiedQuestion.issue)
                            {
                                merged[i] = new Question(
                                    unmodifiedQuestion.issue,
                                    MergeAnswers(unmodifiedQuestion.answers, modifiedQuestion.answers)
                                    );
                                foundQuestion = true;
                                break;
                            }
                        }
                    }

                    if (!foundQuestion)
                        merged[i] = unmodified[i];
                }
                else
                {
                    merged[i] = unmodified[i];
                }
            }

            return merged;
        }

        private static Answer[] MergeAnswers(Answer[] unmodified, Answer[] modified)
        {
            Answer[] merged = new Answer[unmodified.Length];
            for (int i = 0; i < unmodified.Length; i++)
            {
                bool foundAnswer = false;

                for (int j = 0; j < modified.Length; j++)
                {
                    if (modified[j].answerText == unmodified[i].answerText)
                    {
                        unmodified[i].isChosen = modified[j].isChosen;
                        merged[i] = unmodified[i];
                        foundAnswer = true;
                        break;
                    }
                }

                if (!foundAnswer)
                    merged[i] = unmodified[i];
            }

            return merged;
        }
    }
}
