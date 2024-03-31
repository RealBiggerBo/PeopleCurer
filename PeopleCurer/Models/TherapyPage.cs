using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public sealed class TherapyPage
    {
        [JsonInclude]
        public readonly string name;
        [JsonInclude]
        public readonly string description;
        [JsonInclude]
        public readonly Feature[] features;

        public TherapyPage(string name, string description, params Feature[] features) 
        {
            this.name = name;
            this.description = description;
            this.features = features;
        }
    }
}