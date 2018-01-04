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

            string randomBugcat = bugcats[rand.Next(bugcats.Length)];
            await Context.Channel.SendFileAsync(randomBugcat);
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

        [Command("randomhero")]
        [Remarks("The classic random for all the elitists who equate randoming to higher level play")]
        [MinPermissions(AccessLevel.User)]
        public async Task RandomHero()
        {
            // Hmm I feel like there should possibly be a better way but I'm too lazy to think too hard. Pls no hate
            // Also future adding hero pictures?
            string[] heros =
            {
                "Abaddon",
                "Alchemist",
                "Ancient Apparition",
                "Anti-Mage",
                "Arc Warden",
                "Axe",
                "Bane",
                "Batrider",
                "Beastmaster",
                "Bloodseeker",
                "Bounty Hunter",
                "Brewmaster",
                "Bristleback",
                "Broodmother",
                "Centaur Warrunner",
                "Chaos Knight",
                "Chen",
                "Clinkz",
                "Clockwork",
                "Crystal Maiden",
                "Dark Seer",
                "Dark Willow",
                "Dazzle",
                "Death Prophet",
                "Disruptor",
                "Doom",
                "Dragon Knight",
                "Drow Ranger",
                "Earth Spirit",
                "Earthshaker",
                "Elder Titan",
                "Ember Spirit",
                "Enchantress",
                "Enigma",
                "Faceless Void",
                "Gyrocopter",
                "Huskar",
                "Invoker",
                "Io",
                "Jakiro",
                "Juggernaut",
                "Keeper of the Light",
                "Kunkka",
                "Legion Commander",
                "Leshrac",
                "Lich",
                "Lifestealer",
                "Lina",
                "Lion",
                "Lone Druid",
                "Luna",
                "Lycan",
                "Magnus",
                "Medusa",
                "Meepo",
                "Mirana",
                "Monkey King",
                "Morphling",
                "Naga Siren",
                "Nature's Prophet",
                "Necrophos",
                "Night Stalker",
                "Nyx Assassin",
                "Ogre Magi",
                "Omniknight",
                "Oracle",
                "Outworld Devourer",
                "Pangolier",
                "Phantom Assassin",
                "Phantom Lancer",
                "Phoenix",
                "Puck",
                "Pudge",
                "Pugna",
                "Queen of Pain",
                "Razor",
                "Riki",
                "Rubick",
                "Sand King",
                "Shadow Demon",
                "Shadow Fiend",
                "Shadow Shaman",
                "Silencer",
                "Skywrath Mage",
                "Slardar",
                "Slark",
                "Sniper",
                "Spectre",
                "Spirit Breaker",
                "Storm Spirit",
                "Sven",
                "Techies",
                "Templar Assassin",
                "Terrorblade",
                "Tidehunter",
                "Timbersaw",
                "Tinker",
                "Tiny",
                "Treat Protector",
                "Troll Warlord",
                "Tusk",
                "Underlord",
                "Undying",
                "Ursa",
                "Vengeful Spirit",
                "Venomancer",
                "Viper",
                "Visage",
                "Warlock",
                "Weaver",
                "Windranger",
                "Winter Wyvern",
                "Witch Doctor",
                "Wraith King",
                "Zeus"
            };

            string rand_hero = heros[rand.Next(heros.Length)];
            await Context.Channel.SendMessageAsync($"You have randomed {rand_hero}");

            //string Url = "https://dota2.gamepedia.com/Table_of_hero_attributes";
            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load(Url);
            //List<string> heroes = new List<string>();

            //var tables = doc.DocumentNode.SelectNodes("//div[@id='mw-content-text']/table[2]/tbody/tr[1]/td[1]/span/a[2]");

            //foreach (var v in herotable)
            //{
            //    await Context.Channel.SendMessageAsync($"{v.InnerText}");
            //    // heroes.Add(v.InnerText);
            //}//*[@id="mw-content-text"]/table[2]/tbody/tr[1]/td[1]/span/a[2]

        }
    }
}
