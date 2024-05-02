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
    [JsonDerivedType(typeof(BehaviourExperiment), nameof(BehaviourExperiment))]
    [JsonDerivedType(typeof(ThoughtTestContainer), nameof(ThoughtTestContainer))]
    [JsonDerivedType(typeof(RelaxationProcedureContainer), nameof(RelaxationProcedureContainer))]
    [JsonDerivedType(typeof(StrengthsCourse), nameof(StrengthsCourse))]
    public abstract class Feature;

    public sealed class Course : Feature
    {
        [JsonInclude]
        public readonly string courseName;
        [JsonInclude]
        public readonly string description;
        [JsonInclude]
        public readonly Lesson[] lessons;
        [JsonInclude]
        public bool isActive;

        public Course(string courseName, string description, params Lesson[] lessons)
        {
            this.courseName = courseName;
            this.description = description;

            this.lessons = lessons;

            this.isActive = false;
        }
    }

    public sealed class BehaviourExperiment : Feature
    {
        [JsonInclude]
        public List<Situation> situations;

        public BehaviourExperiment(List<Situation> situations)
        {
            this.situations = situations;
        }
    }

    public sealed class ThoughtTestContainer : Feature
    {
        [JsonInclude]
        public List<ThoughtTest> thoughtTests;

        public ThoughtTestContainer(List<ThoughtTest> thoughtTests)
        {
            this.thoughtTests = thoughtTests;
        }
    }

    public sealed class RelaxationProcedureContainer : Feature
    {
        [JsonInclude]
        public List<RelaxationProcedure> relaxationProcedures;

        public RelaxationProcedureContainer(List<RelaxationProcedure> relaxationProcedures)
        {
            this.relaxationProcedures = relaxationProcedures;
        }
    }

    public sealed class StrengthsCourse : Feature
    {
        [JsonInclude]
        public Lesson strengthsLesson;
        [JsonInclude]
        public Lesson successMomentsLesson;
        [JsonInclude]
        public Lesson trainingSuccessLesson;

        public StrengthsCourse(Lesson strengthsLesson, Lesson successMomentsLesson, Lesson trainingSuccessLesson)
        {
            this.strengthsLesson = strengthsLesson;
            this.successMomentsLesson = successMomentsLesson;
            this.trainingSuccessLesson = trainingSuccessLesson;
        }
    }
}
