namespace advent_of_code_2025;

public static class Day02
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var ranges = input[0].Split(',');

        var partOneTotal = 0L;
        foreach (var range in ranges)
        {
            var idRanges = range.Split('-');
            
            var start = long.Parse(idRanges[0]);
            var end = long.Parse(idRanges[1]);

            for (var id = start; id <= end; id++)
            {
                var digits = (long)Math.Round(Math.Floor(Math.Log10(id) + 1));

                // Odd is automatically valid
                if (digits % 2 == 1)
                {
                    continue;
                }
                
                var splitter = Math.Pow(10, digits / 2);
                
                var leftSide = (long)(id / splitter);
                var rightSide = (long)(id % splitter);

                if (leftSide == rightSide)
                {
                    partOneTotal += id;
                    Console.WriteLine($"Invalid {id} in range {start} to {end}");
                }
            }
        }
        
        Console.WriteLine($"Day 2 - Part 1: {partOneTotal}");
    }
}
