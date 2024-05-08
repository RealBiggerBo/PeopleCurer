using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    public static class PreferenceManager
    {
        private const string lastSymptomCheckDate = "lastSymptomCheckDate";
        private const string welcomePageCompletionStatus = "welcomePageCompletionStatus";
        private const string courseProgress = "courseProgress";

        //last symptom check date
        public static void UpdateLastSymptomCheckDate()
        {
            Preferences.Set(lastSymptomCheckDate, DateOnly.FromDateTime(DateTime.UtcNow).ToString());
        }
        public static DateOnly GetLastSymptomCheckDate()
        {
            DateOnly defaultVal = DateOnly.MinValue;

            if (DateOnly.TryParse(Preferences.Get(lastSymptomCheckDate, defaultVal.ToString()), out DateOnly result))
                return result;
            return DateOnly.MinValue;

        }
        public static void RemoveLastSymptomCheckDate()
        {
            Preferences.Remove(lastSymptomCheckDate);
        }

        //welcome page completion status
        public static void CompleteWelcomePage()
        {
            Preferences.Set(welcomePageCompletionStatus, true);
        }
        public static bool GetWelcomePageCompletionStatus()
        {
            return Preferences.Get(welcomePageCompletionStatus, false);
        }
        public static void RemoveWelcomePageCompletionStatus()
        {
            Preferences.Remove(welcomePageCompletionStatus);
        }

        //course progress
        /// <summary>
        /// Updates the preference related to CourseProgress
        /// </summary>
        /// <param name="currentCourseNumber">the number of the highest active course starting from 1</param>
        /// <param name="lastCompletedLessonNumber">the number of the highest completed Lesson starting from 1</param>
        /// <returns>true if references was updated. False otherwise</returns>
        public static bool UpdateCourseProgress(int currentCourseNumber, int lastCompletedLessonNumber)
        {
            int newProgress = currentCourseNumber * 100 + lastCompletedLessonNumber;
            int oldProgress = GetCourseProgress();

            if(newProgress > oldProgress)
            {
                Preferences.Set(courseProgress, newProgress);
                return true;
            }
            return false;
        }
        public static int GetCourseProgress()
        {
            return Preferences.Get(courseProgress, 000);
        }
        public static void RemoveCourseProgress()
        {
            Preferences.Remove(courseProgress);
        }
    }
}
