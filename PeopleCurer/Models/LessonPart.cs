using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    [JsonDerivedType(typeof(Evaluation),nameof(Evaluation))]
    [JsonDerivedType(typeof(Question),nameof(Question))]
    public abstract class LessonPart
    {
        public LessonPart() { }
    }

    public class Question : LessonPart
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

    public class Evaluation : LessonPart
    {
        [JsonInclude]
        public readonly Dictionary<int, string> evaluationResults;

        public Evaluation(Dictionary<int, string> evaluationResults)
        {
            this.evaluationResults = evaluationResults;
        }
    }
}
