using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class Answer
    {
        [JsonInclude]
        public readonly string answerText;
        [JsonInclude]
        public readonly int answerValue;
        [JsonInclude]
        public bool isChosen;

        public Answer(string answerText, int answerValue)
        {
            this.answerText = answerText;
            this.answerValue = answerValue;
            isChosen = false;
        }
    }
}
