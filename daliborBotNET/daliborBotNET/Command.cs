using System.Runtime.CompilerServices;
using Discord;
using Discord.WebSocket;

namespace daliborBotNET;

public class Command
{
    public ulong? guildId;
    public string commandName;
    private string _commandDescription;
    private List<SlashCommandOptionBuilder> _options;
    private Func<SocketSlashCommand, Task> _commandAction;
    public bool isGlobal;
    private DiscordSocketClient _client;
    private SlashCommandBuilder _builder;
    
    public Command(ulong guildId, string commandName, string commandDescription, List<SlashCommandOptionBuilder> options, Func<SocketSlashCommand, Task> commandAction, DiscordSocketClient client)
    {
        this.guildId = guildId;
        this.commandName = commandName;
        _commandDescription = commandDescription;
        _options = options;
        _commandAction = commandAction;
        _client = client;

        GenerateCommand();
    }
    
    public Command(ulong guildId, string commandName, string commandDescription, Func<SocketSlashCommand, Task> commandAction, DiscordSocketClient client)
    {
        this.guildId = guildId;
        this.commandName = commandName;
        _commandDescription = commandDescription;
        _options = new List<SlashCommandOptionBuilder>();
        _commandAction = commandAction;
        _client = client;

        GenerateCommand();
    }
    
    public Command(string commandName, string commandDescription, Func<SocketSlashCommand, Task> commandAction, DiscordSocketClient client)
    {
        guildId = null;
        this.commandName = commandName;
        _commandDescription = commandDescription;
        _options = new List<SlashCommandOptionBuilder>();
        _commandAction = commandAction;
        _client = client;

        GenerateCommand();
    }
    
    public Command(string commandName, string commandDescription, List<SlashCommandOptionBuilder> options, Func<SocketSlashCommand, Task> commandAction, DiscordSocketClient client)
    { 
        guildId = null;
        this.commandName = commandName;
        _commandDescription = commandDescription;
        _options = options;
        _commandAction = commandAction;
        _client = client;

        GenerateCommand();
    }

    private void GenerateCommand()
    {
        if (guildId == null) isGlobal = true;
        _builder = new SlashCommandBuilder();
        _builder.WithName(commandName);
        _builder.WithDescription(_commandDescription);
        if (_options.Count > 0) _builder.AddOptions(_options.ToArray());
    }

    public SlashCommandBuilder GetBuilder()
    {
        return _builder;   
    }
    
    public virtual async void Execute(SocketSlashCommand command)
    {
        await _commandAction(command);
    }

    public virtual void AddCommand(List<Command> commands)
    {
        commands.Add(this);
    }
}