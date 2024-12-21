namespace advent_of_code_2024;

public static class Day19
{
    private static readonly Dictionary<string, long> ArrangementsByPattern = new();
    
    public static void Solve(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);

        var patterns = lines[0].Split(", ").ToList();

        var designs = lines.Skip(2).ToList();
        
        var partOneTotal =designs.Count(design => NumbersOfPossibleArrangements(patterns, design) > 0);
        var partTwoTotal = designs.Sum(design => NumbersOfPossibleArrangements(patterns, design));

        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static long NumbersOfPossibleArrangements(ICollection<string> patterns, string design)
    {
        if (ArrangementsByPattern.TryGetValue(design, out var arrangements))
        {
            return arrangements;
        }
        var value = 0L;
        foreach (var pattern in patterns)
        {
            if (pattern == design)
            {
                value += 1;
            }
            else if (design.StartsWith(pattern))
            {
                value += NumbersOfPossibleArrangements(patterns, design.Substring(pattern.Length));
            }
        }
        
        ArrangementsByPattern.Add(design, value);
        return value;
    }
}
