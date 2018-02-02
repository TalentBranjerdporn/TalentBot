using Discord;
using Discord.Commands;
using TalentBot.Preconditions;

using System;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

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

            string random_bugcat = bugcats[rand.Next(bugcats.Length)];
            await Context.Channel.SendFileAsync(random_bugcat);
        }

        [Command("ancestor")]
        [Remarks("Wisdom from the ancestor")]
        [MinPermissions(AccessLevel.User)]
        public async Task Ancestor()
        {
            string Url = "https://darkestdungeon.gamepedia.com/Narrator";
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

            int rand_pos = rand.Next(quotes.Count);
            string wisdom = quotes[rand_pos];

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

        [Command("bestgirl")]
        [Remarks("Best girl")]
        [MinPermissions(AccessLevel.User)]
        public async Task BestGirl()
        {
            string path = @"C:\Users\Talent\Dropbox\Pictures\Cute\NewGame\BestGirl";
            string[] bestgirl = Directory.GetFiles(path);

            string random_hifumi = bestgirl[rand.Next(bestgirl.Length)];
            await Context.Channel.SendFileAsync(random_hifumi);
        }

        [Command("notlikethis")]
        [Remarks("NotLikeThis")]
        [MinPermissions(AccessLevel.User)]
        public async Task NotLikeThis()
        {
            string path = @"C:\Users\Talent\Dropbox\Pictures\Reactions\NotLikeThis.PNG";
            await Context.Channel.SendFileAsync(path);
        }
    }
}
