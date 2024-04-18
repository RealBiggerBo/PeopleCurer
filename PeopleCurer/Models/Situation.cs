using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class Situation
    {
        [JsonInclude]
        public string situationName;

        [JsonInclude]
        public string situationTime;

        [JsonInclude]
        public List<SituationFear> situationFears;

        [JsonInclude]
        public List<SafetyBehaviour> situationSafetyBehaviours;

        [JsonInclude]
        public int overallFear;

        [JsonInclude]
        public string conclusion;

        [JsonInclude]
        public bool isFinished;

        public Situation(string situationName, string situationTime, List<SituationFear> situationFears, List<SafetyBehaviour> situationSafetyBehaviours, int overallFear, string conclusion, bool isFinished)
        {
            this.situationName = situationName;
            this.situationTime = situationTime;
            this.situationFears = situationFears;
            this.situationSafetyBehaviours = situationSafetyBehaviours;
            this.overallFear = overallFear;
            this.conclusion = conclusion;
            this.isFinished = isFinished;
        }
    }

    public sealed class SituationFear
    {
        [JsonInclude]
        public string fearName;

        [JsonInclude]
        public int fearProbability;

        [JsonInclude]
        public int actualFearStrength;

        public SituationFear(string fearName, int fearProbability, int actualFearStrength)
        {
            this.fearName = fearName;
            this.fearProbability = fearProbability;
            this.actualFearStrength = actualFearStrength;
        }
    }

    public sealed class SafetyBehaviour
    {
        [JsonInclude]
        public string safetyBehaviourName;

        [JsonInclude]
        public int safetyBehaviourAmount;

        public SafetyBehaviour(string safetyBehaviourName, int safetyBehaviourAmount)
        {
            this.safetyBehaviourName = safetyBehaviourName;
            this.safetyBehaviourAmount = safetyBehaviourAmount;
        }
    }
}
