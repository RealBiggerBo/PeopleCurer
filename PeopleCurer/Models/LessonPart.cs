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

    public sealed class InfoPage : LessonPart
    {
        [JsonInclude]
        public readonly TextPart[] textParts;

        public InfoPage(params TextPart[] textParts) 
        {
            this.textParts = textParts;
        }
    }

    public sealed class Question : LessonPart
    {
        [JsonInclude]
        public readonly string issue;
        [JsonInclude]
        public readonly Answer[] answers;

        public Question(string issue, params Answer[] answers) 
        {
            this.issue = issue;

            this.answers = answers;
        }
    }

    public sealed class Evaluation : LessonPart
    {
        [JsonInclude]
        public readonly Dictionary<int, string> evaluationResults;

        public Evaluation(Dictionary<int, string> evaluationResults)
        {
            this.evaluationResults = evaluationResults;
        }
    }

    public sealed class SymptomCheckQuestion : LessonPart
    {
        [JsonInclude]
        public readonly string issue;
        [JsonInclude]
        public readonly string description;

        [JsonInclude]
        public readonly string lowText;
        [JsonInclude]
        public readonly string highText;

        [JsonInclude]
        public int answerValue;

        [JsonInclude]
        public Dictionary<DateOnly, int> data;

        public SymptomCheckQuestion(string issue, string description, string lowText, string highText, int answerValue, Dictionary<DateOnly, int> data)
        {
            this.issue = issue;
            this.description = description;

            this.lowText = lowText;
            this.highText = highText;

            this.answerValue = answerValue;

            this.data = data;
        }
    }
}
