using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TalentBot.Preconditions;

using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TalentBot.Module
{
    [Name("Blackjack")]
    public class BlackjackModule : ModuleBase<SocketCommandContext>
    {
        const bool MIN = true;
        const bool MAX = false;
        Random rand = new Random();
        private static Card[] cards;
        static Card[] shuffledDeck = new Card[52];
        static Card[] botCards = new Card[10];
        static Card[] playerCards = new Card[10];
        static bool playing = false;
        static int playerNumCards = 0;
        static int botNumCards = 0;
        static int index = 0;

        [Command("bj")]
        [Remarks("Play a game of blackjack")]
        [MinPermissions(AccessLevel.ServerAdmin)]
        public async Task Blackjack(params string[] cmds)
        {
            if (cmds[0].Equals("play"))
            {
                if (playing)
                {
                    await ReplyAsync("A game is already in progress");
                }
                else
                {
                    playing = true;
                    await ReplyAsync("Lets play a game of Blackjack");

                    if (cards == null || cards.Length == 0)
                    {
                        // build new deck
                        var query =
                            from suit in new[] { "Spades", "Hearts", "Clubs", "Diamonds", }
                            from rank in Enumerable.Range(1, 13)
                            select new Card(suit, rank);

                        cards = query.ToArray();
                    }

                    shuffledDeck = cards.OrderBy(x => rand.Next()).ToArray();
                    index = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        botCards[i] = shuffledDeck[index++];
                        botNumCards++;
                        playerCards[i] = shuffledDeck[index++];
                        playerNumCards++;
                    }

                    var builder = buildGame(botCards, playerCards, false);

                    await ReplyAsync("", false, builder.Build());
                    await ReplyAsync("Hit or Stand?");
                }

            }
            else if (cmds[0].Equals("Hit") || cmds[0].Equals("hit")) {

                if (playing)
                {
                    playerCards[playerNumCards++] = shuffledDeck[index++];

                    if (checkVal(playerCards, MIN) > 21)
                    {
                        var builder = buildGame(botCards, playerCards, true);

                        await ReplyAsync("", false, builder.Build());

                        resetGame();
                        await ReplyAsync("You bust, I win once again");
                    }
                    else
                    {
                        var builder = buildGame(botCards, playerCards, false);

                        await ReplyAsync("", false, builder.Build());
                        await ReplyAsync("Hit or Stand?");
                    }
                }
                else
                {
                    await ReplyAsync("A game is not currently being played. Use 'play' to start a game");
                }
            }
            else if (cmds[0].Equals("Stand") || cmds[0].Equals("stand"))
            {
                if (playing)
                {
                    if (checkVal(botCards,MIN) == 2)
                    {
                        while (checkVal(botCards, MIN) < 17)
                        {
                            botCards[botNumCards++] = shuffledDeck[index++];
                        }
                    }
                    else
                    {
                        while (checkVal(botCards, MAX) < 17)
                        {
                            botCards[botNumCards++] = shuffledDeck[index++];
                        }
                    }
                    var builder = buildGame(botCards, playerCards, true);
                    await ReplyAsync("", false, builder.Build());

                    int botResult = maxViable(checkVal(botCards, MIN), checkVal(botCards, MAX));
                    int playerResult = maxViable(checkVal(playerCards, MIN), checkVal(playerCards, MAX));

                    if (botResult > 21)
                    {
                        if (playerResult > 21)
                        {
                            // draw
                            await ReplyAsync($"result {botResult} vs {playerResult}\nWelp we both bust. Draw");
                        }
                        else
                        {
                            // player win
                            await ReplyAsync($"result {botResult} vs {playerResult}\nI bust, you win");
                        }
                    }
                    else
                    {
                        if (playerResult > 21)
                        {
                            // bot win
                            await ReplyAsync($"result {botResult} vs {playerResult}\nBad luck you bust");
                        }
                        else
                        {
                            if (playerResult < botResult)
                            {
                                // bot win
                                await ReplyAsync($"result {botResult} vs {playerResult}\nI win once again!");
                            }
                            else if (playerResult > botResult)
                            {
                                // player win
                                await ReplyAsync($"result {botResult} vs {playerResult}\nCongratulations you win");
                            }
                            else
                            {
                                // draw
                                await ReplyAsync($"result {botResult} vs {playerResult}\nIt's a draw!");
                            }
                        }
                    }

                    resetGame();
                }
                else
                {
                    await ReplyAsync("A game is not currently being played. Use 'play' to start a game");
                }
            }
        }

        private void resetGame()
        {
            shuffledDeck = new Card[52];
            botCards = new Card[10];
            playerCards = new Card[10];
            playing = false;
            playerNumCards = 0;
            botNumCards = 0;
            index = 0;
        }

        private string printCards(Card[] cards)
        {
            StringBuilder stringCards = new StringBuilder();
            foreach (Card c in cards)
            {
                if (c != null)
                {
                    stringCards.Append(c);
                }
            }

            return stringCards.ToString();
        }

        private int checkVal(Card[] cards, bool type)
        {
            int result = 0;
            foreach (Card c in cards)
            {
                if (c != null)
                {
                    if (type)   // min = true
                    {
                        result += c.getVal();
                    }
                    else        // max = false
                    {
                        if (c.getVal() == 1)
                        {
                            result += 11;
                        }
                        else
                        {
                            result += c.getVal();
                        }
                    }
                }
            }
            return result;
        }

        private int maxViable(int min, int max)
        {
            if (min < 21)
            {
                if (max < 21)
                {
                    return max;
                }
            }
            return min;
        }

        private EmbedBuilder buildGame(Card[] botCards, Card[] playerCards, bool show)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
            };

            builder.AddField(x =>
            {
                x.Name = "My cards";
                if (show)
                {
                    x.Value = printCards(botCards);
                }
                else
                {
                    x.Value = $" { botCards[0] }[ X ] ";
                }
                x.IsInline = true;
            });

            builder.AddField(x =>
            {
                x.Name = "Your cards";
                x.Value = printCards(playerCards);
                x.IsInline = true;
            });

            return builder;
        }
    }

    internal class Card
    {
        private static string[] suits =
        {
            "Hearts",
            "Clubs",
            "Diamonds",
            "Spades"
        };

        private static string[] vals =
        {
            "A",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "J",
            "Q",
            "K"
        };

        private string suit;
        private int val;

        public Card()
        {

        }

        public Card(string suit, int val)
        {
            this.suit = suit;
            this.val = val;
        }

        public int getVal()
        {
            int result = val;
            if (result > 10)
            {
                result = 10;
            }

            return result;
        }

        public override string ToString()
        {
            return $"[ { vals[val-1] }{ suit[0] } ]";
        }
    }
}
