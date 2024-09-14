using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class Thought(string thoughtName, int thoughtProbability)
    {
        [JsonInclude]
        public string thoughtName = thoughtName;

        [JsonInclude]
        public int thoughtProbability = thoughtProbability;
    }

    public sealed class Emotion(string emotionName)
    {
        [JsonInclude]
        public string emotionName = emotionName;
    }
}
