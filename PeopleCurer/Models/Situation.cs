using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    //public sealed class Situation(string situationName, string situationTime, List<SituationFear> situationFears, SafetyBehaviour situationSafetyBehaviour, int overallFear, string conclusion, bool isCompleted)
    //{
    //    [JsonInclude]
    //    public string situationName = situationName;

    //    [JsonInclude]
    //    public string situationTime = situationTime;

    //    [JsonInclude]
    //    public List<SituationFear> situationFears = situationFears;

    //    [JsonInclude]
    //    public SafetyBehaviour situationSafetyBehaviour = situationSafetyBehaviour;

    //    [JsonInclude]
    //    public int overallFear = overallFear;

    //    [JsonInclude]
    //    public string conclusion = conclusion;

    //    [JsonInclude]
    //    public bool isCompleted = isCompleted;
    //}

    public sealed class SituationFear(string fearName, int fearProbability, int actualFearStrength)
    {
        [JsonInclude]
        public string fearName = fearName;

        [JsonInclude]
        public int fearProbability = fearProbability;

        [JsonInclude]
        public int actualFearStrength = actualFearStrength;
    }

    public sealed class SafetyBehaviour(string safetyBehaviourName, int safetyBehaviourAmount)
    {
        [JsonInclude]
        public string safetyBehaviourName = safetyBehaviourName;

        [JsonInclude]
        public int safetyBehaviourAmount = safetyBehaviourAmount;
    }
}
