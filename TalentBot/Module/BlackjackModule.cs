using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TalentBot.Preconditions;

using System;
using System.Threading.Tasks;

namespace TalentBot.Module
{
    [Name("Blackjack")]
    public class BlackjackModule : ModuleBase<SocketCommandContext>
    {

        Random rand = new Random();

        [Command("bj")]
        [Remarks("Play a game of blackjack")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Blackjack(string cmds)
        {
            if (cmds[0].Equals("play"))
            {
                await ReplyAsync("Lets play a game of Blackjack");


                var builder = new EmbedBuilder()
                {
                    Color = new Discord.Color(114, 137, 218),
                    Description = "The draw"
                };

                builder.AddField(x =>
                {
                    x.Name = "My cards";
                    x.Value = "[ 4H ] [ X ]";
                    x.IsInline = true;
                });

                builder.AddField(x =>
                {
                    x.Name = "Your cards";
                    x.Value = "[ 9C ]  [ AS ]";
                    x.IsInline = true;
                });
                await ReplyAsync("", false, builder.Build());
            } 
        }
    }
}
