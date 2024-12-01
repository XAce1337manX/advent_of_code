namespace advent_of_code_2024;

public static class Day01
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        
        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in input)
        {
            var numbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(numbers[0]));
            right.Add(int.Parse(numbers[1]));
        }

        left.Sort();
        right.Sort();

        var partOneTotal = 0;
        var partTwoTotal = 0;
        for (var i = 0; i < left.Count; i++)
        {
            var number = left[i];
            var similarity = right.Count(x => x == number);
            partOneTotal += Math.Abs(left[i] - right[i]);
            partTwoTotal += similarity * number;
        }

        Console.WriteLine(partOneTotal);
        Console.WriteLine(partTwoTotal);
    }
}
