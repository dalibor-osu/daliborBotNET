using System.Net;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace daliborBotNET
{
    public class Program
    {
        private DiscordSocketClient? _client;
        private IConfiguration? _config;
        private Commands? _commands;

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
            _client.MessageReceived += MessageHandler;

            Console.WriteLine("Logging in...");
            await _client.LoginAsync(TokenType.Bot, _config["Token"]);
            await _client.StartAsync();
            await ReadUserInput();
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task MessageHandler(SocketMessage msg)
        {
            if (IsPrivateMessage(msg) && msg.Author.Id != _client.CurrentUser.Id) Console.WriteLine($"{msg.Author.Username} ({msg.Author.Id}): {msg.Content}");
            return Task.CompletedTask;
        }

        private bool IsPrivateMessage(SocketMessage msg)
        {
            return msg.Channel.GetType() == typeof(SocketDMChannel);
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
            Console.WriteLine($"{command.User.Username} ({command.User.Id}) used {command.Data.Name}");;
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
                List<Command> commands = _commands.GetCommands();
                if (commands.Count == 0)
                {
                    Console.WriteLine("There are no commands!");
                    return;
                }
                
                foreach (var command in commands)
                {
                    if (command.isGlobal)
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
            catch(HttpException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private Task ReadUserInput()
        {
            StreamReader reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
            Task<string?> input = reader.ReadLineAsync();
            HandleCommand(input);
            return Task.CompletedTask;
        }

        private void HandleCommand(Task<string?> command)
        {
            switch (command.Result)
            {
                case "stop":
                    Environment.Exit(0);
                    break;
                
                case string a when a.Contains("msg"):
                    string[] args = a.Split(' ');
                    HandleMessageUser(args, GetMessageContent(command.Result));
                    break;
            }

            ReadUserInput();
        }

        private async void HandleMessageUser(string[] args, string msgContent)
        {
            ulong id;

            if (!ulong.TryParse(args[1], out id))
            {
                Console.WriteLine("Wrong UserID format");
                return;
            }
            
            IUser user = _client.GetUser(id);
            
            if (user == null)
            {
                Console.WriteLine($"Couldn't get user: {args[1]}");
                return;
            }
            
            var channel = await user.CreateDMChannelAsync();
            
            try
            {
                await channel.SendMessageAsync(msgContent);
                Console.WriteLine($"daliborBot -> {user.Username}: {msgContent}");
            }
            catch (HttpException ex) when (ex.HttpCode == HttpStatusCode.Forbidden)
            {
                Console.WriteLine($"Boo, I cannot message {user}.");
            }
        }

        private string GetMessageContent(string msg)
        {
            int index = msg.IndexOf(" ");
            index = msg.IndexOf(" ", index + 1);
            return msg.Substring(index, msg.Length - index);
        }
    }
}
