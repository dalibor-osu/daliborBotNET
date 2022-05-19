using Discord;

namespace daliborBotNET.SlashCommands;

using Discord.WebSocket;

public class QuadraticEquation : SlashCommand
{
    public QuadraticEquation(DiscordSocketClient client)
    {
        command = new Command(976544106438348810, "quad-equation", "Solves quadratic equation", GetOptions(), Execute, client);
    }
    
    private List<SlashCommandOptionBuilder> GetOptions()
    {
        List<SlashCommandOptionBuilder> options = new List<SlashCommandOptionBuilder>();
        SlashCommandOptionBuilder optionA = new SlashCommandOptionBuilder
        {
            Name = "a",
            Description = "Number that multiplies the x^2",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };

        SlashCommandOptionBuilder optionB = new SlashCommandOptionBuilder
        {
            Name = "b",
            Description = "Number that multiplies the x",
            Type = ApplicationCommandOptionType.Number,
            IsRequired = true
        };
        
        SlashCommandOptionBuilder optionC = new SlashCommandOptionBuilder
        {
            Name = "c",
            Description = "Number that is added to the result of a*x^2 + b*x",
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
        var a = (double) command.Data.Options.First().Value;
        var b = (double) command.Data.Options.ElementAt(1).Value;
        var c = (double) command.Data.Options.Last().Value;
        
        if(a == 0) await command.RespondAsync("This is not a quadratic equation. Number \"a\" has to be different than 0.");
        
        var result = Solve(a, b, c);

        if (result == null)
        {
            var complexResult = SolveComplex(a, b, c);
            await command.RespondAsync($"This equation has complex solution: x1 = {complexResult[0].Real} + {complexResult[0].Imaginary}i, x2 = {complexResult[1].Real} + {complexResult[1].Imaginary}i");
        }
        else if (result[0] == result[1])
        {
            await command.RespondAsync($"This equation has one solution: {result[0]}");
        }
        else
        {
            await command.RespondAsync($"This equation has two solutions: x1 = {result[0]}, x2 = {result[1]}");
        }
    }

    private double[]? Solve(double a, double b, double c)
    {
        var d = b * b - 4 * a * c;
        if (d < 0) return null;
        
        var x1 = (-b + Math.Sqrt(d)) / (2 * a);
        var x2 = (-b - Math.Sqrt(d)) / (2 * a);

        return new[] {x1, x2};
    }

    private Complex[] SolveComplex(double a, double b, double c)
    {
        var d = b * b - 4 * a * c;
        Complex dSqrt = new Complex(0, Math.Sqrt(Math.Abs(d)));
        
        var x1 = new Complex(-b / (2 * a), dSqrt.Imaginary / (2 * a));
        var x2 = new Complex(-b / (2 * a), - dSqrt.Imaginary / (2 * a));
        
        return new[] {x1, x2};
    }
}