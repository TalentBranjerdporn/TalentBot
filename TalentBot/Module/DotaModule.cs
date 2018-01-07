using Discord;
using Discord.Commands;
using TalentBot.Preconditions;
using TalentBot.Common.API;

using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace TalentBot.Module
{
    [Name("Dota")]
    public class DotaModule : ModuleBase<SocketCommandContext>
    {

        enum Medal
        {
            Herald = 2,
            Crusader = 3,
            Archon = 4,
            Legend = 5,
            Ancient = 6,
            Divine = 7
        };

        [Command("medal")]
        [Remarks("Medal check")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task MedalCheck(params string[] cmds)
        {
            // Get player IDs from server log
            List<String> players = OpenDotaAPI.GetPlayerIDs();
            var result = String.Join(", ", players.ToArray());

            Console.WriteLine($"{result}");

            List<int> ranks = new List<int>();
            StringBuilder sb1 = new StringBuilder("| ");
            StringBuilder sb2 = new StringBuilder("| ");

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
            };

            for (int i = 0; i < 10; i++)
            {
                PlayerData data = await OpenDotaAPI.GetPlayerDataAsync(players[i]);
                int rank = 0;
                if (data.rank_tier != null)
                {
                    rank = Convert.ToInt32(data.rank_tier);
                }
                ranks.Add(rank);
                if (i < 5)
                {
                    if (rank == 0)
                    {
                        sb1.Append("Uncalibrated | ");
                    }
                    else
                    {
                        sb1.Append($"{(Medal)(rank / 10)} {rank % 10} | ");
                    }
                } else
                {
                    if (rank == 0)
                    {
                        sb2.Append("Uncalibrated | ");
                    }
                    else
                    {
                        sb2.Append($"{(Medal)(rank / 10)} {rank % 10} | ");
                    }
                }   
            }

            builder.AddField(x =>
            {
                x.Name = "Radiant";
                x.Value = sb1;
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "Dire";
                x.Value = sb2;
                x.IsInline = true;
            });

            //var stringsArray = ranks.Select(i => i.ToString()).ToArray();
            //var output = string.Join(",", stringsArray);

            await ReplyAsync("", false, builder.Build());
        }
    }
}
