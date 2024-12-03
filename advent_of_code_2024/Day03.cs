using System.Text.RegularExpressions;

namespace advent_of_code_2024;

public static class Day03
{
    public static void Solve(string inputFile)
    {        
        var input = File.ReadAllText(inputFile);
        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        
        var partOneTotal = GetTotal(input, regex);

        var inputDonts = input.Split("don't()", StringSplitOptions.RemoveEmptyEntries);
        var inputDos = inputDonts.Select(x =>
        {
            var dos = x.Split("do()", StringSplitOptions.RemoveEmptyEntries).ToList();
            dos.RemoveAt(0);
            return dos;
        }).ToList();
        inputDos.Insert(0, [inputDonts[0]]);
        
        var partTwoTotal = inputDos
            .SelectMany(x => x)
            .Sum(inputDo => GetTotal(inputDo, regex));

        Console.WriteLine($"Part 1: {partOneTotal}");
        Console.WriteLine($"Part 2: {partTwoTotal}");
    }

    private static int GetTotal(string input, Regex regex)
    {
        var total = 0;
        
        var matches = regex.Matches(input);
        foreach (Match match in matches)
        {
            // Console.WriteLine($"{match.Groups[0].Value} - {match.Groups[1].Value} - {match.Groups[2].Value}");
            total += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }

        return total;
    }
}
