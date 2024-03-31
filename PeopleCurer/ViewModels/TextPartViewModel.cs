using PeopleCurer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public abstract class TextPartViewModel
    {
        public TextPartViewModel() { }
    }

    public sealed class TextBlockViewModel : TextPartViewModel
    {
        public readonly TextBlock textBlock;

        public string Text
        {
            get => textBlock.text;
        }

        public TextBlockViewModel(TextBlock textBlock)
        {
            this.textBlock = textBlock;
        }
    }

    public sealed class EnumerationViewModel : TextPartViewModel
    {
        public readonly Enumeration enumeration;

        public string[] Texts
        {
            get => enumeration.texts;
        }

        public EnumerationViewModel(Enumeration enumeration)
        {
            this.enumeration = enumeration;
        }
    }
}
