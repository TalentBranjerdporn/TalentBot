using Discord;
using Discord.Commands;
using TalentBot.Preconditions;
using TalentBot.Common.API;

using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using HtmlAgilityPack;


namespace TalentBot.Modules
{
    [Name("Yugioh")]
    public class YugiohModule : ModuleBase<SocketCommandContext>
    {
        // Capitalising strings
        private static string Capitalise(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        [Command("ygotext")]
        [Remarks("Search for a card text.\nThanks to http://yugiohprices.com for the API")]
        [MinPermissions(AccessLevel.User)]
        public async Task YGOText(params string[] search)
        {
            string cardName = string.Join(" ", search);

            CardData request = await YGOAPI.GetCardData(cardName);

            if (request.status == "success")
            {
                Card data = request.data;
                var builder = new EmbedBuilder()
                {
                    Color = new Color(114, 137, 218),
                };

                var urlName = string.Join("_", search);
                builder.WithAuthor(x =>
                {
                    x.Name = data.name;
                    x.Url = $"https://yugipedia.com/wiki/{urlName}";
                });

                StringBuilder des = new StringBuilder();

                if (data.card_type.Equals("monster"))
                {
                    des.AppendLine($"Level: {data.level}, " +
                        $"Category: {Capitalise(data.card_type)}, " +
                        $"Type: {data.type}, " +
                        $"Attribute: {Capitalise(data.family)}");
                    des.AppendLine();
                    des.AppendLine(data.text);
                    des.AppendLine();
                    des.AppendLine($"ATK: {data.atk}, " +
                        $"DEF: {data.def}");
                } else if (data.card_type.Equals("spell"))
                {
                    des.AppendLine($"Category: {Capitalise(data.card_type)}, " +
                        $"Property: {data.property}");
                    des.AppendLine();
                    des.AppendLine(data.text);
                } else if (data.card_type.Equals("trap")) {
                    des.AppendLine($"Category: {Capitalise(data.card_type)}, " +
                        $"Property: {data.property}");
                    des.AppendLine();
                    des.AppendLine(data.text);
                }

                builder.Description = des.ToString();

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Card does not exist");
            }
        }

        [Command("ygosupport")]
        [Remarks("Search card support.\nThanks to http://yugiohprices.com for the API")]
        [MinPermissions(AccessLevel.BotOwner)]
        public async Task YGOSupport(params string[] search)
        {
            string cardName = string.Join(" ", search);

            string[] request = await YGOAPI.GetCardSupportData(cardName);

            if (request != null)
            {
                var builder = new EmbedBuilder()
                {
                    Color = new Color(114, 137, 218),
                };
                builder.WithAuthor(x =>
                {
                    x.Name = cardName + " Support";
                });

                StringBuilder des = new StringBuilder();

                foreach (string support in request)
                {
                    des.AppendLine(support);
                }

                builder.Description = des.ToString();

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            } else
            {
                await Context.Channel.SendMessageAsync("Card does not exist");
            }
        }
    }
}