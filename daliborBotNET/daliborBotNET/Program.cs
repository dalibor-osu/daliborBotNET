using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace daliborBotNET
{
    public class Program
    {
        private DiscordSocketClient _client;
        private ulong _guildId = 835534125544112189;
        private IConfiguration _config;
        private Commands _commands;

        public static Task Main(string[] args) => new Program().MainAsync();

        private async Task MainAsync()
        {
            Console.WriteLine("Creating variables...");
            _client = new DiscordSocketClient();
            _commands = new Commands(_client);
            _config = GetConfig();
            
            Console.WriteLine("Assigning client events...");
            _client.Log += Log;
            _client.Ready += Client_Ready;
            _client.SlashCommandExecuted += SlashCommandHandler;

            Console.WriteLine("Logging in...");
            await _client.LoginAsync(TokenType.Bot, _config["Token"]);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        
        private static IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build();
        }
        
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            Command? usedCommand = _commands.GetCommands()?.Find(x => x.commandName == command.Data.Name);
            if (usedCommand == null)
            {
                await command.RespondAsync($"Failed to execute {command.Data.Name}");   
            }
            else
            { 
                usedCommand.Execute(command);
            }
        }
        
        private async Task Client_Ready()
        {
            try
            {
                if (_commands.GetCommands() == null) Console.WriteLine("Commands are null");
                foreach (var command in _commands.GetCommands())
                {
                    if (command._isGlobal)
                    {
                        Console.WriteLine("Adding command: " + command.commandName + " as global");
                        await _client.CreateGlobalApplicationCommandAsync(command.GetBuilder().Build());
                    }
                    else
                    {
                        var newGuildId = (ulong)command.guildId!;
                        Console.WriteLine("Adding command: " + command.commandName + " as private in guild " + newGuildId);
                        var guild = _client.GetGuild(newGuildId);
                        await guild.CreateApplicationCommandAsync(command.GetBuilder().Build());
                    }
                }
            }
            catch(ApplicationCommandException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
