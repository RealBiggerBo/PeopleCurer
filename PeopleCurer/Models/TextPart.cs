using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    [JsonDerivedType(typeof(ListText),nameof(ListText))]
    [JsonDerivedType(typeof(FearCircleDiagram),nameof(FearCircleDiagram))]
    [JsonDerivedType(typeof(UserInputTextBlock), nameof(UserInputTextBlock))]
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

    public sealed class UserInputTextBlock : TextPart
    {
        [JsonInclude]
        public readonly string id;

        public UserInputTextBlock(string id)
        {
            this.id = id;
        }
    }

    public sealed class FearCircleDiagram : TextPart
    {
        [JsonInclude]
        public string trigger;
        [JsonInclude]
        public string perception;
        [JsonInclude]
        public string thoughts;
        [JsonInclude]
        public string emotion;
        [JsonInclude]
        public string change;
        [JsonInclude]
        public string behaviour;

        public FearCircleDiagram(string trigger, string perception, string thoughts, string emotion, string change, string behaviour)
        {
            this.trigger = trigger;
            this.perception = perception;
            this.thoughts = thoughts;
            this.emotion = emotion;
            this.change = change;
            this.behaviour = behaviour;
        }
    }

    public sealed class ListText : TextPart
    {
        [JsonInclude]
        public List<TextBox> texts;
        [JsonInclude]
        public readonly int maxAmount;

        public ListText(List<TextBox> texts, int maxAmount) 
        {
            this.texts = texts;
            this.maxAmount = maxAmount;
        }
    }
}
