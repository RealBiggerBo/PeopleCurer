using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleCurer.Models
{
    public class TherapyPage
    {
        [JsonInclude]
        public readonly string name;
        [JsonInclude]
        public readonly Feature[] features;

        public TherapyPage(string name, params Feature[] features) 
        {
            this.name = name;
            this.features = features;
        }
    }
}