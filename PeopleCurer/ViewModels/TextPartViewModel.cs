using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
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

    public sealed class UserInputTextBlockViewModel : TextPartViewModel
    {
        public readonly UserInputTextBlock userInputTextBlock;

        private string text;
        public string Text
        {
            get => text;
            set
            {
                if(value != text)
                {
                    this.text = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public UserInputTextBlockViewModel(UserInputTextBlock userInputTextBlock)
        {
            this.userInputTextBlock = userInputTextBlock;

            if(!UserInputDataManager.GetUserInputData(userInputTextBlock.id, out string text))
            {
                Text = string.Empty;
            }
            Text = text;

            UserInputDataManager.userInputDataChangedEvent += UserInputDataChanged;
        }

        private void UserInputDataChanged(object? obj, EventArgs e)
        {
            if (!UserInputDataManager.GetUserInputData(userInputTextBlock.id, out string text))
            {
                Text = string.Empty;
            }
            Text = text;
        }
    }

    public sealed class FearCircleDiagramViewModel : TextPartViewModel
    {
        public readonly FearCircleDiagram fearCircleDiagram;
        
        public string Trigger
        {
            get => fearCircleDiagram.trigger;
            set
            {
                if(value != fearCircleDiagram.trigger)
                {
                    fearCircleDiagram.trigger = value;
                    base.RaisePropertyChanged();
                }
            }
        }
        
        public string Perception
        {
            get => fearCircleDiagram.perception;
            set
            {
                if (value != fearCircleDiagram.perception)
                {
                    fearCircleDiagram.perception = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string Thoughts
        {
            get => fearCircleDiagram.thoughts;
            set
            {
                if (value != fearCircleDiagram.thoughts)
                {
                    fearCircleDiagram.thoughts = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string Emotion
        {
            get => fearCircleDiagram.emotion;
            set
            {
                if (value != fearCircleDiagram.emotion)
                {
                    fearCircleDiagram.emotion = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string Change
        {
            get => fearCircleDiagram.change;
            set
            {
                if (value != fearCircleDiagram.change)
                {
                    fearCircleDiagram.change = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public string Behaviour
        {
            get => fearCircleDiagram.behaviour;
            set
            {
                if (value != fearCircleDiagram.behaviour)
                {
                    fearCircleDiagram.behaviour = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public FearCircleDiagramViewModel(FearCircleDiagram fearCircleDiagram)
        {
            this.fearCircleDiagram = fearCircleDiagram;
        }
    }
}
