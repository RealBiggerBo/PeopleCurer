using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    internal static class RewardManager
    {
        public static EventHandler? OnRewardXPChanged;

        /// <summary>
        /// Adds the specified amount to the current xp. Then calls reward changed event
        /// </summary>
        /// <param name="xp">the amount of xp to be added</param>
        public static void AddRewardXP(int xp) 
        {
            PreferenceManager.AddRewardXP(xp);
            OnRewardXPChanged?.Invoke(null, EventArgs.Empty);
        }

        public static int GetCurrentLevel()
        {
            int curXP = PreferenceManager.GetRewardXP();

            int curLevel;
            if (curXP < 500)
            {
                curLevel = 1;
            }
            else if (curXP < 2000)
            {
                curLevel = 2;
            }
            else if (curXP < 8000)
            {
                curLevel = 3;
            }
            else if (curXP < 20000)
            {
                curLevel = 4;
            }
            else
            {
                curLevel = 5;
            }

            return curLevel;
        }

        public static float GetCurrentLevelProgress()
        {
            int curXP = PreferenceManager.GetRewardXP();

            int curLevel = GetCurrentLevel();

            return curLevel switch
            {
                1 => curXP / 500f,
                2 => (curXP - 500) / 2000f,
                3 => (curXP - 2000) / 8000f,
                4 => (curXP - 8000) / 20000f,
                _ => MathF.Min((curXP - 20000) / 40000f, 1f),
            };
        }
    }
}
