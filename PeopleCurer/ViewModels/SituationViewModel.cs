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
    public sealed class SituationViewModel : NotifyableBaseObject
    {
        public readonly Situation situation;

        public string SituationName { get => situation.situationName; }

        public string SituationTime { get => situation.situationTime; }

        public List<SituationFear> SituationFears { get => situation.situationFears; }

        public List<string> SituationSafetyBehaviour { get => situation.situationSafetyBehaviour; }

        public int OverallFear { get => situation.overallFear; }

        public string Conclusion { get => situation.conclusion; }

        public SituationViewModel(Situation situation)
        {
            this.situation = situation;
        }
    }
}
