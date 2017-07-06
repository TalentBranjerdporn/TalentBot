using Discord.Commands;
using Discord.WebSocket;
using TalentBot.Preconditions;

using System;
using System.Threading.Tasks;

namespace TalentBot.Module
{
    [Name("RoShamBo")]
    public class RPSModule : ModuleBase<SocketCommandContext>
    {

        private string[] rpsStrings = new string[3] { "rock", "paper", "scissors" };
        private string[] rpsIcons = new string[3] { ":fist:", ":page_facing_up:", ":scissors:" };

        Random rand = new Random();

        [Command("rps")]
        [Remarks("Play a game of rock paper scissors")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Rps([Remainder]string rps)
        {
            int rpsBot = rand.Next(rpsStrings.Length);
            int rpsPlayer = Array.IndexOf(rpsStrings, rps);

            int rpsResult = rpsPlayer - rpsBot;

            string played = rpsStrings[rpsBot];
            string rpsIcon = rpsIcons[rpsBot];

            if (rpsResult == 0)
            {
                await ReplyAsync($"I played {played} {rpsIcon} \ndraw");
            } else if (rpsResult == 1 || rpsResult == -2)
            {
                await ReplyAsync($"I played {played} {rpsIcon} \nI win (I promise I make the choice randomly and not based on your pick)");
            } else if (rpsResult == -1 || rpsResult == 2)
            {
                await ReplyAsync($"I played {played} {rpsIcon} \nHow lucky you won");
            }
        }
    }
}
