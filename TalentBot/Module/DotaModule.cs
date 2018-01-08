using Discord;
using Discord.Commands;
using TalentBot.Preconditions;
using TalentBot.Common.API;

using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace TalentBot.Module
{
    [Name("Dota")]
    public class DotaModule : ModuleBase<SocketCommandContext>
    {

        enum Medal
        {
            Herald = 2,
            Crusader = 3,
            Archon = 4,
            Legend = 5,
            Ancient = 6,
            Divine = 7
        };

        // Random instance
        private Random rand = new Random();

        private Dictionary<string, string> known_players = new Dictionary<string, string>();

        [Command("flip")]
        [Remarks("Flip a coin")]
        [MinPermissions(AccessLevel.User)]
        public async Task Flip()
        {
            if (rand.Next(2) == 1)
            {
                await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} flipped a coin: ***HEADS***");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} flipped a coin: ***TAILS***");
            }
        }

        [Command("roll")]
        [Remarks("Roll a something with many sides apparently")]
        [MinPermissions(AccessLevel.User)]
        public async Task Roll(params string[] nums)
        {
            if (nums.Length == 0)
            {
                await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} rolls {rand.Next(100) + 1}");
            }
            else if (nums.Length == 1)
            {
                await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} rolls {rand.Next(int.Parse(nums[0])) + 1}");
            }
            else if (nums.Length == 2)
            {
                int num = int.Parse(nums[1]) - int.Parse(nums[0]);
                await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} rolls {rand.Next(num) + 1 + int.Parse(nums[0])}");
            }
        }

        [Command("randomhero")]
        [Remarks("The classic random for all the elitists who equate randoming to higher level play and satisfaction")]
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
            await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} has randomed {rand_hero}");

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

        [Command("medal")]
        [Remarks("Medal check. Note: Only works for games with Pwnstar unfortunately")]
        [MinPermissions(AccessLevel.User)]
        public async Task MedalCheck(params string[] cmds)
        {
            // Get player IDs from server log
            List<String> players = OpenDotaAPI.GetPlayerIDs();
            var result = String.Join(", ", players.ToArray());

            Console.WriteLine($"{result}");

            List<int> ranks = new List<int>();
            StringBuilder sb1 = new StringBuilder("| ");
            StringBuilder sb2 = new StringBuilder("| ");

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
            };

            for (int i = 0; i < 10; i++)
            {
                PlayerData data = await OpenDotaAPI.GetPlayerDataAsync(players[i]);
                int rank = 0;
                if (data.rank_tier != null)
                {
                    rank = Convert.ToInt32(data.rank_tier);
                }
                ranks.Add(rank);
                if (i < 5)
                {
                    if (rank == 0)
                    {
                        sb1.Append("Uncalibrated | ");
                    }
                    else
                    {
                        sb1.Append($"{(Medal)(rank / 10)} {rank % 10} | ");
                    }
                }
                else
                {
                    if (rank == 0)
                    {
                        sb2.Append("Uncalibrated | ");
                    }
                    else
                    {
                        sb2.Append($"{(Medal)(rank / 10)} {rank % 10} | ");
                    }
                }
            }

            builder.AddField(x =>
            {
                x.Name = "Radiant";
                x.Value = sb1;
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "Dire";
                x.Value = sb2;
                x.IsInline = true;
            });

            //var stringsArray = ranks.Select(i => i.ToString()).ToArray();
            //var output = string.Join(",", stringsArray);

            await ReplyAsync("", false, builder.Build());


        }

        [Command("lastmatch")]
        [Remarks("Get the last match on OpenDota")]
        [MinPermissions(AccessLevel.BotOwner)]
        public async Task LastMatch()
        {
            if (known_players.Count() == 0)
            {
                known_players.Add("Pwnstar#6451", "110093717");
            }

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
            };

            string id;
            if (known_players.TryGetValue(Context.Message.Author.ToString(), out id))
            {
                MatchData[] data = await OpenDotaAPI.GetPlayerMatches(id, 1, 0, 0);

                await Context.Channel.SendMessageAsync($@"https://www.opendota.com/matches/{data[0].match_id}");
                //builder.AddField(x =>
                //{
                //    x.Name = "Result";
                //    x.Value = $@"https://www.opendota.com/matches/{data[0].match_id}";
                //});
            }

            //await ReplyAsync("", false, builder.Build());
        }
    }
}
