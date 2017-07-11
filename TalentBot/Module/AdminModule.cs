using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TalentBot.Preconditions;

using System.Threading.Tasks;
using System.IO;
using System;


namespace TalentBot.Modules
{
    [Name("Admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
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
        [MinPermissions(AccessLevel.User)]
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

        [Command("purge")]
        [Summary("Deletes the specified amount of messages.")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task PurgeChat(int amount)
        {

            await Context.Channel.DeleteMessagesAsync((await Context.Channel.GetMessagesAsync(amount+1).Flatten()));
            //var messages = await Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();

            //await Context.Channel.DeleteMessagesAsync(messages);
            //const int delay = 5000;
            //var m = await ReplyAsync($"Purge completed. _This message will be deleted in {delay / 1000} seconds._");
            //await Task.Delay(delay);
            //await m.DeleteAsync();
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