using daliborBotNET.SlashCommands;
using Discord.WebSocket;

namespace daliborBotNET;

public class Commands
{
    private readonly List<Command>? _commands;
    private readonly DiscordSocketClient _client;
    
    public Commands(DiscordSocketClient client, List<Command>? commands = null)
    {
        _client = client;
        _commands = commands ?? new List<Command>();
        
        CreateCommands();
    }

    private Task AddCommand(Command command)
    {
        _commands?.Add(command);
        return Task.CompletedTask;
    }
    
    public List<Command>? GetCommands()
    {
        return _commands;
    }

    private async void CreateCommands()
    {
        await AddCommand(new PingPong(_client).command);
        await AddCommand(new Add(_client).command);
        await AddCommand(new Subtract(_client).command);
        await AddCommand(new QuadraticEquation(_client).command);
    }
}