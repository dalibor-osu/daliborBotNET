using Discord;

namespace daliborBotNET.SlashCommands;
using Discord.WebSocket;

public class StopBan : SlashCommand
{
    public static bool stop = false;
    private string _name;
    private string _description;
    //private ulong _guildID;
    public StopBan(DiscordSocketClient client)
    {
        _name = "stopban";
        _description = "Stops banning Brambora";
        
        command = new Command(936736299698749521, _name, _description, Execute, client);
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        BanBrambora.stop = true;
        await command.RespondAsync($"No longer banning Brambora.");
    }
}