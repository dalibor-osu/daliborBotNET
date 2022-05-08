using Discord;
using Discord.WebSocket;

namespace daliborBotNET.SlashCommands;

public class PingPong : SlashCommand
{
    //private DiscordSocketClient _client;
    public PingPong(DiscordSocketClient client)
    {
        //_client = client;
        command = new Command(835534125544112189, "ping", "Answers with Pong", GetOptions(), Execute, client);
    }
    
    private List<SlashCommandOptionBuilder> GetOptions()
    {
        List<SlashCommandOptionBuilder> options = new List<SlashCommandOptionBuilder>();
        SlashCommandOptionBuilder optionA = new SlashCommandOptionBuilder
        {
            Name = "a",
            Description = "Number that multiplies x^2",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };

        SlashCommandOptionBuilder optionB = new SlashCommandOptionBuilder
        {
            Name = "b",
            Description = "Number that multiplies x",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };
        
        SlashCommandOptionBuilder optionC = new SlashCommandOptionBuilder
        {
            Name = "c",
            Description = "Number that adds to the result of ax^2 + bx",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };
        
        options.Add(optionA);
        options.Add(optionB);
        options.Add(optionC);

        return options;
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        await command.RespondAsync("Pong!");
    }
}