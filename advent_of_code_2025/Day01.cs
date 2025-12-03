namespace advent_of_code_2025;

public static class Day01
{
    private const int DialLength = 100;
    
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var zeroDialCountP1 = 0;
        var zeroDialCountP2 = 0;
        var dialPosition = 50;
        foreach (var line in input)
        {
            var rightDirection = line[0] == 'R';

            var turnAmount = int.Parse(line[1..]);

            var currentPosition = dialPosition;
            var nextPosition = rightDirection
                ? dialPosition + turnAmount
                : dialPosition - turnAmount;

            dialPosition = (nextPosition + DialLength) % DialLength;

            // P1 - Lands on 0
            if (dialPosition == 0)
            {
                zeroDialCountP1++;
            }

            // P2 - Everytime it passes or lands on 0
            if (rightDirection)
            {
                zeroDialCountP2 += Math.Abs((int)Math.Floor(nextPosition / (double)DialLength)
                                            - (int)Math.Floor(currentPosition / (double)DialLength));
            }
            else
            {
                zeroDialCountP2 += Math.Abs((int)Math.Ceiling(nextPosition / (double)DialLength)
                                            - (int)Math.Ceiling(currentPosition / (double)DialLength));
            }
        }

        Console.WriteLine($"Day 1 - Part 1: {zeroDialCountP1}");
        Console.WriteLine($"Day 2 - Part 2: {zeroDialCountP2}");
    }
}
