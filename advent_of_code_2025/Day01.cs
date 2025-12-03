namespace advent_of_code_2025;

public static class Day01
{
    private const int DialLength = 100;
    
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var zeroDialCount = 0;
        var dialPosition = 50;
        foreach (var line in input)
        {
            var rightDirection = line[0] == 'R';

            var turnAmount = int.Parse(line[1..]);

            if (rightDirection)
            {
                dialPosition += turnAmount;
            }
            else
            {
                dialPosition -= turnAmount;
            }

            dialPosition = (dialPosition + DialLength) % DialLength;

            if (dialPosition == 0)
            {
                zeroDialCount++;
            }
            
            //Console.WriteLine(dialPosition);
        }

        Console.WriteLine($"Day 1 - Part 1: {zeroDialCount}");
    }
}
