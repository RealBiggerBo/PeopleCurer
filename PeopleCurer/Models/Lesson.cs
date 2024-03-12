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

        public Lesson(string lessonName, params LessonPart[] lessonParts)
        {
            this.lessonName = lessonName;

            this.lessonParts = lessonParts;

            this.isActive = false;
        }
    }
}
