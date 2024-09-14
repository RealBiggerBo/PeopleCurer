using PeopleCurer.Models;
using PeopleCurer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    public static class SerializationManager
    {
        static readonly string relativeUnmodifiedCoursesDataPath = Path.Combine("Assets", "courses.json");
        static readonly string modifiedCoursesDataPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "courses.json");

        static readonly string relativeUnmodifiedTrainingPageDataPath = Path.Combine("Assets", "training.json");
        static readonly string modifiedTrainingPageDataPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "training.json");

        static readonly string relativeUnmodifiedStrengthsPageDataPath = Path.Combine("Assets", "strengths.json");
        static readonly string modifiedStrengthsPageDataPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "strengths.json");

        static readonly string relativeUnmodifiedSymptomCheckDataPath = Path.Combine("Assets", "symptomCheck.json");
        static readonly string modifiedSymptomCheckDataPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "symptomCheck.json");

        static readonly string absoluteUserInputDataPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "userInputData.json");

        //Courses
        public static bool LoadUnmodifiedCourses(out TherapyPage? coursesPage)
        {
            if (!FileSystem.AppPackageFileExistsAsync(relativeUnmodifiedCoursesDataPath).Result)
            {
                coursesPage = null;
                return false;
            }

            using (Stream fs = FileSystem.OpenAppPackageFileAsync(relativeUnmodifiedCoursesDataPath).Result)
            {
                try
                {
                    coursesPage = JsonSerializer.Deserialize<TherapyPage>(fs);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return coursesPage is not null;
        }
        public static bool LoadCourses(out TherapyPage? coursesPage, bool tryDifferentialUpdate)
        {
            if (tryDifferentialUpdate)
                DifferentialUpdateManager.UpdateModifiedCoursesVersion();

            if (!File.Exists(modifiedCoursesDataPath))
            {
                coursesPage = null;
                return false;
            }
            string data = File.ReadAllText(modifiedCoursesDataPath);

            try
            {
                coursesPage = JsonSerializer.Deserialize<TherapyPage>(data);
            }
            catch (Exception ex)
            {
                coursesPage = null;
            }

            return coursesPage is not null;
        }
        public static void SaveCourses(in TherapyPage coursesPage)
        {
            string data = JsonSerializer.Serialize<TherapyPage>(coursesPage);


            if (!Directory.Exists(modifiedCoursesDataPath))
                Directory.CreateDirectory(Path.GetDirectoryName(modifiedCoursesDataPath)!);

            File.WriteAllText(modifiedCoursesDataPath, data);
        }
        public static void RemoveCourses()
        {
            File.Delete(modifiedCoursesDataPath);
            if (!Directory.GetFileSystemEntries(Path.GetDirectoryName(modifiedCoursesDataPath)!).Any())
                Directory.Delete(Path.GetDirectoryName(modifiedCoursesDataPath)!);
        }

        //TrainingPage
        public static bool LoadUnmodifiedTrainingPage(out TherapyPage? trainingPage)
        {
            if (!FileSystem.AppPackageFileExistsAsync(relativeUnmodifiedTrainingPageDataPath).Result)
            {
                trainingPage = null;
                return false;
            }

            using (Stream fs = FileSystem.OpenAppPackageFileAsync(relativeUnmodifiedTrainingPageDataPath).Result)
            {
                try
                {
                    trainingPage = JsonSerializer.Deserialize<TherapyPage>(fs);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return trainingPage is not null;
        }
        public static bool LoadTrainingPage(out TherapyPage? trainingPage, bool tryDifferentialUpdate)
        {
            if (tryDifferentialUpdate)
                DifferentialUpdateManager.UpdateModifiedTrainingPageVersion();

            if (!File.Exists(modifiedTrainingPageDataPath))
            {
                trainingPage = null;
                return false;
            }
            string data = File.ReadAllText(modifiedTrainingPageDataPath);

            try
            {
                trainingPage = JsonSerializer.Deserialize<TherapyPage>(data); 
            }
            catch (Exception ex)
            {
                trainingPage = null;
            }

            return trainingPage is not null;
        }
        public static void SaveTrainingPage(in TherapyPage trainingPage)
        {
            string data = JsonSerializer.Serialize<TherapyPage>(trainingPage);

            if (!Directory.Exists(modifiedTrainingPageDataPath))
                Directory.CreateDirectory(Path.GetDirectoryName(modifiedTrainingPageDataPath)!);

            File.WriteAllText(modifiedTrainingPageDataPath, data);
        }
        public static void RemoveTrainingPage()
        {
            File.Delete(modifiedTrainingPageDataPath);
            if (Directory.GetFileSystemEntries(Path.GetDirectoryName(modifiedTrainingPageDataPath)!).Length != 0)
                Directory.Delete(Path.GetDirectoryName(modifiedTrainingPageDataPath)!);
        }

        //StrengthsPage
        public static bool LoadUnmodifiedStrengthsPage(out TherapyPage? strengthsPage)
        {
            if (!FileSystem.AppPackageFileExistsAsync(relativeUnmodifiedStrengthsPageDataPath).Result)
            {
                strengthsPage = null;
                return false;
            }

            try
            {
                using (Stream fs = FileSystem.OpenAppPackageFileAsync(relativeUnmodifiedStrengthsPageDataPath).Result)
                {
                    strengthsPage = JsonSerializer.Deserialize<TherapyPage>(fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strengthsPage is not null;
        }
        public static bool LoadStrengthsPage(out TherapyPage? strengthsPage, bool tryDifferentialUpdate)
        {
            if (tryDifferentialUpdate)
                DifferentialUpdateManager.UpdateModifiedStrengthsPageVersion();

            if (!File.Exists(modifiedStrengthsPageDataPath))
            {
                strengthsPage = null;
                return false;
            }
            string data = File.ReadAllText(modifiedStrengthsPageDataPath);

            strengthsPage = JsonSerializer.Deserialize<TherapyPage>(data);

            return strengthsPage is not null;
        }
        public static void SaveStrengthsPage(in TherapyPage strengthsPage)
        {
            string data = JsonSerializer.Serialize<TherapyPage>(strengthsPage);

            if (!Directory.Exists(modifiedStrengthsPageDataPath))
                Directory.CreateDirectory(Path.GetDirectoryName(modifiedStrengthsPageDataPath)!);

            File.WriteAllText(modifiedStrengthsPageDataPath, data);
        }
        public static void RemoveStrengthsPage()
        {
            File.Delete(modifiedStrengthsPageDataPath);
            if (Directory.GetFileSystemEntries(Path.GetDirectoryName(modifiedStrengthsPageDataPath)!).Length != 0)
                Directory.Delete(Path.GetDirectoryName(modifiedStrengthsPageDataPath)!);
        }

        #region BehaviourExperiment
        //BehaviourExperimentContainer
        //public static bool LoadBehaviourExperimentData(out BehaviourExperimentContainer? behaviourExperiment)
        //{
        //    if (!File.Exists(absoluteBehaviourExperimentDataPath))
        //    {
        //        behaviourExperiment = null;
        //        return false;
        //    }

        //    string data = File.ReadAllText(absoluteBehaviourExperimentDataPath);

        //    behaviourExperiment = JsonSerializer.Deserialize<BehaviourExperimentContainer>(data);

        //    return behaviourExperiment is not null;
        //}
        //public static void SaveBehaviourExperimenttData(in BehaviourExperimentContainer behaviourExperiment)
        //{
        //    string data = JsonSerializer.Serialize<BehaviourExperimentContainer>(behaviourExperiment);

        //    if (!Directory.Exists(absoluteBehaviourExperimentDataPath))
        //        Directory.CreateDirectory(Path.GetDirectoryName(absoluteBehaviourExperimentDataPath)!);

        //    File.WriteAllText(absoluteBehaviourExperimentDataPath, data);
        //}
        //public static void RemoveBehaviourExperimentData()
        //{
        //    File.Delete(absoluteBehaviourExperimentDataPath);
        //    if (Directory.GetFileSystemEntries(Path.GetDirectoryName(absoluteBehaviourExperimentDataPath)!).Length >= 0)
        //        Directory.Delete(Path.GetDirectoryName(absoluteBehaviourExperimentDataPath)!);
        //} 
        #endregion

        //SymptomCheck
        public static bool LoadUnmodifiedSymptomCheckData(out NormalLesson? symptomCheckLesson)
        {
            if (!FileSystem.AppPackageFileExistsAsync(relativeUnmodifiedSymptomCheckDataPath).Result)
            {
                symptomCheckLesson = null;
                return false;
            }

            using (Stream fs = FileSystem.OpenAppPackageFileAsync(relativeUnmodifiedSymptomCheckDataPath).Result)
            {
                symptomCheckLesson = JsonSerializer.Deserialize<NormalLesson>(fs);
            }

            return symptomCheckLesson is not null;
        }
        public static bool LoadSymptomCheckData(out NormalLesson? symptomCheckLesson, bool tryDifferentialUpdate)
        {
            if (tryDifferentialUpdate)
                DifferentialUpdateManager.UpdateModifiedSymptomCheckVersion();

            if (!File.Exists(modifiedSymptomCheckDataPath))
            {
                symptomCheckLesson = null;
                return false;
            }
            string data = File.ReadAllText(modifiedSymptomCheckDataPath);

            symptomCheckLesson = JsonSerializer.Deserialize<NormalLesson>(data);

            return symptomCheckLesson is not null;
        }
        public static void SaveSymptomCheckData(in NormalLesson symptomCheckLesson)
        {
            string data = JsonSerializer.Serialize<NormalLesson>(symptomCheckLesson);

            if (!Directory.Exists(modifiedSymptomCheckDataPath))
                Directory.CreateDirectory(Path.GetDirectoryName(modifiedSymptomCheckDataPath)!);

            File.WriteAllText(modifiedSymptomCheckDataPath, data);
        }
        public static void RemoveSymptomCheckResults()
        {
            File.Delete(modifiedSymptomCheckDataPath);
            if (Directory.GetFileSystemEntries(Path.GetDirectoryName(modifiedSymptomCheckDataPath)!).Length == 0)
                Directory.Delete(Path.GetDirectoryName(modifiedSymptomCheckDataPath)!);
        }

        //UserInputData
        public static bool LoadUserInputData(out Dictionary<string, string>? userInputData)
        {
            if (!File.Exists(absoluteUserInputDataPath))
            {
                userInputData = null;
                return false;
            }

            string data = File.ReadAllText(absoluteUserInputDataPath);

            userInputData = JsonSerializer.Deserialize<Dictionary<string, string>>(data);

            return userInputData is not null;
        }
        public static void SaveUserInputData(in Dictionary<string, string> userInputData)
        {
            string data = JsonSerializer.Serialize<Dictionary<string, string>>(userInputData);

            if (!Directory.Exists(absoluteUserInputDataPath))
                Directory.CreateDirectory(Path.GetDirectoryName(absoluteUserInputDataPath)!);

            File.WriteAllText(absoluteUserInputDataPath, data);
        }
        public static void RemoveUserInputData()
        {
            File.Delete(absoluteUserInputDataPath);
            if (Directory.GetFileSystemEntries(Path.GetDirectoryName(absoluteUserInputDataPath)!).Any())
                Directory.Delete(Path.GetDirectoryName(absoluteUserInputDataPath)!);
        }

        public static Stream[] GetStreams()
        {
            List<Stream> streams = [];

            //if (File.Exists(modifiedCoursesDataPath))
            //{
            //    streams.Add(File.OpenRead(modifiedCoursesDataPath));
            //}
            //if (File.Exists(modifiedTrainingPageDataPath))
            //{
            //    streams.Add(File.OpenRead(modifiedTrainingPageDataPath));
            //}
            //if (File.Exists(modifiedStrengthsPageDataPath))
            //{
            //    streams.Add(File.OpenRead(modifiedStrengthsPageDataPath));
            //}
            if (File.Exists(modifiedSymptomCheckDataPath))
            {
                streams.Add(File.OpenRead(modifiedSymptomCheckDataPath));
            }
            //if (File.Exists(absoluteUserInputDataPath))
            //{
            //    streams.Add(File.OpenRead(absoluteUserInputDataPath));
            //}

            return [.. streams];
        }
    }
}
