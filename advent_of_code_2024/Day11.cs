namespace advent_of_code_2024;

public static class Day11
{
    private record State(long stone, int blinks);

    private static Dictionary<State, long> resultByState = new();
    
    public static void Solve(string inputFile)
    {
        var line = File.ReadAllLines(inputFile)[0];
        var stones = line.Split(' ').Select(int.Parse).ToArray();

        var partOneTotal = stones.Sum(stone => GetNumberOfStonesAfterBlinks(stone, 25));
        var partTwoTotal = stones.Sum(stone => GetNumberOfStonesAfterBlinks(stone, 75));

        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static long GetNumberOfStonesAfterBlinks(long stone, int blinks)
    {
        var state = new State(stone, blinks);
        
        if (resultByState.TryGetValue(state, out var value))
        {
            return value;
        }

        if (blinks <= 0)
        {
            resultByState.TryAdd(state, 1);
            return 1;
        }
        
        // Split stone
        long stateResult;
        if (stone == 0)
        {
            stateResult = GetNumberOfStonesAfterBlinks(1, blinks - 1);
        }
        else if (Math.Floor(Math.Log10(stone) + 1) % 2 == 0) // even
        {
            var stoneString = stone.ToString();
            var firstHalf = long.Parse(stoneString[..(stoneString.Length / 2)]);
            var secondHalf = long.Parse(stoneString[(stoneString.Length / 2)..]);
            
            var firstHalfResult = GetNumberOfStonesAfterBlinks(firstHalf, blinks - 1);
            var secondHalfResult = GetNumberOfStonesAfterBlinks(secondHalf, blinks - 1);

            stateResult = firstHalfResult + secondHalfResult;
        }
        else
        {
            stateResult = GetNumberOfStonesAfterBlinks(stone * 2024, blinks - 1);
        }
        
        resultByState.TryAdd(state, stateResult);
        return stateResult;
    }
}
