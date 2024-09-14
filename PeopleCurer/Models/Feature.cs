using PeopleCurer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    [JsonDerivedType(typeof(Course),nameof(Course))]
    [JsonDerivedType(typeof(BehaviourExperimentContainer), nameof(BehaviourExperimentContainer))]
    [JsonDerivedType(typeof(ThoughtTestContainer), nameof(ThoughtTestContainer))]
    [JsonDerivedType(typeof(RelaxationProcedureContainer), nameof(RelaxationProcedureContainer))]
    [JsonDerivedType(typeof(StrengthsCourse), nameof(StrengthsCourse))]
    [JsonDerivedType(typeof(BodyScanContainer), nameof(BodyScanContainer))]
    [JsonDerivedType(typeof(ResponseTrainingContainer), nameof(ResponseTrainingContainer))]
    public abstract class Feature;

    public sealed class Course : Feature
    {
        [JsonInclude]
        public readonly string courseName;
        [JsonInclude]
        public readonly string description;
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public readonly ColorTheme courseColor;
        [JsonInclude]
        public readonly Lesson[] lessons;
        [JsonInclude]
        public bool isActive;


        public Course(string courseName, string description, ColorTheme courseColor, params Lesson[] lessons)
        {
            this.courseName = courseName;
            this.description = description;

            this.courseColor = courseColor;

            this.lessons = lessons;

            this.isActive = false;
        }
    }

    public sealed class BehaviourExperimentContainer : Feature
    {
        [JsonInclude]
        public List<SituationLesson> situations;
        [JsonIgnore]
        public int requiredCourseProgress;

        public BehaviourExperimentContainer(List<SituationLesson> situations)
        {
            this.situations = situations;
            this.requiredCourseProgress = 306;
        }
    }

    public sealed class ThoughtTestContainer(List<ThoughtTestLesson> thoughtTests) : Feature
    {
        [JsonInclude]
        public List<ThoughtTestLesson> thoughtTests = thoughtTests;
        [JsonIgnore]
        public readonly int requiredCourseProgress = 111;
    }

    public sealed class RelaxationProcedureContainer(List<RelaxationProcedure> relaxationProcedures) : Feature
    {
        [JsonInclude]
        public readonly List<RelaxationProcedure> relaxationProcedures = relaxationProcedures;
        [JsonIgnore]
        public readonly int requiredCourseProgress = 403;
    }

    public sealed class BodyScanContainer() : Feature
    {
        [JsonIgnore]
        public readonly int requiredCourseProgress = 202;
    }

    public sealed class ResponseTrainingContainer(ResponseTraining[] responseTrainings) : Feature
    {
        [JsonInclude]
        public readonly ResponseTraining[] responseTrainings = responseTrainings;
        [JsonIgnore]
        public readonly int requiredCourseProgress = 202;
    }

    public sealed class StrengthsCourse : Feature
    {
        [JsonInclude]
        public NormalLesson strengthsLesson;
        [JsonInclude]
        public NormalLesson successMomentsLesson;
        [JsonInclude]
        public NormalLesson trainingSuccessLesson;

        public StrengthsCourse(NormalLesson strengthsLesson, NormalLesson successMomentsLesson, NormalLesson trainingSuccessLesson)
        {
            this.strengthsLesson = strengthsLesson;
            this.successMomentsLesson = successMomentsLesson;
            this.trainingSuccessLesson = trainingSuccessLesson;
        }
    }

    public enum ColorTheme
    {
        Blue,
        Green,
        Red,
        Purple,
        Yellow,
    }
}
