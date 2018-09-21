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
    class YGOAPI
    {
        public static async Task<YGOData.CardData> GetCardData(string cardName)
        {
            string result = await RequestHandler.GET($"http://yugiohprices.com/api/card_data/{cardName}");

            if (result != null)
            {
                return JsonConvert.DeserializeObject<YGOData.CardData>(result);
            } else
            {
                Console.WriteLine("CardData Failed");
                return null;
            }
        }
    }
}
