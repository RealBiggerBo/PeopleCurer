using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    static class UserInputDataManager
    {
        private static Dictionary<string, string>? userInputData;

        public static EventHandler? userInputDataChangedEvent;

        public static bool GetUserInputData(string key, out string value)
        {
            //check if user data is loaded
            if (userInputData is null)
            {
                //Load data
                LoadData();
            }

            //try returning data
            if (userInputData!.TryGetValue(key, out value!))
            {
                return true;
            }

            return false;
        }

        public static void SetData(string key, string value, bool saveDataToDisk = true)
        {
            //check if user data is loaded
            if (userInputData is null)
            {
                LoadData();
            }

            //check if current val is equal to target value => no processing necessary
            if (GetUserInputData(key, out string oldVal))
            {
                if (oldVal == value)
                    return;
            }

            //add/update data
            if (!userInputData!.TryAdd(key, value))
            {
                userInputData[key] = value;
            }

            //fire data changed event
            userInputDataChangedEvent?.Invoke(null, EventArgs.Empty);

            //serialize data if requested
            if (saveDataToDisk)
            {
                SerializationManager.SaveUserInputData(userInputData);
            }
        }

        private static void LoadData()
        {
            if (!SerializationManager.LoadUserInputData(out userInputData))
            {
                userInputData = new Dictionary<string, string>();
            }
        }
    }
}
