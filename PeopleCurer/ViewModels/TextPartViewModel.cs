using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public abstract class TextPartViewModel : NotifyableBaseObject
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

    public sealed class TextBoxViewModel : TextPartViewModel
    {
        public readonly TextBox textBox;

        public string Question
        {
            get => textBox.question;
        }

        public string Text
        {
            get => textBox.text;
            set
            {
                if(value != textBox.text)
                {
                    textBox.text = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public TextBoxViewModel(TextBox textBox)
        {
            this.textBox = textBox;
        }
    }
}
