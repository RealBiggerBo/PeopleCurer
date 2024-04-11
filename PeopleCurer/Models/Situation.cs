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
        public List<string> situationSafetyBehaviour;

        [JsonInclude]
        public int overallFear;

        [JsonInclude]
        public string conclusion;

        public Situation(string situationName, string situationTime, List<SituationFear> situationFears, List<string> situationSafetyBehaviour, int overallFear, string conclusion)
        {
            this.situationName = situationName;
            this.situationTime = situationTime;
            this.situationFears = situationFears;
            this.situationSafetyBehaviour = situationSafetyBehaviour;
            this.overallFear = overallFear;
            this.conclusion = conclusion;
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
}
