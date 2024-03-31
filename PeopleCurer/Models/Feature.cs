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
    [JsonDerivedType(typeof(Statistics),nameof(Statistics))]
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

    public sealed class Statistics : Feature
    {
        [JsonInclude]
        public readonly string statisticsName;
        [JsonInclude]
        public readonly string statisticsDescription;
        [JsonInclude]
        public readonly string statisticsID;
        [JsonIgnore]
        public Dictionary<DateTime, int> data;

        public Statistics(string statisticsName, string statisticsDescription, string statisticsID)
        {
            this.statisticsName = statisticsName;
            this.statisticsDescription = statisticsDescription;

            this.statisticsID = statisticsID;

            SerializationManager.LoadSymptomCheckResults(out data, statisticsID);
        }
    }

    //statistics etc
}
