using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class Lesson
    {
        [JsonInclude]
        public readonly string lessonName;
        [JsonInclude]
        public readonly LessonPart[] lessonParts;
        [JsonInclude]
        public bool isActive;
        [JsonInclude]
        public int lessonReward;
        [JsonInclude]
        public bool isCompleted;

        public Lesson(string lessonName, int lessonReward, params LessonPart[] lessonParts)
        {
            this.lessonName = lessonName;

            this.lessonParts = lessonParts;

            this.isActive = false;

            this.lessonReward = lessonReward;

            this.isCompleted = false;
        }
    }
}
