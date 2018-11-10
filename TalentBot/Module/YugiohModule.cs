using Discord;
using Discord.Commands;
using TalentBot.Preconditions;
using TalentBot.Common.API;
using TalentBot.Common;

using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TalentBot.Modules
{
    [Name("Yugioh")]
    public class YugiohModule : ModuleBase<SocketCommandContext>
    {
        private struct cardType
        {
            public string name;
            public int amt, min, max;

            public cardType(string n, int a, int b, int c)
            {
                name = n;
                amt = a;
                min = b;
                max = c;
            }
        }

        private struct cardPool
        {
            public string name;
            public int amt, min, max;

            public cardPool(string n, int a, int b, int c)
            {
                name = n;
                amt = a;
                min = b;
                max = c;
            }
        }

        private class HyperGeometric
        {
            private int population;
            private int sample_size;
            private int[] max;

            public HyperGeometric(int population, int sample_size)
            {
                this.population = population;
                this.sample_size = sample_size;
            }

            public double Cumulative(int[] max)
            {
                this.max = max;
                int[] counters = new int[max.Length];

                double test = array_traverse(counters, max, 0);
                return test;
            }

            private void Populate<T>(T[] arr, T value, int index)
            {
                for (int i = index; i < arr.Length; i++)
                {
                    arr[i] = value;
                }
            }

            private double array_traverse(int[] counters, int[] length, int level)
            {
                double output = 0;
                if (level == counters.Length)
                {
                    return perform_calc(counters);
                }
                else
                {
                    for (counters[level] = 1; counters[level] <= length[level] & counters.Sum() <= sample_size; counters[level]++)
                    {
                        output += array_traverse(counters, length, level + 1);
                    }
                }
                counters[level] = 1;
                return output;

            }

            private double perform_calc(int[] counters)
            {
                return AdvMath.multihypergeo(counters,population,sample_size,max);
            }
        }

        // Capitalising strings
        private string capitalise(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        [Command("ygotext")]
        [Remarks("Search for a card text.\nThanks to http://yugiohprices.com for the API")]
        [MinPermissions(AccessLevel.User)]
        public async Task YGOText(params string[] search)
        {
            string cardName = string.Join(" ", search);

            CardData request = await YGOAPI.GetCardData(cardName);

            if (request.status == "success")
            {
                Card data = request.data;
                var builder = new EmbedBuilder()
                {
                    Color = new Color(114, 137, 218),
                };

                var urlName = string.Join("_", search);
                builder.WithAuthor(x =>
                {
                    x.Name = data.name;
                    x.Url = $"https://yugipedia.com/wiki/{urlName}";
                });

                StringBuilder des = new StringBuilder();

                if (data.card_type.Equals("monster"))
                {
                    des.AppendLine($"Level: {data.level}, " +
                        $"Category: {capitalise(data.card_type)}, " +
                        $"Type: {data.type}, " +
                        $"Attribute: {capitalise(data.family)}");
                    des.AppendLine();
                    des.AppendLine(data.text);
                    des.AppendLine();
                    des.AppendLine($"ATK: {data.atk}, " +
                        $"DEF: {data.def}");
                }
                else if (data.card_type.Equals("spell"))
                {
                    des.AppendLine($"Category: {capitalise(data.card_type)}, " +
                        $"Property: {data.property}");
                    des.AppendLine();
                    des.AppendLine(data.text);
                }
                else if (data.card_type.Equals("trap"))
                {
                    des.AppendLine($"Category: {capitalise(data.card_type)}, " +
                        $"Property: {data.property}");
                    des.AppendLine();
                    des.AppendLine(data.text);
                }

                builder.Description = des.ToString();

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Card does not exist");
            }
        }

        [Command("ygosupport")]
        [Remarks("Search card support.\nThanks to http://yugiohprices.com for the API\nThis kinda sucks just warning")]
        [MinPermissions(AccessLevel.BotOwner)]
        public async Task YGOSupport(params string[] search)
        {
            string cardName = string.Join(" ", search);

            string[] request = await YGOAPI.GetCardSupportData(cardName);

            if (request != null)
            {
                var builder = new EmbedBuilder()
                {
                    Color = new Color(114, 137, 218),
                };
                builder.WithAuthor(x =>
                {
                    x.Name = cardName + " Support";
                });

                StringBuilder des = new StringBuilder();

                foreach (string support in request)
                {
                    des.AppendLine(support);
                }

                builder.Description = des.ToString();

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync("Card does not exist");
            }
        }

        [Command("ygoprob")]
        [Remarks("Work out the probability of starting a particular hand\nUsage: ygoprob <deck_size> <hand_size> <cards{name, amount}> [<cards{name, amount}>]")]
        [MinPermissions(AccessLevel.User)]
        public async Task YGOProb(int deck_size, int hand_size, params string[] cards)
        {
            List<cardPool> population = new List<cardPool>();

            for (int i = 0; i < cards.Length; i += 2)
            {
                population.Add(new cardPool(cards[i], int.Parse(cards[i + 1]), 1, int.Parse(cards[i + 1])));
            }

            StringBuilder names = new StringBuilder();
            StringBuilder amounts = new StringBuilder();

            List<int> x = new List<int>();
            List<int> k = new List<int>();
            foreach (cardPool pop in population)
            {
                x.Add(pop.min);
                k.Add(pop.max);
                names.AppendLine(pop.name);
                amounts.AppendLine(pop.max.ToString());
            }

            HyperGeometric hg = new HyperGeometric(deck_size, hand_size);
            double prob = hg.Cumulative(k.ToArray());

            var builder = new EmbedBuilder().WithColor(new Color(114, 137, 218));

            builder.AddField(i =>
            {
                i.Name = "Card Name";
                i.Value = names;
                i.IsInline = true;
            });

            builder.AddField(i =>
            {
                i.Name = "Amount";
                i.Value = amounts;
                i.IsInline = true;
            });

            builder.AddField(i =>
            {
                i.Name = "Your probability of starting this hand is";
                i.Value = prob.ToString("P");
                i.IsInline = true;
            });

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}