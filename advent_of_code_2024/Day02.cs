namespace advent_of_code_2024;

public static class Day02
{
    public static void Solve(string inputFile)
    {
        var partOneTotal = 0;
        var partTwoTotal = 0;
        
        var input = File.ReadAllLines(inputFile);
        foreach (var line in input)
        {
            var numbers = line
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            // Part 1
            var isSafe = IsSafe(numbers);
            if (isSafe) partOneTotal += 1;
            
            // Part 2 lol brute force
            var numbersWithTolerance = numbers
                .Select(_ => numbers.ToList())
                .ToList();
            var isSafeWithTolerance = numbersWithTolerance
                .Select((num, i) => IsSafe(num, i))
                .ToList();
            if (isSafe || isSafeWithTolerance.Any(x => x)) partTwoTotal += 1;
        }

        Console.WriteLine($"Part 1: {partOneTotal}");
        Console.WriteLine($"Part 2: {partTwoTotal}");
    }

    private static bool IsSafe(List<int> numbers, int? indexRemove = null)
    {
        // Part 2
        if (indexRemove.HasValue) numbers.RemoveAt(indexRemove.Value);

        var isSafe = true;
        
        var start = numbers[0];
        var end = numbers[^1];

        var isAscending = end - start > 0;
        
        for (var i = 1; i < numbers.Count; i++)
        {
            var current = numbers[i];
            var previous = numbers[i - 1];
            var difference = isAscending
                ? current - previous
                : previous - current; 
                
            if (difference is < 1 or > 3)
            {
                isSafe = false;
            }
        }
        
        return isSafe;
    }
}
