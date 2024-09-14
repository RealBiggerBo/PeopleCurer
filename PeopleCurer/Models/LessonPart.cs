using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    [JsonDerivedType(typeof(InfoPage), nameof(InfoPage))]
    [JsonDerivedType(typeof(Evaluation),nameof(Evaluation))]
    [JsonDerivedType(typeof(Question),nameof(Question))]
    [JsonDerivedType(typeof(SymptomCheckQuestion), nameof(SymptomCheckQuestion))]
    public abstract class LessonPart
    {
        public LessonPart() { }
    }

    public sealed class InfoPage(params TextPart[] textParts) : LessonPart
    {
        [JsonInclude]
        public readonly TextPart[] textParts = textParts;
    }

    public sealed class Question(string questionText, params Answer[] answers) : LessonPart
    {
        [JsonInclude]
        public readonly string questionText = questionText;
        [JsonInclude]
        public readonly Answer[] answers = answers;
    }

    public sealed class Evaluation() : LessonPart;

    public sealed class SymptomCheckQuestion(string issue, string description, string lowText, string highText, int answerValue, Dictionary<DateOnly, int> data) : LessonPart
    {
        [JsonInclude]
        public readonly string issue = issue;
        [JsonInclude]
        public readonly string description = description;

        [JsonInclude]
        public readonly string lowText = lowText;
        [JsonInclude]
        public readonly string highText = highText;

        [JsonInclude]
        public int answerValue = answerValue;

        [JsonInclude]
        public Dictionary<DateOnly, int> data = data;
    }
}
