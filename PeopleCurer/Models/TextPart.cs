using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    [JsonDerivedType(typeof(TextBlock), nameof(TextBlock))]
    [JsonDerivedType(typeof(Enumeration), nameof(Enumeration))]
    [JsonDerivedType(typeof(TextBox), nameof(TextBox))]
    public abstract class TextPart
    {
        public TextPart() { }
    }

    public sealed class TextBlock : TextPart
    {
        [JsonInclude]
        public readonly string text;

        public TextBlock(string text) 
        {
            this.text = text;
        }
    }

    public sealed class Enumeration : TextPart
    {
        [JsonInclude]
        public readonly string[] texts;

        public Enumeration(params string[] texts)
        {
            this.texts = texts;
        }
    }

    public sealed class TextBox : TextPart 
    {
        [JsonInclude]
        public readonly string question;

        [JsonInclude]
        public string text;

        public TextBox(string question, string text)
        {
            this.question = question;

            this.text = text;
        }
    }
}
