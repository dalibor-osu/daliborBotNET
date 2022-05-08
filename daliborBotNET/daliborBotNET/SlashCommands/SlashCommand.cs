using Discord.WebSocket;

namespace daliborBotNET.SlashCommands;

public abstract class SlashCommand
{
    public Command command;
    
    public virtual async Task Execute(SocketSlashCommand command)
    {
        await command.RespondAsync($"Successfully executed {command.Data.Name}");
    }
}