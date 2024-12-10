using System.Text;

namespace advent_of_code_2024;

public static class Day10
{
    private record Coordinate(int X, int Y);

    private static string[] input = null!;
    private static int ySize, xSize;
    
    private static Dictionary<Coordinate, List<Coordinate>> reachableTrailHeads = new();
    
    public static void Solve(string inputFile)
    {
        input = File.ReadAllLines(inputFile);
        
        ySize = input.Length;
        xSize = input[0].Length;

        for (var y = 0; y < ySize; y++)
        {
            for (var x = 0; x < xSize; x++)
            {
                var number = input[y][x];
                if (number == '0')
                {
                    var startPosition = new Coordinate(x, y);
                    reachableTrailHeads[startPosition] = [];
                    FindReachableTrailHeads(startPosition, x, y, number);
                }
            }
        }

        var partOneTotal = reachableTrailHeads.Values.SelectMany(x => x).Distinct().Count();
        var partTwoTotal = reachableTrailHeads.Values.SelectMany(x => x).Count();
        
        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static void FindReachableTrailHeads(Coordinate startPosition, int x, int y, char step)
    {
        if (step == '9')
        {
            reachableTrailHeads[startPosition].Add(new Coordinate(x, y));
            return;
        }

        var nextStep = (char)(step + 1);

        if (y > 0 && input[y - 1][x] == nextStep) FindReachableTrailHeads(startPosition, x, y - 1, nextStep);
        if (y < ySize - 1 && input[y + 1][x] == nextStep) FindReachableTrailHeads(startPosition, x, y + 1, nextStep);
        if (x > 0 && input[y][x - 1] == nextStep) FindReachableTrailHeads(startPosition, x - 1, y, nextStep);
        if (x < xSize - 1 && input[y][x + 1] == nextStep) FindReachableTrailHeads(startPosition, x + 1, y, nextStep);
    }
}
