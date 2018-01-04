using Discord;
using Discord.Commands;
using TalentBot.Preconditions;

using System;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using System.Collections.Generic;

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

        [Command("ancestor")]
        [Remarks("Wisdom from the ancestor")]
        [MinPermissions(AccessLevel.User)]
        public async Task Ancestor()
        {
            string Url = "http://darkestdungeon.gamepedia.com/Narrator";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);
            List<string> quotes = new List<string>();

            var tables = doc.DocumentNode.SelectNodes("//table/tr/td");
            foreach (var v in tables)
            {
                if (tables.IndexOf(v) % 3 == 1)
                {
                    quotes.Add(v.InnerText);
                }
            }

            int randPos = rand.Next(quotes.Count);
            string wisdom = quotes[randPos];

            var builder = new EmbedBuilder()
            {
                Color = new Color(1, 1, 1),
                Description = wisdom
            };
            builder.WithFooter(footer =>
            {
                footer.WithText("~The Ancestor");
            });

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("flip")]
        [Remarks("Flip a coin")]
        [MinPermissions(AccessLevel.User)]
        public async Task Flip()
        {
            if (rand.Next(2) == 1)
            {
                await Context.Channel.SendMessageAsync("Flipped Heads");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Flipped Tails");
            }
        }

        [Command("roll")]
        [Remarks("Roll a something with many sides apparently")]
        [MinPermissions(AccessLevel.User)]
        public async Task Roll(params string[] nums)
        {
            if (nums.Length == 0)
            {
                await Context.Channel.SendMessageAsync($"Rolled a {rand.Next(100) + 1}");
            }
            else if (nums.Length == 1)
            {
                await Context.Channel.SendMessageAsync($"Rolled a {rand.Next(int.Parse(nums[0])) + 1}");
            }
            else if (nums.Length == 2)
            {
                int num = int.Parse(nums[1]) - int.Parse(nums[0]);
                await Context.Channel.SendMessageAsync($"Rolled a {rand.Next(num) + 1 + int.Parse(nums[0])}");
            }

        }
    }
}
