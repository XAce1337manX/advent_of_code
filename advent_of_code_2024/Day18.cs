namespace advent_of_code_2024;

public static class Day18
{
    private record struct Coordinate(int X, int Y);
    
    public static void Solve(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        
        var bytes = lines
            .Select(line => line.Split(','))
            .Select(numbers => new Coordinate(int.Parse(numbers[0]), int.Parse(numbers[1])))
            .ToList();

        var fallenBytes = bytes
            .Take(1024)
            .ToHashSet();

        var partOneTotal = StepsToReachEnd(fallenBytes);
        
        var i = 1;
        while (true)
        {
            fallenBytes = bytes.Take(i).ToHashSet();
            var steps = StepsToReachEnd(fallenBytes);
            if (steps == -1)
            {
                break;
            }
            
            i++;
        }

        var partTwoTotal = bytes[i - 1];

        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static int StepsToReachEnd(ICollection<Coordinate> fallenBytes)
    {
        var endCoordinate = new Coordinate(70, 70);
        
        var queue = new PriorityQueue<Coordinate, int>();
        var visited = new Dictionary<Coordinate, int>();
        
        queue.Enqueue(new Coordinate(), 0);

        while (queue.TryDequeue(out var element, out var priority))
        {
            if (!visited.TryAdd(element, priority))
            {
                continue;
            }

            if (element == endCoordinate)
            {
                return priority;
            }

            if (element is { X: > 0 } && !fallenBytes.Contains(element with { X = element.X - 1 })) 
                queue.Enqueue(element with { X = element.X - 1 }, priority + 1);
            
            if (element is { Y: > 0 } && !fallenBytes.Contains(element with { Y = element.Y - 1 })) 
                queue.Enqueue(element with { Y = element.Y - 1 }, priority + 1);
            
            if (element is { X: < 70 } && !fallenBytes.Contains(element with { X = element.X + 1 })) 
                queue.Enqueue(element with { X = element.X + 1 }, priority + 1);
            
            if (element is { Y: < 70 } && !fallenBytes.Contains(element with { Y = element.Y + 1 })) 
                queue.Enqueue(element with { Y = element.Y + 1 }, priority + 1);
        }

        return -1;
    }
}
