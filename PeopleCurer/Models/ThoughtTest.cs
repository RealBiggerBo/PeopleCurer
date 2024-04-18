using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class ThoughtTest
    {
        [JsonInclude]
        public string situationName;

        [JsonInclude]
        public Thought mainThought;

        [JsonInclude]
        public List<Emotion> emotions;

        [JsonInclude]
        public List<Thought> alternateThoughts;

        [JsonInclude]
        public string conclusion;

        [JsonInclude]
        public bool isFinished;

        public ThoughtTest(string situationName, Thought mainThought, List<Emotion> emotions, List<Thought> alternateThoughts, string conclusion, bool isFinished)
        {
            this.situationName = situationName;
            this.mainThought = mainThought;
            this.emotions = emotions;
            this.alternateThoughts = alternateThoughts;
            this.conclusion = conclusion;
            this.isFinished = isFinished;
        }
    }

    public sealed class Thought
    {
        [JsonInclude]
        public string thoughtName;

        [JsonInclude]
        public int thoughtProbability;

        public Thought(string thoughtName, int thoughtProbability)
        {
            this.thoughtName = thoughtName;
            this.thoughtProbability = thoughtProbability;
        }
    }

    public sealed class Emotion
    {
        [JsonInclude]
        public string emotionName;

        public Emotion(string emotionName)
        {
            this.emotionName = emotionName;
        }
    }
}
