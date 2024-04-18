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
                coursesPage = JsonSerializer.Deserialize<TherapyPage>(fs);
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

            coursesPage = JsonSerializer.Deserialize<TherapyPage>(data);

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
            if(!Directory.GetFileSystemEntries(Path.GetDirectoryName(modifiedCoursesDataPath)!).Any())
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
                trainingPage = JsonSerializer.Deserialize<TherapyPage>(fs);
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
           
            trainingPage = JsonSerializer.Deserialize<TherapyPage>(data);

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

        //BehaviourExperiment
        //public static bool LoadBehaviourExperimentData(out BehaviourExperiment? behaviourExperiment)
        //{
        //    if (!File.Exists(absoluteBehaviourExperimentDataPath))
        //    {
        //        behaviourExperiment = null;
        //        return false;
        //    }

        //    string data = File.ReadAllText(absoluteBehaviourExperimentDataPath);

        //    behaviourExperiment = JsonSerializer.Deserialize<BehaviourExperiment>(data);

        //    return behaviourExperiment is not null;
        //}
        //public static void SaveBehaviourExperimenttData(in BehaviourExperiment behaviourExperiment)
        //{
        //    string data = JsonSerializer.Serialize<BehaviourExperiment>(behaviourExperiment);

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

        //SymptomCheck
        public static bool LoadUnmodifiedSymptomCheckData(out TherapyPage? symptomCheckPage)
        {
            if (!FileSystem.AppPackageFileExistsAsync(relativeUnmodifiedSymptomCheckDataPath).Result)
            {
                symptomCheckPage = null;
                return false;
            }

            using (Stream fs = FileSystem.OpenAppPackageFileAsync(relativeUnmodifiedSymptomCheckDataPath).Result)
            {
                symptomCheckPage = JsonSerializer.Deserialize<TherapyPage>(fs);
            }

            return symptomCheckPage is not null;
        }
        public static bool LoadSymptomCheckData(out TherapyPage? symptomCheckPage, bool tryDifferentialUpdate)
        {
            if (tryDifferentialUpdate)
                DifferentialUpdateManager.UpdateModifiedSymptomCheckVersion();

            if (!File.Exists(modifiedSymptomCheckDataPath))
            {
                symptomCheckPage = null;
                return false;
            }
            string data = File.ReadAllText(modifiedSymptomCheckDataPath);

            symptomCheckPage = JsonSerializer.Deserialize<TherapyPage>(data);

            return symptomCheckPage is not null;
        }
        public static void SaveSymptomCheckData(in TherapyPage symptomCheckPage)
        {
            string data = JsonSerializer.Serialize<TherapyPage>(symptomCheckPage);

            if (!Directory.Exists(modifiedSymptomCheckDataPath))
                Directory.CreateDirectory(Path.GetDirectoryName(modifiedSymptomCheckDataPath)!);

            File.WriteAllText(modifiedSymptomCheckDataPath, data);
        }
        public static void RemoveSymptomCheckResults()
        {
            File.Delete(modifiedSymptomCheckDataPath);
            if (!Directory.GetFileSystemEntries(Path.GetDirectoryName(modifiedSymptomCheckDataPath)!).Any())
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
        public static void SaveUserInputData(in Dictionary<string,string> userInputData)
        {
            string data = JsonSerializer.Serialize<Dictionary<string,string>>(userInputData);

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
    }
}
