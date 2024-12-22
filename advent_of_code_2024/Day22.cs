namespace advent_of_code_2024;

public static class Day22
{
    public static void Solve(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);

        var secretNumbers = lines.Select(long.Parse).ToList();
        
        var partOneTotal = 0L;
        
        foreach (var secretNumber in secretNumbers)
        {
            var nextNumber = secretNumber;
            foreach (var i in Enumerable.Range(1, 2000))
            {
                nextNumber = GetNextSecretNumber(nextNumber);
            }
            
            partOneTotal += nextNumber;
        }
        
        Console.WriteLine($"Part One: {partOneTotal}");
        // Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static Dictionary<long, long> secretMemo = new();

    private static long GetNextSecretNumber(long input)
    {
        if (secretMemo.TryGetValue(input, out var result))
        {
            return result;
        }

        var newSecret = input;
        
        var stepOneResult = newSecret * 64;
        newSecret = stepOneResult ^ newSecret;
        newSecret = (newSecret % 16777216);

        var stepTwoResult = newSecret / 32;
        newSecret = stepTwoResult ^ newSecret;
        newSecret = (newSecret % 16777216);
        
        var stepThreeResult = newSecret * 2048;
        newSecret = stepThreeResult ^ newSecret;
        newSecret = (newSecret % 16777216);

        secretMemo[input] = newSecret;
        return newSecret;
    }
    
}
