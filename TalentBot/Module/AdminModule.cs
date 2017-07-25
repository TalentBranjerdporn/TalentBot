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
    }
}