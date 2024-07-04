using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    public static class PreferenceManager
    {
        private const string lastSymptomCheckDate_Key = "lastSymptomCheckDate";
        private const string welcomePageCompletionStatus_Key = "welcomePageCompletionStatus";
        private const string courseProgress_Key = "courseProgress";
        private const string collectedRewardXP_Key = "collectedRewardXP";

        //last symptom check date
        public static void UpdateLastSymptomCheckDate()
        {
            Preferences.Set(lastSymptomCheckDate_Key, DateOnly.FromDateTime(DateTime.UtcNow).ToString());
        }
        public static DateOnly GetLastSymptomCheckDate()
        {
            DateOnly defaultVal = DateOnly.MinValue;

            string lastSymptomCheckDate = Preferences.Get(lastSymptomCheckDate_Key, defaultVal.ToLongDateString());
            if (DateOnly.TryParse(lastSymptomCheckDate, out DateOnly result))
            {
                return result;
            }

            return defaultVal;
        }
        public static void RemoveLastSymptomCheckDate()
        {
            Preferences.Remove(lastSymptomCheckDate_Key);
        }

        //welcome page completion status
        public static void CompleteWelcomePage()
        {
            Preferences.Set(welcomePageCompletionStatus_Key, true);
        }
        public static bool GetWelcomePageCompletionStatus()
        {
            return Preferences.Get(welcomePageCompletionStatus_Key, false);
        }
        public static void RemoveWelcomePageCompletionStatus()
        {
            Preferences.Remove(welcomePageCompletionStatus_Key);
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
                Preferences.Set(courseProgress_Key, newProgress);
                return true;
            }
            return false;
        }
        public static int GetCourseProgress()
        {
            return Preferences.Get(courseProgress_Key, 000);
        }
        public static void RemoveCourseProgress()
        {
            Preferences.Remove(courseProgress_Key);
        }

        //reward system
        public static void AddRewardXP(int gainedXP)
        {
            int newXP = GetRewardXP() + gainedXP;

            Preferences.Set(collectedRewardXP_Key, newXP);
        }
        public static int GetRewardXP()
        {
            return Preferences.Get(collectedRewardXP_Key, 0);
        }
        public static void RemoveRewardXP()
        {
            Preferences.Remove(collectedRewardXP_Key);
        }
    }
}
