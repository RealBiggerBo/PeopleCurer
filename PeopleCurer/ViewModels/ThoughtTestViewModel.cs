using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using PeopleCurer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public sealed class ThoughtViewModel : NotifyableBaseObject
    {
        public readonly Thought thought;

        public string ThoughtName
        {
            get => thought.thoughtName;
            set
            {
                if (thought.thoughtName != value)
                {
                    thought.thoughtName = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public int ThoughtProbability
        {
            get => thought.thoughtProbability;
            set
            {
                if (thought.thoughtProbability != value)
                {
                    thought.thoughtProbability = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public ThoughtViewModel(Thought thought)
        {
            this.thought = thought;
        }
    }

    public sealed class EmotionViewModel : NotifyableBaseObject
    {
        public readonly Emotion emotion;

        public string EmotionName
        {
            get => emotion.emotionName;
            set
            {
                if (emotion.emotionName != value)
                {
                    emotion.emotionName = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public EmotionViewModel(Emotion emotion)
        {
            this.emotion = emotion;
        }
    }
}
