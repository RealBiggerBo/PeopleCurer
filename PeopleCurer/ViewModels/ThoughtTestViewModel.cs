using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
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
    public sealed class ThoughtTestViewModel : NotifyableBaseObject
    {
        public readonly ThoughtTest thoughtTest;

        public string SituationName 
        { 
            get => thoughtTest.situationName;
            set
            {
                if(value != thoughtTest.situationName)
                {
                    thoughtTest.situationName = value;
                    base.RaisePropertyChanged();
                }
            }
        }
       
        public ThoughtViewModel MainThought { get; }

        public ObservableCollection<EmotionViewModel> Emotions { get; }

        public ObservableCollection<ThoughtViewModel> AlternateThoughts { get; }

        public string Conclusion
        {
            get => thoughtTest.conclusion;
            set
            {
                if(thoughtTest.conclusion != value)
                {
                    thoughtTest.conclusion = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public bool IsFinished
        {
            get => thoughtTest.isFinished;
            set
            {
                if(thoughtTest.isFinished != value)
                {
                    thoughtTest.isFinished = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand GoToThoughtTestPage { get; }
        public DelegateCommand AddEmotion { get; }
        public DelegateCommand DeleteEmotion { get; }
        public DelegateCommand AddAlternateThought { get; }
        public DelegateCommand DeleteAlternateThought { get; }
        public DelegateCommand FinishThoughtTest { get; }
        public DelegateCommand CompletedThoughtTest { get; }

        public EventHandler OnTestFinishEditEvent;

        public ThoughtTestViewModel(ThoughtTest thoughtTest)
        {
            this.thoughtTest = thoughtTest;

            this.MainThought = new ThoughtViewModel(thoughtTest.mainThought);

            this.Emotions = new ObservableCollection<EmotionViewModel>();
            for (int i = 0; i < thoughtTest.emotions.Count; i++)
            {
                Emotions.Add(new EmotionViewModel(thoughtTest.emotions[i]));
            }

            this.AlternateThoughts = new ObservableCollection<ThoughtViewModel>();
            for (int i = 0; i < thoughtTest.alternateThoughts.Count; i++)
            {
                AlternateThoughts.Add(new ThoughtViewModel(thoughtTest.alternateThoughts[i]));
            }

            GoToThoughtTestPage = new DelegateCommand((obj) =>
            {
                if (this.IsFinished == false)
                {
                    Shell.Current.GoToAsync(nameof(ThoughtTestEditPage),
                        new Dictionary<string, object>
                        {
                            ["ThoughtTestVM"] = this
                        });
                }
                else
                {
                    Shell.Current.GoToAsync(nameof(ThoughtTestCompletedPage),
                        new Dictionary<string, object>
                        {
                            ["ThoughtTestVM"] = this
                        });
                }
            });
            AddEmotion = new DelegateCommand((obj) =>
            {
                Emotion newEmotion = new Emotion("neue Emotion");

                //Model
                thoughtTest.emotions ??= [];
                thoughtTest.emotions.Add(newEmotion);
                //VM
                Emotions.Add(new EmotionViewModel(newEmotion));
            });
            DeleteEmotion = new DelegateCommand((obj) =>
            {
                if (obj is EmotionViewModel emotion)
                {
                    //Model
                    thoughtTest.emotions?.Remove(emotion.emotion);
                    //VM
                    Emotions.Remove(emotion);
                }
            });
            AddAlternateThought = new DelegateCommand((obj) =>
            {
                Thought newThought = new Thought("alternativer Gedanke", 50);

                //Model
                thoughtTest.alternateThoughts ??= [];
                thoughtTest.alternateThoughts.Add(newThought);
                //VM
                AlternateThoughts.Add(new ThoughtViewModel(newThought));
            });
            DeleteAlternateThought = new DelegateCommand((obj) =>
            {
                if (obj is ThoughtViewModel thought)
                {
                    //Model
                    thoughtTest.alternateThoughts?.Remove(thought.thought);
                    //VM
                    AlternateThoughts.Remove(thought);
                }
            });
            FinishThoughtTest = new DelegateCommand((obj) =>
            {
                Shell.Current.GoToAsync("../" + nameof(ThoughtTestFinishPage),
                        new Dictionary<string, object>
                        {
                            ["ThoughtTestVM"] = this
                        });
            });
            CompletedThoughtTest = new DelegateCommand((obj) =>
            {
                this.IsFinished = true;
                OnTestFinishEditEvent?.Invoke(this, EventArgs.Empty);
                Shell.Current.GoToAsync("../" + nameof(ThoughtTestCompletedPage),
                    new Dictionary<string, object>
                    {
                        ["ThoughtTestVM"] = this
                    });
            });
        }
    }

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
