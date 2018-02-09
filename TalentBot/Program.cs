using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private CommandHandler _commands;

        public async Task MainAsync()
        {
            Configuration.EnsureExists();

            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,              // Specify console verbose information level.
                MessageCacheSize = 1000                      // Tell discord.net how long to store messages (per channel).
            });

            _client.Log += (l)                               // Register the console log event.
                => Console.Out.WriteLineAsync(l.ToString());

            _client.MessageReceived += MessageReceived;

            await _client.LoginAsync(TokenType.Bot, Configuration.Load().Token);
            await _client.StartAsync();

            ChangeGame();

            _commands = new CommandHandler();                // Initialize the command handler service
            await _commands.InstallAsync(_client);

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            // Logging
            Console.WriteLine($"{message.Author}: {message.ToString()}");

            string[] hi_message =
            {
                "Nice to meet you",
                "How are you today?",
                "Hope you're having a nice day",
                "You ready for some games?",
                "Hello!",
                "Hiya",
                "Greetings"
            };


            foreach (SocketUser user in message.MentionedUsers)
            {
                if (user.ToString() == "talent-bot#7593")
                {
                    if (message.Content.Contains("Hi") || message.Content.Contains("hi") ||
                        message.Content.Contains("Hello") ||  message.Content.Contains("hello") || 
                        message.Content.Contains("Hiya") || message.Content.Contains("hiya") ||
                        message.Content.Contains("Greetings") || message.Content.Contains("greetings"))
                    {
                        if (message.Author.ToString() == "Pwnstar#6451")
                        {
                            await message.Channel.SendMessageAsync($"Hi {message.Author.Mention} I am awake");
                        }
                        else if (message.Author.ToString() == "!!#7047")
                        {
                            await message.Channel.SendMessageAsync($"Hello! I would appreciate if you didn't try to break me {message.Author.Mention}");
                        }
                        else
                        {
                            Random rand = new Random();
                            await message.Channel.SendMessageAsync($"{hi_message[rand.Next(hi_message.Length)]} {message.Author.Mention}");
                        }

                        break;
                    }
                }
            }

            if (message.Content == "welcome back")
            {
                await message.Channel.SendMessageAsync("yay!");
            }

            if ((message.Content.Contains("good") || message.Content.Contains("Good")) && (message.Content.Contains("bot")))
            {
                string path = @"C:\Users\Talent\Dropbox\Pictures\Cute\NewGame\BestGirl\HifumiDidIt.gif";
                await message.Channel.SendFileAsync(path);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async void ChangeGame()
        {
            // Choose random game
            Random rand = new Random();
            string game = Hidden.game_display[rand.Next(Hidden.game_display.Length)];
            await _client.SetGameAsync(game);
        }

    }
}
