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
        public static void UpdateModifiedCoursesVersion()
        {
            //TODO: Check creation time to only update if necesarry

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

        public static void UpdateModifiedSymptomCheckVersion()
        {
            //TODO: Check creation time to only update if necesarry

            if (SerializationManager.LoadUnmodifiedSymptomCheckData(out TherapyPage? unmodifiedSymptomCheck))
            {
                if (SerializationManager.LoadSymptomCheckData(out TherapyPage? modifiedSymptomCheck, false))
                {
                    //get new SymptomCheck
                    TherapyPage newModifiedSymptomCheck = DifferentialUpdate(unmodifiedSymptomCheck!, modifiedSymptomCheck!);
                    //Serialize
                    SerializationManager.SaveSymptomCheckData(newModifiedSymptomCheck);
                    Trace.WriteLine("Merged modified and unmodified versions (SymptomCheck).");
                }
                else
                {
                    //unmodified exists, modified not => save unmodified as modified
                    SerializationManager.SaveSymptomCheckData(unmodifiedSymptomCheck!);
                    Trace.WriteLine("No modified version. Created new file (SymptomCheck).");
                }
            }
        }

        public static void UpdateModifiedTrainingPageVersion()
        {
            //TODO: Check creation time to only update if necesarry

            if (SerializationManager.LoadUnmodifiedTrainingPage(out TherapyPage? unmodifiedTrainingPage))
            {
                if (SerializationManager.LoadTrainingPage(out TherapyPage? modifiedTrainingPage, false))
                {
                    //get new SymptomCheck
                    TherapyPage newModifiedSymptomCheck = DifferentialUpdate(unmodifiedTrainingPage!, modifiedTrainingPage!);
                    //Serialize
                    SerializationManager.SaveTrainingPage(newModifiedSymptomCheck);
                    Trace.WriteLine("Merged modified and unmodified versions (TraininPage).");
                }
                else
                {
                    //unmodified exists, modified not => save unmodified as modified
                    SerializationManager.SaveTrainingPage(unmodifiedTrainingPage!);
                    Trace.WriteLine("No modified version. Created new file (TrainingPage).");
                }
            }
        }

        public static void UpdateModifiedStrengthsPageVersion()
        {
            //TODO: Check creation time to only update if necesarry

            if (SerializationManager.LoadUnmodifiedStrengthsPage(out TherapyPage? unmodifiedStrengthsPage))
            {
                if (SerializationManager.LoadStrengthsPage(out TherapyPage? modifiedStrengthsPage, false))
                {
                    //get new SymptomCheck
                    TherapyPage newModifiedStrengthsPage = DifferentialUpdate(unmodifiedStrengthsPage!, modifiedStrengthsPage!);
                    //Serialize
                    SerializationManager.SaveStrengthsPage(newModifiedStrengthsPage);
                    Trace.WriteLine("Merged modified and unmodified versions (StrengthsPage).");
                }
                else
                {
                    //unmodified exists, modified not => save unmodified as modified
                    SerializationManager.SaveStrengthsPage(unmodifiedStrengthsPage!);
                    Trace.WriteLine("No modified version. Created new file (StrengthsPage).");
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
                else if (unmodified[i] is BehaviourExperiment unmodifiedBehaviourExperiment)
                {
                    bool foundBehaviourExperiment = false;

                    for (int j = 0; j < modified.Length; j++)
                    {
                        if (modified[j] is BehaviourExperiment modifiedBehaviourExperiment)
                        {
                            merged[i] = new BehaviourExperiment(modifiedBehaviourExperiment.situations);
                            foundBehaviourExperiment = true;
                            break;
                        }
                    }

                    if(!foundBehaviourExperiment)
                        merged[i] = unmodified[i];
                }
                else if (unmodified[i] is ThoughtTestContainer unmodifiedThoughtTestContainer)
                {
                    bool foundThoughtTestContainer = false;

                    for (int j = 0; j < modified.Length; j++)
                    {
                        if (modified[j] is ThoughtTestContainer modifiedThoughtTestContainer)
                        {
                            merged[i] = new ThoughtTestContainer(modifiedThoughtTestContainer.thoughtTests);
                            foundThoughtTestContainer = true;
                            break;
                        }
                    }

                    if (!foundThoughtTestContainer)
                        merged[i] = unmodified[i];
                }
                else if (unmodified[i] is StrengthsCourse unmodifiedStrengthsCourse)
                {
                    bool foundStrengthsCourse = false;

                    for (int j = 0; j < modified.Length; j++)
                    {
                        if (modified[j] is StrengthsCourse modifiedStrengthsCourse)
                        {
                            merged[i] = new StrengthsCourse(
                                MergeLesson(unmodifiedStrengthsCourse.strengthsLesson, modifiedStrengthsCourse.strengthsLesson),
                                MergeLesson(unmodifiedStrengthsCourse.successMomentsLesson, modifiedStrengthsCourse.successMomentsLesson),
                                MergeLesson(unmodifiedStrengthsCourse.trainingSuccessLesson, modifiedStrengthsCourse.trainingSuccessLesson));
                            foundStrengthsCourse = true;
                            break;
                        }
                    }

                    if (!foundStrengthsCourse)
                        merged[i] = unmodified[i];
                }
                else
                {
                    merged[i] = unmodified[i];
                }
            }

            return merged;
        }

        private static Lesson MergeLesson(Lesson unmodified, Lesson modified)
        {
            return new Lesson(unmodified.lessonName, unmodified.lessonReward, MergeLessonParts(unmodified.lessonParts, modified.lessonParts));
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
                                unmodified[i].lessonName,
                                unmodified[i].lessonReward,
                                MergeLessonParts(unmodified[i].lessonParts, modified[j].lessonParts))
                            {
                                isActive = unmodified[i].isActive || modified[j].isActive,
                                isCompleted = modified[j].isCompleted,
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
                else if (unmodified[i] is InfoPage unmodifiedInfoPage)
                {
                    bool foundInfoPage = false;

                    for (int j = 0; j < modified.Length; j++)
                    {
                        if (modified[j] is InfoPage modifiedInfoPage)
                        {
                            if (AreInfoPagesSimilar(unmodifiedInfoPage, modifiedInfoPage))
                            {
                                merged[i] = new InfoPage(MergeTextParts(unmodifiedInfoPage.textParts, modifiedInfoPage.textParts));
                                foundInfoPage = true;
                                break;
                            }
                        }
                    }

                    if (!foundInfoPage)
                        merged[i] = unmodified[i];
                }
                else if (unmodified[i] is SymptomCheckQuestion unmodifiedSymptomCheckQuestion)
                {
                    bool foundSymptomCheckQuestion = false;

                    for (int j = 0; j < modified.Length; j++)
                    {
                        if (modified[j] is SymptomCheckQuestion modifiedSymptomCheckQuestion)
                        {
                            if(unmodifiedSymptomCheckQuestion.issue == modifiedSymptomCheckQuestion.issue)
                            {
                                merged[i] = new SymptomCheckQuestion(
                                    unmodifiedSymptomCheckQuestion.issue,
                                    unmodifiedSymptomCheckQuestion.description,
                                    unmodifiedSymptomCheckQuestion.lowText,
                                    unmodifiedSymptomCheckQuestion.highText,
                                    unmodifiedSymptomCheckQuestion.answerValue,
                                    modifiedSymptomCheckQuestion.data);
                                foundSymptomCheckQuestion = true;
                                break;
                            }
                        }
                    }

                    if (!foundSymptomCheckQuestion)
                        merged[i] = unmodified[i];
                }
                else
                {
                    merged[i] = unmodified[i];
                }
            }

            return merged;
        }

        private static bool AreInfoPagesSimilar(InfoPage unmodified, InfoPage modified)
        {
            if(unmodified.textParts.Length == modified.textParts.Length)
            {
                for (int i = 0; i < unmodified.textParts.Length; i++)
                {
                    if (unmodified.textParts[i].GetType() != modified.textParts[i].GetType())
                    {
                        //types do not match 
                        return false;
                    }
                }
            }
            else
            {
                //lengths do not match
                return false;
            }

            return true;
        }

        private static TextPart[] MergeTextParts(TextPart[] unmodified, TextPart[] modified)
        {
            TextPart[] merged = new TextPart[unmodified.Length];
            for (int i = 0; i < unmodified.Length; i++)
            {
                bool foundTextPart = false;

                for (int j = 0; j < modified.Length; j++)
                {
                    if (AreTextPartsSimilar(modified[j], unmodified[i]))
                    {
                        merged[i] = MergeTextPart(unmodified[i], modified[j]);
                        foundTextPart = true;
                        break;
                    }
                }

                if(!foundTextPart)
                    merged[i] = unmodified[i];
            }

            return merged;
        }

        private static bool AreTextPartsSimilar(TextPart unmodified, TextPart modified)
        {
            if(unmodified.GetType() == modified.GetType())
            {
                if(unmodified is TextBox unmodifiedTextBox && modified is TextBox modifiedTextBox)
                {
                    if(unmodifiedTextBox.question == modifiedTextBox.question)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private static TextPart MergeTextPart(TextPart unmodified, TextPart modified)
        {
            if (unmodified is TextBox unmodifiedTextBox && modified is TextBox modifiedTextBox)
            {
                if (unmodifiedTextBox.question == modifiedTextBox.question)
                {
                    return modifiedTextBox;
                }
            }
            else if (unmodified is FearCircleDiagram unmodifiedFearCircleDiagram && modified is FearCircleDiagram modifiedFearCircleDiagram)
            {
                return modifiedFearCircleDiagram;
            }
            else if(unmodified is ListText unmodifiedListText && modified is ListText modifiedListText)
            {
                if(unmodifiedListText.maxAmount == modifiedListText.maxAmount)
                {
                    return modifiedListText;
                }
            }

            return unmodified;
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
