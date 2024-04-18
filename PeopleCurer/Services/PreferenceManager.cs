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
    }
}
