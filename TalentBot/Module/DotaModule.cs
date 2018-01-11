using Discord;
using Discord.Commands;
using TalentBot.Preconditions;
using TalentBot.Common.API;

using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace TalentBot.Module
{
    [Name("Dota")]
    public class DotaModule : ModuleBase<SocketCommandContext>
    {

        enum Medal
        {
            Herald = 1,
            Guardian = 2,
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
        [Remarks("Roll a dice with 100 sides apparently")]
        [MinPermissions(AccessLevel.User)]
        public async Task Roll()
        {
            await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} rolls {rand.Next(100) + 1}");
        }

        [Command("roll")]
        [Remarks("Roll a dice with 'num' sides\nUsage: roll <num>")]
        [MinPermissions(AccessLevel.User)]
        public async Task Roll(int num)
        {
            await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} rolls {rand.Next(num) + 1}");
        }

        [Command("roll")]
        [Remarks("Roll between two numbers\nUsage: roll <num1> <num2>")]
        [MinPermissions(AccessLevel.User)]
        public async Task Roll(params string[] nums)
        {
            try
            {
                if (nums.Length == 2)
                {
                    int num = int.Parse(nums[1]) - int.Parse(nums[0]);
                    await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} rolls {rand.Next(num) + 1 + int.Parse(nums[0])}");
                }
                else
                {
                    throw new Exception("Input string was not in a correct format.");
                }
            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync($"{e.Message} \nUsage: roll <num1> <num2>");
            }
        }

        [Command("randomhero")]
        [Remarks("The classic random for all the elitists who equate randoming to higher level play and greater satisfaction")]
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

        [Command("medals")]
        [Remarks("Medal check. Note: Only works for games with Pwnstar unfortunately. Also has unintended effects with coaches")]
        [MinPermissions(AccessLevel.User)]
        public async Task MedalCheck(params string[] cmds)
        {
            // Get player IDs from server log
            List<String> players = OpenDotaAPI.GetPlayerIDs();
            var result = String.Join(", ", players.ToArray());

            //Console.WriteLine($"{result}");

            List<int> ranks = new List<int>();
            StringBuilder radi_rank = new StringBuilder();
            StringBuilder radi_name = new StringBuilder();
            StringBuilder radi_numb = new StringBuilder();
            StringBuilder dire_rank = new StringBuilder();
            StringBuilder dire_name = new StringBuilder();
            StringBuilder dire_numb = new StringBuilder();

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
            };

            for (int i = 0; i < 10; i++)
            {
                PlayerData data = await OpenDotaAPI.GetPlayerData(players[i]);
                int rank = 0;
                if (data.rank_tier != null)
                {
                    rank = Convert.ToInt32(data.rank_tier);
                }
                ranks.Add(rank);

                if (i < 5)      // Radiant team
                {

                    if (data.profile != null)
                    {
                        radi_name.AppendLine($"{data.profile.personaname}");
                        int solo = data.solo_competitive_rank == null ? 0 : int.Parse(data.solo_competitive_rank);
                        int party = data.competitive_rank == null ? 0 : int.Parse(data.competitive_rank);
                        int? est = data.mmr_estimate.estimate;

                        if (solo == 0 && party == 0)
                        {
                            radi_numb.AppendLine($"Est: {est}");
                        }
                        else if (solo > party)
                        {
                            radi_numb.AppendLine($"Solo: {solo}");
                        }
                        else if (solo < party)
                        {
                            radi_numb.AppendLine($"Party: {party}");
                        }
                        else
                        {
                            radi_numb.AppendLine($"Est: {est}");
                        }
                    }
                    else
                    {
                        radi_name.AppendLine("Anonymous");
                        radi_numb.AppendLine("N/A");
                    }

                    if (rank == 0)
                    {
                        radi_rank.AppendLine("Uncalibrated");
                    }
                    else
                    {
                        radi_rank.AppendLine($"{(Medal)(rank / 10)} {rank % 10}");
                    }
                }
                else            // Dire team
                {
                    if (data.profile != null)
                    {
                        dire_name.AppendLine($"{data.profile.personaname}");
                        int solo = data.solo_competitive_rank == null ? 0 : int.Parse(data.solo_competitive_rank);
                        int party = data.competitive_rank == null ? 0 : int.Parse(data.competitive_rank);
                        int? est = data.mmr_estimate.estimate;

                        if (solo == 0 && party == 0)
                        {
                            dire_numb.AppendLine($"Est: {est}");
                        }
                        else if (solo > party)
                        {
                            dire_numb.AppendLine($"Solo: {solo}");
                        }
                        else if (solo < party)
                        {
                            dire_numb.AppendLine($"Party: {party}");
                        }
                        else
                        {
                            dire_numb.AppendLine($"Est: {est}");
                        }
                    }
                    else
                    {
                        dire_name.AppendLine("Anonymous");
                        dire_numb.AppendLine("N/A");
                    }

                    if (rank == 0)
                    {
                        dire_rank.AppendLine("Uncalibrated");
                    }
                    else
                    {
                        dire_rank.AppendLine($"{(Medal)(rank / 10)} {rank % 10}");
                    }
                }
            }

            Console.WriteLine(radi_name);
            Console.WriteLine(dire_name);

            builder.AddField(x =>
            {
                x.Name = "Radiant";
                x.Value = radi_name;
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "rank";
                x.Value = radi_rank;
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "mmr";
                x.Value = radi_numb;
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "Dire";
                x.Value = dire_name;
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "rank";
                x.Value = dire_rank;
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "mmr";
                x.Value = dire_numb;
                x.IsInline = true;
            });

            //var stringsArray = ranks.Select(i => i.ToString()).ToArray();
            //var output = string.Join(",", stringsArray);

            await ReplyAsync("", false, builder.Build());


        }

        [Command("lastmatch")]
        [Remarks("Get the last match on OpenDota")]
        [MinPermissions(AccessLevel.User)]
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

        [Command("herostats")]
        [Remarks("Get statistics for a specific hero")]
        [MinPermissions(AccessLevel.BotOwner)]
        public async Task HeroStats(string hero)
        {
            HeroData[] data = await OpenDotaAPI.GetHeroStatData();
            HeroData stats = null;

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
            };

            builder.AddField(x =>
            {
                x.Name = "Statistics";
                StringBuilder labels = new StringBuilder("Pro\n");
                for (int i = 1; i < 8; i++)
                {
                    labels.AppendLine($"{(Medal) i}");
                }

                labels.AppendLine("Total");

                x.Value = labels;
                x.IsInline = true;
            });

            int pro_picks = 0;
            int one_picks = 0;
            int two_picks = 0;
            int three_picks = 0;
            int four_picks = 0;
            int five_picks = 0;
            int six_picks = 0;
            int seven_picks = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (hero == data[i].localized_name)         // Check for hero
                {
                    stats = data[i];
                }

                pro_picks += data[i].pro_pick + data[i].pro_ban;
                one_picks += data[i].one_pick;
                two_picks += data[i].two_pick;
                three_picks += data[i].three_pick;
                four_picks += data[i].four_pick;
                five_picks += data[i].five_pick;
                six_picks += data[i].six_pick;
                seven_picks += data[i].seven_pick;
            }

            Console.WriteLine(pro_picks);

            if (stats != null)
            {
                builder.Title = stats.localized_name;

                builder.AddField(x =>
                {
                    x.Name = "Winrate";

                    StringBuilder winrate = new StringBuilder();
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.pro_win / stats.pro_pick * 100));
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.one_win / stats.one_pick * 100));
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.two_win / stats.two_pick * 100));
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.three_win / stats.three_pick * 100));
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.four_win / stats.four_pick * 100));
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.five_win / stats.five_pick * 100));
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.six_win / stats.six_pick * 100));
                    winrate.AppendLine(string.Format("{0:0.00}%", (double)stats.seven_win / stats.seven_pick * 100));

                    int total_win = stats.one_win + stats.two_win + stats.three_win + stats.four_win + stats.five_win + stats.six_win + stats.seven_win;
                    int total_pick = stats.one_pick + stats.two_pick + stats.three_pick + stats.four_pick + stats.five_pick + stats.six_pick + stats.seven_pick;
                    winrate.AppendLine(string.Format("%{0:0.00}", (double)total_win / total_pick * 100));

                    x.Value = winrate;
                    x.IsInline = true;
                });

                builder.AddField(x =>
                {
                    x.Name = "Pickrate";

                    StringBuilder pickrate = new StringBuilder();
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)(stats.pro_pick + stats.pro_ban) / pro_picks * 2200));
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)stats.one_pick / one_picks * 1000));
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)stats.two_pick / two_picks * 1000));
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)stats.three_pick / three_picks * 1000));
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)stats.four_pick / four_picks * 1000));
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)stats.five_pick / five_picks * 1000));
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)stats.six_pick / six_picks * 1000));
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)stats.seven_pick / seven_picks * 1000));

                    int total_pick = stats.one_pick + stats.two_pick + stats.three_pick + stats.four_pick + stats.five_pick + stats.six_pick + stats.seven_pick;
                    int total_picks = one_picks + two_picks + three_picks + four_picks + five_picks + six_picks + seven_picks;
                    pickrate.AppendLine(string.Format("{0:0.00}%", (double)total_pick / total_picks * 1000));

                    x.Value = pickrate;
                    x.IsInline = true;
                });

                await ReplyAsync("", false, builder.Build());
            } else
            {
                await ReplyAsync("Hero not found");
            }
        }
    }
}
