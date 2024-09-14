using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    [JsonDerivedType(typeof(NormalLesson),nameof(NormalLesson))]
    [JsonDerivedType(typeof(SituationLesson),nameof(SituationLesson))]
    [JsonDerivedType(typeof(ThoughtTestLesson),nameof(ThoughtTestLesson))]
    public abstract class Lesson()
    {
        [JsonInclude]
        public bool isActive = false;
        [JsonInclude]
        public bool isCompleted = false;
    }

    public class NormalLesson : Lesson
    {
        [JsonInclude]
        public readonly string lessonName;
        [JsonInclude]
        public readonly string lessonDescription;
        [JsonInclude]
        public readonly bool showStartPage;
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public readonly LessonType lessonType;
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ColorTheme colorTheme;
        [JsonInclude]
        public readonly LessonPart[] lessonParts;
        [JsonInclude]
        public int lessonReward;
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public readonly SaveFrequency saveFrequency;

        public NormalLesson(string lessonName, string lessonDescription, bool showStartPage, LessonType lessonType, SaveFrequency saveFrequency, int lessonReward, params LessonPart[] lessonParts)
        {
            this.lessonName = lessonName;
            this.lessonDescription = lessonDescription;

            this.showStartPage = showStartPage;

            this.lessonParts = lessonParts;

            this.isActive = false;

            this.lessonReward = lessonReward;

            this.isCompleted = false;

            this.saveFrequency = saveFrequency;

            this.lessonType = lessonType;

            this.colorTheme = ColorTheme.Blue;
        }
    }

    public class SituationLesson() : Lesson()
    {
        [JsonInclude]
        public string situationName = "neue Situation";

        [JsonInclude]
        public string situationTime = string.Empty;

        [JsonInclude]
        public List<SituationFear> situationFears = [];

        [JsonInclude]
        public SafetyBehaviour situationSafetyBehaviour = new("neues Verhalten", 50);

        [JsonInclude]
        public int overallFear = 50;

        [JsonInclude]
        public string conclusion = string.Empty;

        [JsonInclude] 
        public bool isOngoing = false;
    }

    public class ThoughtTestLesson() : Lesson()
    {
        [JsonInclude]
        public string thoughtTestName = "neuer Gedankentest";

        [JsonInclude]
        public string situationDescription = string.Empty;

        [JsonInclude]
        public Thought mainThought = new("neuer Gedanke", 50);

        [JsonInclude]
        public List<Emotion> emotions = [];

        [JsonInclude]
        public List<Thought> alternateThoughts = [];

        [JsonInclude]
        public string conclusion = string.Empty;
    }

    //TODO 
    public class AudioRelaxLesson : Lesson
    {

    }

    public enum SaveFrequency
    {
        OnCompletion,
        Never,
        Always
    }

    public enum LessonType
    {
        Informative,
        Interactive,
        Quiz,
        Practice,
    }
}
