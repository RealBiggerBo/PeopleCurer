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

        static readonly string relativeUnmodifiedSymptomCheckDataPath = Path.Combine("Assets", "symptomCheck.json");
        static readonly string modifiedSymptomCheckDataPath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "symptomCheck.json");

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
            Directory.Delete(Path.GetDirectoryName(modifiedCoursesDataPath)!);
        }

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
            Directory.Delete(Path.GetDirectoryName(modifiedSymptomCheckDataPath)!);
        }
    }
}
