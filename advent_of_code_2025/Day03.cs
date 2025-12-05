namespace advent_of_code_2025;

public static class Day03
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var partOneTotal = 0;
        
        foreach (var bank in input)
        {
            var leftMax = '0';
            var rightMax = '0';
            for (var i = 0; i < bank.Length - 1; i++)
            {
                var leftBattery = bank[i];
                if (leftBattery <= leftMax)
                {            
                    continue;
                }
                
                leftMax = leftBattery;

                rightMax = '0';
                for (var j = i + 1; j < bank.Length; j++)
                {
                    var rightBattery = bank[j];
                    if (rightMax < rightBattery)
                    {
                        rightMax = rightBattery;
                    }
                }
            }

            Console.WriteLine($"J {leftMax}-{rightMax}");
            var joltage = int.Parse($"{leftMax}{rightMax}");
            partOneTotal += joltage;
        }
        
        Console.WriteLine($"Day 2 - Part 1: {partOneTotal}");
    }
}
