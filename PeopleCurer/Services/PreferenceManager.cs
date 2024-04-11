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
    }
}
