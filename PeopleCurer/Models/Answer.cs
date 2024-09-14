using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class Answer(string answerText, bool isCorrect)
    {
        [JsonInclude]
        public readonly string answerText = answerText;
        [JsonInclude]
        public readonly bool isCorrect = isCorrect;
        [JsonInclude]
        public bool isChosen = false;
    }
}
