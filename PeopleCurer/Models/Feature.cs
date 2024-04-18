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
}
