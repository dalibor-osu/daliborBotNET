using Discord.WebSocket;

namespace daliborBotNET.SlashCommands;

public class PingPong : SlashCommand
{
    //private DiscordSocketClient _client;
    public PingPong(DiscordSocketClient client)
    {
        //_client = client;
        command = new Command(835534125544112189, "ping", "Answers with Pong", Execute, client);
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        await command.RespondAsync("Pong!");
    }
}