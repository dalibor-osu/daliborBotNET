using Discord;

namespace daliborBotNET.SlashCommands;
using Discord.WebSocket;

public class Add : SlashCommand
{
    private string _name;
    private string _description;
    //private ulong _guildID;
    public Add(DiscordSocketClient client)
    {
        _name = "add";
        _description = "Adds a new command to the bot";
        _description = "Adds two numbers together";
        
        command = new Command(835534125544112189, _name, _description, Execute, client);
    }

    private List<SlashCommandOptionBuilder> GetOptions()
    {
        SlashCommandOptionBuilder builder = new SlashCommandOptionBuilder();
        
        var options = new List<SlashCommandOptionBuilder>
        {
            builder.AddOption("a", ApplicationCommandOptionType.Number, "First number to add", true),
            builder.AddOption("b", ApplicationCommandOptionType.Number, "Second number to add", true)
        };
        
        return options;
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        await command.RespondAsync("1 + 1 = 2");
    }
}