using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TalentBot.Common.API
{
    class YGOHubAPI
    {
        public static async Task<CardData> GetCardData(string cardName)
        {
            string result = await RequestHandler.GET($"https://www.ygohub.com/api/card_info?name={cardName}");

            if (result != null)
            {
                return JsonConvert.DeserializeObject<CardData>(result);
            } else
            {
                Console.WriteLine("CardData Failed");
                return null;
            }
        }
    }
}
