using System.Text;

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
        
        YSize = input.Length;
        XSize = input[0].Length;

        for (var y = 0; y < YSize; y++)
        {
            for (var x = 0; x < XSize; x++)
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
        var visited = new Dictionary<(Coordinate coordinate, Direction direction), int>();
        
        queue.Enqueue((start, startingDirection), 0);

        var partOneTotal = 0;
        Direction partOneDirection = Direction.Right;
        while (queue.TryDequeue(out var element, out var priority))
        {
            if (!visited.TryAdd(element, priority))
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
        
        
        // lol it's brute force time. It takes minutes to run xd
        var bestCoordinates = new List<Coordinate>();
        var maxI = visited.Count;
        var i = 0;
        foreach (var visit in visited)
        {
            if (CanReachEnd(visited, walls, end, visit.Key.coordinate, visit.Key.direction, visit.Value, partOneTotal))
            {
                bestCoordinates.Add(visit.Key.coordinate);
            }

            Console.WriteLine($"{i++} out of {maxI}");
        }

        var partTwoTotal = bestCoordinates.Distinct().Count();
        
        PrintMaze(input, visited);

        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static bool CanReachEnd(Dictionary<(Coordinate coordinate, Direction direction), int> globalVisited, ICollection<Coordinate> walls, Coordinate end, Coordinate startCoordinate, Direction direction, int startPriority, int maxPriority)
    {
        var queue = new PriorityQueue<(Coordinate coordinate, Direction direction), int>();
        var visited = new Dictionary<(Coordinate coordinate, Direction direction), int>();
        
        queue.Enqueue((startCoordinate, direction), startPriority);
        
        while (queue.TryDequeue(out var element, out var priority))
        {
            if (priority > maxPriority)
            {
                continue;
            }
            
            if (!visited.TryAdd(element, priority))
            {
                continue;
            }
            
            if (globalVisited.TryGetValue(element, out var globalPriority) && priority > globalPriority)
            {
                if (priority > globalPriority)
                    continue;

                if (priority == globalPriority)
                    return true;
            }
            
            if (element.coordinate == end)
            {
                return true;
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
        
        return false;
    }

    private static void PrintMaze(string[] input, Dictionary<(Coordinate coordinate, Direction direction), int> visited)
    {
        var stringBuilder = new StringBuilder();

        var coordinates = visited
            .Select(x => x.Key)
            .Select(x => x.coordinate)
            .ToList();
        
        for (var y = 0; y < YSize; y++)
        {
            for (var x = 0; x < XSize; x++)
            {
                var coordinate = new Coordinate(x, y);
                var count = coordinates.Count(x => x == coordinate);
                if (count > 0)
                {
                    if (count > 1)
                    {
                        stringBuilder.Append('$');
                    }
                    else
                    {
                        stringBuilder.Append('x');
                    }
                }
                else
                {
                    stringBuilder.Append(input[y][x] == '#' ? ' ' : input[y][x]);
                }
            }
            stringBuilder.AppendLine();
        }

        Console.WriteLine(stringBuilder.ToString());
    }
}
