using Discord;
using Discord.WebSocket;

namespace daliborBotNET.SlashCommands;

public class DivideComplex : SlashCommand
{
    private string _name;
    private string _description;
    //private ulong _guildID;
    public DivideComplex(DiscordSocketClient client)
    {
        _name = "dividecomplex";
        _description = "Divides two complex numbers";
        
        command = new Command(976544106438348810, _name, _description, GetOptions(), Execute, client);
    }

    private List<SlashCommandOptionBuilder> GetOptions()
    {
        List<SlashCommandOptionBuilder> options = new List<SlashCommandOptionBuilder>();
        SlashCommandOptionBuilder optionA = new SlashCommandOptionBuilder
        {
            Name = "a-real",
            Description = "The first number",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };

        SlashCommandOptionBuilder optionB = new SlashCommandOptionBuilder
        {
            Name = "a-imaginary",
            Description = "The second number",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };
        
        SlashCommandOptionBuilder optionC = new SlashCommandOptionBuilder
        {
            Name = "b-real",
            Description = "The first number",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };

        SlashCommandOptionBuilder optionD = new SlashCommandOptionBuilder
        {
            Name = "b-imaginary",
            Description = "The second number",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };
        
        options.Add(optionA);
        options.Add(optionB);
        options.Add(optionC);
        options.Add(optionD);

        return options;
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        Complex a = new Complex((double)command.Data.Options.ElementAt(0).Value, (double)command.Data.Options.ElementAt(1).Value);
        Complex b = new Complex((double)command.Data.Options.ElementAt(2).Value, (double)command.Data.Options.ElementAt(3).Value);

        Complex result = Complex.Divide(a, b);
        await command.RespondAsync($"({a.ToString()}) / ({b.ToString()}) = {result.ToString()}");
    }
}