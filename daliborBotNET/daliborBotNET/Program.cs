using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace daliborBotNET
{
    public class Program
    {
        public DiscordSocketClient client { get; private set; }
        private ulong guildId = 835534125544112189;
        private IConfiguration _config;
        private Commands _commands;

        public static Task Main(string[] args) => new Program().MainAsync();

        private async Task MainAsync()
        {
            Console.WriteLine("Creating variables...");
            client = new DiscordSocketClient();
            _commands = new Commands(client);
            _config = GetConfig();
            
            Console.WriteLine("Assigning client events...");
            client.Log += Log;
            client.Ready += Client_Ready;
            client.SlashCommandExecuted += SlashCommandHandler;

            Console.WriteLine("Logging in...");
            await client.LoginAsync(TokenType.Bot, _config["Token"]);
            await client.StartAsync();

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
        
        public async Task Client_Ready()
        {
            try
            {
                if (_commands.GetCommands() == null) Console.WriteLine("Commands are null");
                foreach (var command in _commands.GetCommands())
                {
                    if (command._isGlobal)
                    {
                        Console.WriteLine("Adding command: " + command.commandName + " as global");
                        await client.CreateGlobalApplicationCommandAsync(command.GetBuilder().Build());
                    }
                    else
                    {
                        var newGuildId = (ulong)command.guildId!;
                        Console.WriteLine("Adding command: " + command.commandName + " as private in guild " + newGuildId);
                        var guild = client.GetGuild(newGuildId);
                        await guild.CreateApplicationCommandAsync(command.GetBuilder().Build());
                    }
                }
            }
            catch(ApplicationCommandException exception)
            {
                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(exception.Message);
            }
        }
    }
}
