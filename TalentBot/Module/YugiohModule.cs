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
        [Command("ygocard")]
        [Remarks("Search for a card.\nThanks to https://www.ygohub.com for the API")]
        [MinPermissions(AccessLevel.BotOwner)]
        public async Task YGOCard(params string[] search)
        {
            string cardName = string.Join(" ", search);

            CardData request = await YGOHubAPI.GetCardData(cardName);

            if (request.status == "success")
            {
                var card = request.card;
                var builder = new EmbedBuilder()
                {
                    Color = new Color(114, 137, 218),
                };

                builder.ThumbnailUrl = card.image_path;

                var urlName = string.Join("_", search);
                builder.WithAuthor(x =>
                {
                    x.Name = card.name;
                    x.Url = $"https://yugipedia.com/wiki/{urlName}";
                });

                StringBuilder des = new StringBuilder();

                if (card.is_monster)
                {
                    des.AppendLine($"Level: {card.stars}, " +
                        $"Category: {card.type}, " +
                        $"Type: {card.species}/{card.monster_types[0]}, " +
                        $"Attribute: {card.attribute}");
                    des.AppendLine();
                    des.AppendLine(card.text);
                    des.AppendLine();
                    des.AppendLine($"ATK: {card.attack}, " +
                        $"DEF: {card.defense}");
                }
                else if (card.is_spell)
                {
                    des.AppendLine($"Category: {card.type}, " +
                        $"Property: {card.property}");
                    des.AppendLine();
                    des.AppendLine(card.text);
                }
                else if (card.is_trap)
                {
                    des.AppendLine($"Category: {card.type}, " +
                        $"Property: {card.property}");
                    des.AppendLine();
                    des.AppendLine(card.text);
                }

                builder.Description = des.ToString();

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Card does not exist");
            }
        }
    }
}