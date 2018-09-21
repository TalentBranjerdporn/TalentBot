using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentBot.Common.API
{
    public class YGOData
    {
        // Single card data
        public class CardData
        {
            public string status { get; set; }
            public Card data { get; set; }
        }

        public class Card
        {
            public string name { get; set; }
            public string text { get; set; }
            public string card_type { get; set; }
            public string type { get; set; }
            public string family { get; set; }
            public int? atk { get; set; }
            public int? def { get; set; }
            public int? level { get; set; }
            public string property { get; set; }
        }
    }
}