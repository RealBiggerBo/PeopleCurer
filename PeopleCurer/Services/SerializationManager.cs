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

        static readonly string realtiveSymptomCheckDataPath = Path.Combine("Assets", "symptomCheck.json");
        static readonly string symptomCheckResultsDirectory = Path.Combine(FileSystem.AppDataDirectory, "Assets", "SymptomCheckResults");

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
                DifferentialUpdateManager.UpdateModifiedVersion();

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
            Directory.Delete(Path.GetDirectoryName(modifiedCoursesDataPath)!);
        }

        //SymptomCheck
        public static bool LoadSymptomCheckData(out TherapyPage? symptomCheckPage)
        {
            if (!FileSystem.AppPackageFileExistsAsync(realtiveSymptomCheckDataPath).Result)
            {
                symptomCheckPage = null;
                return false;
            }

            using (Stream fs = FileSystem.OpenAppPackageFileAsync(realtiveSymptomCheckDataPath).Result)
            {
                symptomCheckPage = JsonSerializer.Deserialize<TherapyPage>(fs);
            }

            return symptomCheckPage is not null;
        }
        public static void SaveSymptomCheckResult(in SymptomCheckQuestionViewModel symptomCheckPage)
        {
            string data = JsonSerializer.Serialize<int>(symptomCheckPage.AnswerValue);

            string path = Path.Combine(symptomCheckResultsDirectory, DateTime.Today.ToFileTime().ToString(), symptomCheckPage.StatisticsID + ".json");

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            }

            File.WriteAllText(path, data);
        }
        public static bool LoadSymptomCheckResults(out Dictionary<DateTime, int> results, string statisticsID)
        {
            //Get all directories(dates) of symptom checks
            string[] directories = Directory.GetFileSystemEntries(symptomCheckResultsDirectory);

            List<DateTime> dates = new List<DateTime>(directories.Length);

            //Get creationTime of all entries
            for (int i = 0; i < directories.Length; i++)
            {
                if(long.TryParse(new DirectoryInfo(directories[i]).Name,out long parsedDate))
                {
                    dates.Add(DateTime.FromFileTime(parsedDate));
                }
            }

            //sort dates
            dates.Sort(0,int.Min(7,dates.Count), null);


            results = new Dictionary<DateTime, int>();

            //access 7 latest paths
            for (int i = 0; i < int.Min(7,dates.Count); i++)
            {
                //access path and 
                string path = Path.Combine(symptomCheckResultsDirectory, dates[i].ToFileTime().ToString(), statisticsID + ".json");
                if (File.Exists(path))
                {
                    int value = JsonSerializer.Deserialize<int>(File.ReadAllText(path));

                    results.Add(dates[i],value);
                }

                //
            }

             return true;
        }
        public static void RemoveSymptomCheckResults()
        {
            Directory.Delete(symptomCheckResultsDirectory!, true);
        }
    }
}
