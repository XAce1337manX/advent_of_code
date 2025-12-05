namespace advent_of_code_2025;

public static class Day03
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        
        Console.WriteLine($"Day 2 - Part 1: {ComputeForBatteryLimit(input, 2)}");
        Console.WriteLine($"Day 2 - Part 2: {ComputeForBatteryLimit(input, 12)}");
    }

    private static long ComputeForBatteryLimit(string[] input, int batteryLimit)
    {
        var total = 0L;
        
        foreach (var bank in input)
        {
            var indexes = new List<int>();
            var offset = 0;
            for (var i = batteryLimit; i > 0; i--)
            {
                var maxChar = '0';
                // Console.WriteLine($"Looking at {new string(' ',offset)}{bank[offset..^i]}");
                for (var j = offset; j <= bank.Length - i; j++)
                {
                    if (maxChar < bank[j])
                    {
                        maxChar = bank[j];
                    }
                }
                
                var index = bank[offset..].IndexOf(maxChar) + offset;
                
                // Console.WriteLine($"{maxChar}->{index}");
                indexes.Add(index);
                offset = index + 1;
            }

            var joltage = indexes.Aggregate("", (current, i) => current + bank[i]);

            total += long.Parse(joltage);
        }

        return total;
    }
}
