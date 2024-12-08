using System.Text;

namespace advent_of_code_2024;

public static class Day08
{
    private record Coordinate(int X, int Y);
    
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var antennas = new Dictionary<char, List<Coordinate>>();

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var key = input[y][x];
                
                if (input[y][x] != '.')
                {
                    if (antennas.TryGetValue(key, out var coordinates))
                    {
                        coordinates.Add(new Coordinate(x, y));
                    }
                    else
                    {
                        antennas.Add(key, [new Coordinate(x, y)]);
                    }
                }
            }
        }
        
        
        var xSize = input[0].Length;
        var ySize = input.Length;
        
        var partOneAntinodes = new HashSet<Coordinate>();
        var partTwoAntinodes = new HashSet<Coordinate>();
        
        foreach (var coordinates in antennas.Values)
        {
            for (var i = 0; i < coordinates.Count; i++)
            {
                for (var j = i + 1; j < coordinates.Count; j++)
                {
                    var firstCoordinate = coordinates[i];
                    var secondCoordinate = coordinates[j];

                    var factor = 0;
                    while (true)
                    {
                        var antinode = new Coordinate
                        (
                            firstCoordinate.X - factor * (secondCoordinate.X - firstCoordinate.X),
                            firstCoordinate.Y - factor * (secondCoordinate.Y - firstCoordinate.Y)
                        );

                        if (0 <= antinode.X && antinode.X < xSize && 0 <= antinode.Y && antinode.Y < ySize)
                        {
                            if (factor == 1) partOneAntinodes.Add(antinode);
                            partTwoAntinodes.Add(antinode);
                        }
                        else
                        {
                            break;
                        }
                        
                        factor++;
                    }

                    factor = 0;
                    while (true)
                    {
                        var antinode = new Coordinate
                        (
                            secondCoordinate.X - factor * (firstCoordinate.X - secondCoordinate.X),
                            secondCoordinate.Y - factor * (firstCoordinate.Y - secondCoordinate.Y)
                        );

                        if (0 <= antinode.X && antinode.X < xSize && 0 <= antinode.Y && antinode.Y < ySize)
                        {
                            if (factor == 1) partOneAntinodes.Add(antinode);
                            partTwoAntinodes.Add(antinode);
                        }
                        else
                        {
                            break;
                        }

                        factor++;
                    }
                }
            }
        }
        
        Console.WriteLine($"Part One: {partOneAntinodes.Count}");
        Console.WriteLine($"Part Two: {partTwoAntinodes.Count}");
    }
}
