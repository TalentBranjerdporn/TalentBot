using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalentBot.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private CommandService _service;

        public HelpModule(CommandService service)           // Create a constructor for the commandservice dependency
        {
            _service = service;
        }

        [Command("help")]
        public async Task HelpAsync()
        {
            string prefix = Configuration.Load().Prefix;
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "These are the commands you can use"
            };

            foreach (var module in _service.Modules)
            {
                if (module.Name == "Blackjack" || 
                    module.Name == "Admin" || 
                    module.Name == "HelpModule" || 
                    module.Name == "Math" || 
                    module.Name == "RoShamBo")
                {
                    continue;
                }
                string description = null;

                HashSet<string> cmds = new HashSet<string>();

                foreach (var cmd in module.Commands)
                {
                    if (cmd.Name == "hentai")
                    {
                        continue;
                    }

                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                    {
                        if (cmds.Add(cmd.Aliases.First()))
                        {
                            description += $"{prefix}{cmd.Aliases.First()}\n";
                        }
                        
                    }
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("help")]
        public async Task HelpAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            string prefix = Configuration.Load().Prefix;
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    string param;
                    if (cmd.Parameters.Count == 0)
                    {
                        param = "None";
                    } else
                    {
                        param = string.Join(", ", cmd.Parameters.Select(p => p.Name));
                    }
                    x.Value = $"Parameters: {param}\n" +
                              $"Remarks: {cmd.Remarks}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
