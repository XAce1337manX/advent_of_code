namespace advent_of_code_2024;

public static class Day16
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private record struct Coordinate(int X, int Y);
    
    private static int XSize, YSize;
        
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        
        Coordinate start = new();
        Coordinate end = new();
        
        List<Coordinate> walls = [];

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                switch (input[y][x])
                {
                    case '#':
                        walls.Add(coordinate);
                        break;
                    case 'S':
                        start = coordinate;
                        break;
                    case 'E':
                        end = coordinate;
                        break;
                }
            }
        }

        const Direction startingDirection = Direction.Right;

        var queue = new PriorityQueue<(Coordinate coordinate, Direction direction), int>();
        var visited = new HashSet<(Coordinate coordinate, Direction direction)>();
        
        queue.Enqueue((start, startingDirection), 0);

        var partOneTotal = 0;
        Direction partOneDirection = Direction.Right;
        while (queue.TryDequeue(out var element, out var priority))
        {
            if (!visited.Add(element))
            {
                continue;
            }
            
            if (element.coordinate == end)
            {
                partOneDirection = element.direction;
                partOneTotal = priority;
                break;
            }

            Coordinate moveCoordinate; 
            switch (element.direction)
            {
                case Direction.Up:
                    moveCoordinate = element.coordinate with { Y = element.coordinate.Y - 1 };
                    if (!walls.Contains(moveCoordinate))
                        queue.Enqueue((moveCoordinate, Direction.Up), priority + 1);
                    
                    queue.Enqueue((element.coordinate, Direction.Left), priority + 1000);
                    queue.Enqueue((element.coordinate, Direction.Right), priority + 1000);
                    break;
                case Direction.Down:
                    moveCoordinate = element.coordinate with { Y = element.coordinate.Y + 1 };
                    if (!walls.Contains(moveCoordinate))
                        queue.Enqueue((moveCoordinate, Direction.Down), priority + 1);
                    
                    queue.Enqueue((element.coordinate, Direction.Left), priority + 1000);
                    queue.Enqueue((element.coordinate, Direction.Right), priority + 1000);
                    break;
                case Direction.Left:
                    moveCoordinate = element.coordinate with { X = element.coordinate.X - 1 };
                    if (!walls.Contains(moveCoordinate))
                        queue.Enqueue((moveCoordinate, Direction.Left), priority + 1);
                    
                    queue.Enqueue((element.coordinate, Direction.Up), priority + 1000);
                    queue.Enqueue((element.coordinate, Direction.Down), priority + 1000);
                    break;
                case Direction.Right:
                    moveCoordinate = element.coordinate with { X = element.coordinate.X + 1 };
                    if (!walls.Contains(moveCoordinate))
                        queue.Enqueue((moveCoordinate, Direction.Right), priority + 1);
                    
                    queue.Enqueue((element.coordinate, Direction.Up), priority + 1000);
                    queue.Enqueue((element.coordinate, Direction.Down), priority + 1000);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Console.WriteLine($"Part One: {partOneTotal}");
    }
}
