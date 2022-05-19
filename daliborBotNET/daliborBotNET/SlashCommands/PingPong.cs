using Discord;
using Discord.WebSocket;

namespace daliborBotNET.SlashCommands;

public class PingPong : SlashCommand
{
    //private DiscordSocketClient _client;
    public PingPong(DiscordSocketClient client)
    {
        //_client = client;
        command = new Command(976544106438348810, "ping", "Answers with Pong", Execute, client);
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        await command.RespondAsync("Pong!");
    }
}