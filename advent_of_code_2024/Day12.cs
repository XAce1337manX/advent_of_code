namespace advent_of_code_2024;

public static class Day12
{
    private static List<Region> regions = [];

    private static HashSet<Coordinate> visitedCoordinates = [];

    private static string[] input = null!;
    
    private record struct Coordinate(int X, int Y);
    private class Region
    {
        public char Letter { get; set; }
        public List<(Coordinate coordinate, int neighbours, int corners)> Points { get; set; } = [];

        public void PopulatePoints(int x, int y)
        {
            var coordinate = new Coordinate(x, y);
            if (!visitedCoordinates.Add(coordinate))
            {
                return;
            }
            
            var hasUpLetter = y < input.Length - 1 && input[y + 1][x] == Letter;
            var hasDownLetter = y > 0 && input[y - 1][x] == Letter;
            var hasLeftLetter = x > 0 && input[y][x - 1] == Letter;
            var hasRightLetter = x < input[0].Length - 1 && input[y][x + 1] == Letter;
            
            var hasUpLeftLetter = y < input.Length - 1 && x > 0 && input[y + 1][x - 1] == Letter;
            var hasUpRightLetter = y < input.Length - 1 && x < input[0].Length - 1 && input[y + 1][x + 1] == Letter;
            var hasDownLeftLetter = y > 0 && x > 0 && input[y - 1][x - 1] == Letter;
            var hasDownRightLetter = y > 0 && x < input[0].Length - 1 && input[y - 1][x + 1] == Letter;
            
            var neighbours = 0;
            
            if (hasUpLetter)
            {
                neighbours++;
                PopulatePoints(x, y + 1);
            }
            
            if (hasDownLetter)
            {
                neighbours++;
                PopulatePoints(x, y - 1);
            }
            
            if (hasLeftLetter)
            {
                neighbours++;
                PopulatePoints(x - 1, y);
            }
            
            if (hasRightLetter)
            {
                neighbours++;
                PopulatePoints(x + 1, y);
            }
            
            var corners = 0;
            
            if (!hasUpLetter && !hasLeftLetter) corners++;
            if (!hasUpLetter && !hasRightLetter) corners++;
            if (!hasDownLetter && !hasLeftLetter) corners++;
            if (!hasDownLetter && !hasRightLetter) corners++;
            
            if (hasUpLetter && hasLeftLetter && !hasUpLeftLetter) corners++;
            if (hasUpLetter && hasRightLetter && !hasUpRightLetter) corners++;
            if (hasDownLetter && hasLeftLetter && !hasDownLeftLetter) corners++;
            if (hasDownLetter && hasRightLetter && !hasDownRightLetter) corners++;
            
            Points.Add((coordinate, neighbours, corners));
        }
    }

    public static void Solve(string inputFile)
    {
        input = File.ReadAllLines(inputFile);

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                if (visitedCoordinates.Contains(coordinate))
                {
                    continue;
                }
                
                var region = new Region { Letter = input[y][x] };
                region.PopulatePoints(coordinate.X, coordinate.Y);
                regions.Add(region);
            }
        }

        var partOneTotal = 0L;
        var partTwoTotal = 0L;
        foreach (var region in regions)
        {
            var area = region.Points.Count;
            var perimeter = region.Points.Sum(point => 4 - point.neighbours);
            var corners = region.Points.Sum(point => point.corners);
            
            partOneTotal += area * perimeter;
            partTwoTotal += area * corners;
            
            // Console.WriteLine($"{region.Letter}: P1 - {perimeter * corners} | P2 - {area * corners}");
        }
        
        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part One: {partTwoTotal}");
    }
}
