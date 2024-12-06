namespace advent_of_code_2024;

public static class Day06
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var obstructions = new List<(int x, int y)>();
        var initialGuardPosition = (x: 0, y: 0);
        var guardPosition = (x: 0, y: 0);
        
        for (var i = 0; i < input.Length; i++)
        {
            var row = input[i];
            for (var j = 0; j < row.Length; j++)
            {
                switch (input[i][j])
                {
                    case '#':
                        obstructions.Add((x: j, y: i));
                        break;
                    case '^':
                        initialGuardPosition = guardPosition = (x: j, y: i);
                        break;
                }
            }
        }
        
        // (0, 0) is top left corner
        // (x, y) is bottom right row corner
        var directions = new List<(int x, int y)>
        {
            (0, -1), // Up
            (1, 0), // Right
            (0, 1), // Down
            (-1, 0) // Left
        };
        var directionIndex = 0;
        
        var visitedCoordinates = new HashSet<(int x, int y)>();

        var xSize = input[0].Length;
        var ySize = input.Length;
        
        var guardInGrid = true;
        while (guardInGrid)
        {
            // Console.WriteLine($"Guard at: {guardPosition}");
            visitedCoordinates.Add((guardPosition.x, guardPosition.y));
            
            var direction = directions[directionIndex];

            var newPosition = (guardPosition.x + direction.x, guardPosition.y + direction.y);

            if (obstructions.Contains(newPosition))
            {
                directionIndex = (directionIndex + 1) % 4;
                continue;
            }
            
            
            guardPosition = (guardPosition.x + direction.x, guardPosition.y + direction.y);
            if (guardPosition.x < 0 || guardPosition.x >= xSize
                || guardPosition.y < 0 || guardPosition.y >= ySize)
            {
                guardInGrid = false;
            }
        }
        
        
        var newObstructionCoordinates = new HashSet<(int x, int y)>();
        
        var maxGridSize = xSize * ySize;

        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                var newObstruction = (x, y);
                if (obstructions.Contains(newObstruction))
                {
                    continue;
                }

                if (initialGuardPosition == newObstruction)
                {
                    continue;
                }

                guardPosition = initialGuardPosition;
                
                guardInGrid = true;
                directionIndex = 0;
                var steps = 0;
                while (guardInGrid && steps < maxGridSize)
                {
                    var direction = directions[directionIndex];

                    var newPosition = (guardPosition.x + direction.x, guardPosition.y + direction.y);

                    if (obstructions.Contains(newPosition) || newObstruction == newPosition)
                    {
                        directionIndex = (directionIndex + 1) % 4;
                        continue;
                    }
            
                    guardPosition = (guardPosition.x + direction.x, guardPosition.y + direction.y);
                    if (guardPosition.x < 0 || guardPosition.x >= xSize
                                            || guardPosition.y < 0 || guardPosition.y >= ySize)
                    {
                        guardInGrid = false;
                    }

                    steps++;
                }
                
                // Console.WriteLine($"Steps for {newObstruction} -> {steps}");
                if (steps >= maxGridSize)
                {
                    newObstructionCoordinates.Add(newObstruction);
                }
            }
        }

        var partOneTotal = visitedCoordinates.Count;
        var partTwoTotal = newObstructionCoordinates.Count;
        
        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }
}
