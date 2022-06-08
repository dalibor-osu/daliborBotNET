using System.Diagnostics;
using Discord;
using System.Threading;

namespace daliborBotNET.SlashCommands;
using Discord.WebSocket;
using daliborBotNET;

public class BanBrambora : SlashCommand
{
    private string _name;
    private string _description;
    private ulong _guildID;
    public static bool stop = false;

    private DiscordSocketClient _client;
    //private ulong _guildID;
    public BanBrambora(DiscordSocketClient client)
    {
        _name = "banbrambora";
        _description = "Bans brambora";
        _client = client;
        _guildID = 936736299698749521;
        
        command = new Command(_guildID, _name, _description, GetOptions(), Execute, _client);
    }

    private List<SlashCommandOptionBuilder> GetOptions()
    {
        List<SlashCommandOptionBuilder> options = new List<SlashCommandOptionBuilder>();
        SlashCommandOptionBuilder optionA = new SlashCommandOptionBuilder
        {
            Name = "time",
            Description = "Time in seconds to ban Brambora",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };
        
        options.Add(optionA);

        return options;
    }

    public override async Task Execute(SocketSlashCommand command)
    {
        stop = false;
        double a = (double) command.Data.Options.First().Value;
        a = Math.Floor(a);
        var channel = command.Channel;
        ulong bramboraID = 279344155149467648;
        var brambora = _client!.GetUserAsync(bramboraID).Result;
        var guild = _client.GetGuild(_guildID);
        Stopwatch stopwatch = new Stopwatch();

        int nextMessage = 1;
        stopwatch.Start();
        
        await command.RespondAsync($"Banning Brambora in {a}s");
        
        while (stopwatch.ElapsedMilliseconds / 1000 < a)
        {
            if (stopwatch.ElapsedMilliseconds / 1000 > nextMessage)
            {
                await channel.SendMessageAsync($"Banning Brambora in {a - nextMessage}s");
                nextMessage++;
            }

            if (!stop) continue;
            return;
        }

        stopwatch.Stop();
        var user = guild.GetUser(bramboraID);
        await user.KickAsync();
        Program.HandleMessageUser(_client, new []{"msg", bramboraID.ToString()}, "https://discord.gg/Zy3ACk54Xn");
        await channel.SendMessageAsync("Brambora has been banned");
    }
}