using PeopleCurer.Models;
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
        static readonly string relativeUnmodifiedCoursesPagePath = Path.Combine("Assets", "courses.json");
        static readonly string modifiedCoursesPagePath = Path.Combine(FileSystem.AppDataDirectory, "Assets", "courses.json");

        public static bool LoadUnmodifiedCourses(out TherapyPage? coursesPage)
        {
            if (!FileSystem.AppPackageFileExistsAsync(relativeUnmodifiedCoursesPagePath).Result)
            {
                coursesPage = null;
                return false;
            }

            using (Stream fs = FileSystem.OpenAppPackageFileAsync(relativeUnmodifiedCoursesPagePath).Result)
            {
                coursesPage = JsonSerializer.Deserialize<TherapyPage>(fs);
            }

            return coursesPage is not null;
        }
        public static bool LoadCourses(out TherapyPage? coursesPage, bool tryDifferentialUpdate)
        {
            if (tryDifferentialUpdate)
                DifferentialUpdateManager.UpdateModifiedVersion();

            if (!File.Exists(modifiedCoursesPagePath))
            {
                coursesPage = null;
                return false;
            }
            string data = File.ReadAllText(modifiedCoursesPagePath);

            coursesPage = JsonSerializer.Deserialize<TherapyPage>(data);

            return coursesPage is not null;
        }
        public static void SaveCourses(in TherapyPage coursesPage)
        {
            string data = JsonSerializer.Serialize<TherapyPage>(coursesPage);

            if (!Directory.Exists(modifiedCoursesPagePath))
                Directory.CreateDirectory(Path.GetDirectoryName(modifiedCoursesPagePath)!);

            File.WriteAllText(modifiedCoursesPagePath, data);
        }
        public static void RemoveCourses()
        {
            File.Delete(modifiedCoursesPagePath);
            Directory.Delete(Path.GetDirectoryName(modifiedCoursesPagePath)!);
        }
    }
}
