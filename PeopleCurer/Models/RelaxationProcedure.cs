using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class RelaxationProcedure
    {
        [JsonInclude]
        public readonly string title;

        [JsonInclude]
        public readonly string explanation;

        [JsonInclude]
        public readonly string effectiveness;

        public RelaxationProcedure(string title, string explanation, string effectiveness) 
        {
            this.title = title;
            this.explanation = explanation;
            this.effectiveness = effectiveness;
        }
    }
}
