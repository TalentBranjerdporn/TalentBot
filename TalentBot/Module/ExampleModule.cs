using Discord.Commands;
using Discord.WebSocket;
using TalentBot.Preconditions;
using System.Threading.Tasks;
using System.IO;
using System;

namespace TalentBot.Modules
{
    [Name("Admin")]
    public class ExampleModule : ModuleBase<SocketCommandContext>
    {

        [Command("say"), Alias("s")]
        [Remarks("Make the bot say something")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Say([Remainder]string text)
        {
            await ReplyAsync(text);
        }

        [Command("bugcat"), Alias("bc")]
        [Remarks("Random bugcat!")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Bugcat()
        {
            string[] arrayBugcats = new string[]
            {
                "images/bugcat/BugCatAngel.PNG",
                "images/bugcat/BugCatConfused.JPG",
                "images/bugcat/BugCatDog.JPG"
            };

            Random rand = new Random();

            string path = arrayBugcats[rand.Next(arrayBugcats.Length)];
            await Context.Channel.SendFileAsync(path);
        }

        [Group("set"), Name("Admin")]
        public class Set : ModuleBase
        {
            [Command("nick")]
            [Remarks("Make the bot say something")]
            [MinPermissions(AccessLevel.User)]
            public async Task Nick([Remainder]string name)
            {
                var user = Context.User as SocketGuildUser;
                await user.ModifyAsync(x => x.Nickname = name);

                await ReplyAsync($"{user.Mention} I changed your name to **{name}**");
            }

            [Command("botnick")]
            [Remarks("Make the bot say something")]
            [MinPermissions(AccessLevel.ServerOwner)]
            public async Task BotNick([Remainder]string name)
            {
                var self = await Context.Guild.GetCurrentUserAsync();
                await self.ModifyAsync(x => x.Nickname = name);

                await ReplyAsync($"I changed my name to **{name}**");
            }
        }
    }
}