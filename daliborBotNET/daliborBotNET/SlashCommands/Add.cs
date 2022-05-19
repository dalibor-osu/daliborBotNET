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
        _description = "Adds two numbers together";
        
        command = new Command(976544106438348810, _name, _description, GetOptions(), Execute, client);
    }

    private List<SlashCommandOptionBuilder> GetOptions()
    {
        List<SlashCommandOptionBuilder> options = new List<SlashCommandOptionBuilder>();
        SlashCommandOptionBuilder optionA = new SlashCommandOptionBuilder
        {
            Name = "a",
            Description = "The first number",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };

        SlashCommandOptionBuilder optionB = new SlashCommandOptionBuilder
        {
            Name = "b",
            Description = "The second number",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };
        options.Add(optionA);
        options.Add(optionB);

        return options;
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        var a = (double) command.Data.Options.First().Value;
        var b = (double) command.Data.Options.Last().Value;
        
        await command.RespondAsync($"{a} + {b} = {a + b}");
    }
}