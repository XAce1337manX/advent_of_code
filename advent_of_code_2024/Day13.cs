using System.Text.RegularExpressions;

namespace advent_of_code_2024;

public static class Day13
{
    private readonly record struct Machine(long ButtonAx, long ButtonAy, long ButtonBx, long ButtonBy, long PrizeX, long PrizeY)
    {
        public long ButtonAx { get; } = ButtonAx;
        public long ButtonAy { get; } = ButtonAy;
        public long ButtonBx { get; } = ButtonBx;
        public long ButtonBy { get; } = ButtonBy;
        public long PrizeX { get; } = PrizeX;
        public long PrizeY { get; } = PrizeY;
    }

    public static void Solve(string inputFile)
    {
        var input = File.ReadAllText(inputFile);

        var regex = new Regex(@"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)");

        var matches = regex.Matches(input);
        var machines = matches.Select(match => new Machine(
                long.Parse(match.Groups[1].Value),
                long.Parse(match.Groups[2].Value),
                long.Parse(match.Groups[3].Value),
                long.Parse(match.Groups[4].Value),
                long.Parse(match.Groups[5].Value),
                long.Parse(match.Groups[6].Value)
            ))
            .ToList();

        var partOneTotal = 0L;
        var partTwoTotal = 0L;
        
        foreach (var machine in machines)
        {
            partOneTotal += GetMachineTokens(machine);
            partTwoTotal += GetMachineTokens(machine, 10000000000000L);

            // For fun: My original part 1 solution which dies in part 2
            // var buttonPressLimit = 100;
            // var possibleCombinations = new List<(long buttonA, long buttonB)>();
            //
            // for (var buttonAPresses = 0; buttonAPresses < buttonPressLimit; buttonAPresses++)
            // {
            //     var remaining = machine.PrizeX - buttonAPresses * machine.ButtonAx;
            //
            //     if (remaining % machine.ButtonBx == 0)
            //     {
            //         var buttonBPresses = remaining / machine.ButtonBx;
            //
            //         if (buttonAPresses * machine.ButtonAy + buttonBPresses * machine.ButtonBy == machine.PrizeY)
            //         {
            //             possibleCombinations.Add((buttonAPresses, buttonBPresses));
            //         }
            //         
            //     }
            // }
            // var minimumPresses = possibleCombinations.DefaultIfEmpty().MinBy(combination => 3 * combination.buttonA + combination.buttonB);
            //     
            // var minimumTokens = minimumPresses.buttonA * 3 + minimumPresses.buttonB;
        }


        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part One: {partTwoTotal}");
    }

    private static long GetMachineTokens(Machine machine, long prizeExtra = 0)
    {
        // Looks like there's only one solution
        // Cramer's Rule to solve system of equations
        //
        //               a1 * x +               b1 * y = c1
        //               a2 * x +               b2 * y = c2
        //
        // machine.ButtonAx * x + machine.ButtonBx * y = machine.PrizeX;
        // machine.ButtonAy * x + machine.ButtonBy * y = machine.PrizeY;
        
        var detD = (machine.ButtonAx * machine.ButtonBy - machine.ButtonBx * machine.ButtonAy);
        var detX = ((machine.PrizeX + prizeExtra) * machine.ButtonBy - machine.ButtonBx * (machine.PrizeY + prizeExtra));
        var detY = (machine.ButtonAx * (machine.PrizeY + prizeExtra) - (machine.PrizeX + prizeExtra) * machine.ButtonAy);

        var buttonAPresses = detX / detD;
        var buttonBPresses = detY / detD;
            
        // Make sure the solution is actually possible
        if (buttonAPresses * machine.ButtonAx + buttonBPresses * machine.ButtonBx == (machine.PrizeX + prizeExtra) &&
            buttonAPresses * machine.ButtonAy + buttonBPresses * machine.ButtonBy == (machine.PrizeY + prizeExtra))
            return buttonAPresses * 3 + buttonBPresses;

        return 0;
    }
}
