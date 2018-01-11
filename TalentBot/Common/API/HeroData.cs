using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentBot.Common.API
{
    public class HeroData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string localized_name { get; set; }
        public string img { get; set; }
        public string icon { get; set; }

        public int pro_win { get; set; }
        public int pro_pick { get; set; }
        public int hero_id { get; set; }
        public int pro_ban { get; set; }
        [JsonProperty(PropertyName = "1_pick")]
        public int one_pick { get; set; }
        [JsonProperty(PropertyName = "1_win")]
        public int one_win { get; set; }
        [JsonProperty(PropertyName = "2_pick")]
        public int two_pick { get; set; }
        [JsonProperty(PropertyName = "2_win")]
        public int two_win { get; set; }
        [JsonProperty(PropertyName = "3_pick")]
        public int three_pick { get; set; }
        [JsonProperty(PropertyName = "3_win")]
        public int three_win { get; set; }
        [JsonProperty(PropertyName = "4_pick")]
        public int four_pick { get; set; }
        [JsonProperty(PropertyName = "4_win")]
        public int four_win { get; set; }
        [JsonProperty(PropertyName = "5_pick")]
        public int five_pick { get; set; }
        [JsonProperty(PropertyName = "5_win")]
        public int five_win { get; set; }
        [JsonProperty(PropertyName = "6_pick")]
        public int six_pick { get; set; }
        [JsonProperty(PropertyName = "6_win")]
        public int six_win { get; set; }
        [JsonProperty(PropertyName = "7_pick")]
        public int seven_pick { get; set; }
        [JsonProperty(PropertyName = "7_win")]
        public int seven_win { get; set; }

    }
}
