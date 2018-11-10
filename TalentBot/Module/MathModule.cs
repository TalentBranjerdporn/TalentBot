using Discord.Commands;
using TalentBot.Preconditions;

using System.Linq;
using System.Threading.Tasks;
using TalentBot.Common;
using System.Collections.Generic;

namespace TalentBot.Modules
{
    [Name("Math")]
    public class MathModule : ModuleBase<SocketCommandContext>
    {
        [Command("isinteger")]
        [Remarks("Check if the input text is a whole number.")]
        [MinPermissions(AccessLevel.User)]
        public async Task IsInteger(int number)
        {
            await ReplyAsync($"The text {number} is a number!");
        }

        [Command("multiply")]
        [Remarks("Get the product of two numbers.")]
        [MinPermissions(AccessLevel.User)]
        public async Task Multiply(int a, int b)
        {
            int product = a * b;
            await ReplyAsync($"The product of `{a} * {b}` is `{product}`.");
        }

        [Command("addmany")]
        [Remarks("Get the sum of many numbers")]
        [MinPermissions(AccessLevel.User)]
        public async Task AddMany(params int[] numbers)
        {
            int sum = numbers.Sum();
            await ReplyAsync($"The sum of `{string.Join(", ", numbers)}` is `{sum}`.");
        }

        [Command("factorial")]
        [Remarks("Calculate the factorial of a number")]
        [MinPermissions(AccessLevel.User)]
        public async Task Factorial(int number)
        {
            double fact = AdvMath.factorial(number);
            await ReplyAsync($"{number}! = {fact}");
        }

        [Command("hgm")]
        [Remarks("Calculate the hypergeometric probability")]
        [MinPermissions(AccessLevel.User)]
        public async Task Hgm(int deck_size, int hand_size, params int[] nums)
        {

            //double hyper = AdvMath.hgm(deck_size, hand_size, c1, c2, d1, d2);
            if ((nums.Length) % 2 != 0)
            {
                await ReplyAsync($"Input must be even");
            }
            else
            {
                List<int> k = new List<int>();
                List<int> x = new List<int>();

                for (int i = 0; i < nums.Length; i++)
                {
                    if (i < nums.Length / 2)
                    {
                        k.Add(nums[i]);
                    }
                    else
                    {
                        x.Add(nums[i]);
                    }
                }

                double hyper = AdvMath.multihypergeo(x.ToArray(), deck_size, hand_size, k.ToArray());
                await ReplyAsync($"prob = {hyper}");
            }
        }
    }
}