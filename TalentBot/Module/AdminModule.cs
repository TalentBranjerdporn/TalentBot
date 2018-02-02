using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TalentBot.Preconditions;

using System.Threading.Tasks;
using System.IO;
using System;
using TalentBot.Common.API;
using System.Collections.Generic;
using System.Text;

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

        [Command("purge")]
        [Summary("Deletes the specified amount of messages.")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task PurgeChat(int amount)
        {

            await Context.Channel.DeleteMessagesAsync((await Context.Channel.GetMessagesAsync(amount+1).Flatten()));
        }

        [Group("set"), Name("Admin")]
        public class Set : ModuleBase
        {
            [Command("nick")]
            [Remarks("Change the nickname")]
            [MinPermissions(AccessLevel.User)]
            public async Task Nick([Remainder]string name)
            {
                var user = Context.User as SocketGuildUser;
                await user.ModifyAsync(x => x.Nickname = name);

                await ReplyAsync($"{user.Mention} I changed your name to **{name}**");
            }

            [Command("botnick")]
            [Remarks("Change my nickname")]
            [MinPermissions(AccessLevel.ServerOwner)]
            public async Task BotNick([Remainder]string name)
            {
                var self = await Context.Guild.GetCurrentUserAsync();
                await self.ModifyAsync(x => x.Nickname = name);

                await ReplyAsync($"I changed my name to **{name}**");
            }
        }

        [Command("test")]
        [Remarks("Just a test command")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Test()
        {
            List<String> players = OpenDotaAPI.GetPlayerIDs();
            PlayerData data = await OpenDotaAPI.GetPlayerData(players[0]);

            if (data.profile != null)
            {
                //await Context.Channel.SendMessageAsync($"{data.solo_competitive_rank}");
                if (data.solo_competitive_rank == null)
                {
                    await ReplyAsync($"empty");
                } else
                {
                    await ReplyAsync($"this");
                }
                
            } else
            {
                //await Context.Channel.SendMessageAsync("This is null");
                await ReplyAsync($"This is null");
            }
            
        }

        [Command("todo")]
        [Remarks("Future stuff")]
        [MinPermissions(AccessLevel.User)]
        public async Task Todo(params string[] input)
        {
            string path = @"F:\MyStuff\MyDocuments\scripts\Files\Text\Talbot\Todo.txt";
            string text = String.Join(" ", input);

            using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteLineAsync(text);
            }

            await ReplyAsync($"\'{text}\' has been added to the TODO list");
        }
    }
}