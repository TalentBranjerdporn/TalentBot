using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentBot.Common.API
{
    public class CardData
    {
        public string status { get; set; }
        public Card card { get; set; }
        public Profile profile { get; set; }    
    }

    public class Card
    {
        public string name { get; set; }
        public string image_path { get; set; }
        public string thumbnail_path { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string number { get; set; }
        public string price_low { get; set; }
        public string price_avg { get; set; }
        public string price_high { get; set; }
        public string tcgplayer_link { get; set; }
        public bool is_monster { get; set; }
        public bool is_spell { get; set; }
        public bool is_illegal { get; set; }
        public bool is_trap { get; set; }
        public bool has_name_condition { get; set; }
        public string species { get; set; }
        public string[] monster_types { get; set; }
        public string attack { get; set; }
        public string defense { get; set; }
        public string stars { get; set; }
        public string attribute { get; set; }
        public bool is_pendulum { get; set; }
        public bool is_xyz { get; set; }
        public bool is_synchro { get; set; }
        public bool is_fusion { get; set; }
        public bool is_link { get; set; }
        public bool is_extra_deck { get; set; }
        public bool has_materials { get; set; }
        public string property { get; set; }
        public Legality legality { get; set; }
    }

    // Level 2 JSON
    public class Legality
    {
        public Tcg TCG { get; set; }
        public Ocg OCG { get; set; }
    }

    // Level 3 JSON
    public class Tcg
    {
        public string Advanced { get; set; }
        public string Traditional { get; set; }
    }

    public class Ocg
    {
        public string Advanced { get; set; }
    }
}