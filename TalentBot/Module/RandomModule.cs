using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TalentBot.Preconditions;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace TalentBot.Module
{
    [Name("Random")]
    public class RandomModule : ModuleBase<SocketCommandContext>
    {
        private Random rand = new Random();

        [Command("bugcat")]
        [Remarks("Random bugcat!")]
        [MinPermissions(AccessLevel.User)]
        public async Task Bugcat()
        {
            string path = @"C:\Users\Talent\Dropbox\Pictures\Cute\Bugcat";
            string[] bugcats = Directory.GetFiles(path);

            string randomBugcat = bugcats[rand.Next(bugcats.Length)];
            await Context.Channel.SendFileAsync(randomBugcat);
        }
    }
}
