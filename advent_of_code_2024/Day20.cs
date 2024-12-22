namespace advent_of_code_2024;

public static class Day20
{
    private record struct Coordinate(int X, int Y);
    
    public static void Solve(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);

        var walls = new List<Coordinate>();
        var tracks = new List<Coordinate>();
        var start = new Coordinate();
        var end = new Coordinate();
        
        for (var j = 0; j < lines.Length; j++)
        {
            for (var i = 0; i < lines[j].Length; i++)
            {
                switch (lines[j][i])
                {
                    case '#':
                        walls.Add(new Coordinate(i, j));
                        break;
                    case '.':
                        tracks.Add(new Coordinate(i, j));
                        break;
                    case 'S':
                        start = new Coordinate(i, j);
                        break;
                    case 'E':
                        end = new Coordinate(i, j);
                        break;
                }
            }
        }
        
        var sortedTracks = new List<Coordinate>();
        var alreadyAddedTracks = new HashSet<Coordinate>();

        var previousTrack = start;
        sortedTracks.Add(start);
        alreadyAddedTracks.Add(start);
        var a = 0;
        while (sortedTracks.Count <= tracks.Count)
        {
            Console.WriteLine($"{a++}/{tracks.Count}");
            var nextTrack = tracks.First(track =>
                !alreadyAddedTracks.Contains(track) &&
                Math.Abs(track.X - previousTrack.X) + Math.Abs(track.Y - previousTrack.Y) == 1);
            sortedTracks.Add(nextTrack);
            alreadyAddedTracks.Add(nextTrack);
            previousTrack = nextTrack;
        }
        sortedTracks.Add(end);
        
        var distanceByTrack = new Dictionary<Coordinate, int>();
        foreach (var track in sortedTracks)
        {
            distanceByTrack[track] = sortedTracks.IndexOf(track);
        }

        var cheatDistances = GetCheatDistances(sortedTracks, walls, distanceByTrack);
        var partOneTotal = cheatDistances.Count(distance => distance >= 100);
        
        var cheatDistancesTwo = GetCheatDistancesPartTwo(sortedTracks, walls, distanceByTrack);
        var partTwoTotal = cheatDistancesTwo.Count(distance => distance >= 100);
        foreach (var grouping in cheatDistancesTwo
                     .GroupBy(distance => distance)
                     .Where(group => group.Key >= 100)
                     .OrderBy(group => group.Key))
        {
            Console.WriteLine($"{grouping.Key} - {grouping.Count()}");
        }
        
        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static List<long> GetCheatDistances(List<Coordinate> sortedTracks, List<Coordinate> walls, Dictionary<Coordinate, int> distanceByTrack)
    {
        var cheatDistances = new List<long>();

        foreach (var track in sortedTracks)
        {
            // Outer
            {
                if (distanceByTrack.TryGetValue(track with { X = track.X + 2 }, out var value)
                    && walls.Contains(track with { X = track.X + 1 }))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
            {
                if (distanceByTrack.TryGetValue(track with { X = track.X - 2 }, out var value)
                    && walls.Contains(track with { X = track.X - 1 }))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
            {
                if (distanceByTrack.TryGetValue(track with { Y = track.Y + 2 }, out var value)
                    && walls.Contains(track with { Y = track.Y + 1 }))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
            {
                if (distanceByTrack.TryGetValue(track with { Y = track.Y - 2 }, out var value)
                    && walls.Contains(track with { Y = track.Y - 1 }))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
            
            // Corners
            {
                if (distanceByTrack.TryGetValue(track with { X = track.X + 1, Y = track.Y + 1}, out var value)
                    && (walls.Contains(track with { X = track.X + 1 }) || walls.Contains(track with { Y = track.Y + 1 })))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
            {
                if (distanceByTrack.TryGetValue(track with { X = track.X - 1, Y = track.Y - 1}, out var value)
                    && (walls.Contains(track with { X = track.X - 1 }) || walls.Contains(track with { Y = track.Y - 1 })))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
            {
                if (distanceByTrack.TryGetValue(track with { X = track.X + 1, Y = track.Y - 1}, out var value)
                    && (walls.Contains(track with { X = track.X + 1 }) || walls.Contains(track with { Y = track.Y - 1 })))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
            {
                if (distanceByTrack.TryGetValue(track with { X = track.X - 1, Y = track.Y + 1}, out var value)
                    && (walls.Contains(track with { X = track.X - 1 }) || walls.Contains(track with { Y = track.Y + 1 })))
                {
                    var distancedSaved = value - distanceByTrack[track] - 2;
                    if (distancedSaved > 0)
                    {
                        cheatDistances.Add(distancedSaved);
                    }
                }
            }
        }

        return cheatDistances;
    }
    
    private static List<long> GetCheatDistancesPartTwo(List<Coordinate> sortedTracks, List<Coordinate> walls, Dictionary<Coordinate, int> distanceByTrack)
    {
        var cheatDistances = new List<long>();

        var sortedClone = sortedTracks.ToList();
        
        var i = 0;
        foreach (var track in sortedTracks)
        {
            Console.WriteLine($"{i++}/{sortedTracks.Count}");
            foreach (var otherTrack in sortedClone)
            {
                if (track == otherTrack) continue;
                
                // Within reach
                var manhattanDistance = Math.Abs(track.X - otherTrack.X) + Math.Abs(track.Y - otherTrack.Y);
                if (manhattanDistance <= 20)
                {
                    // Saves time
                    var distancedSaved =  distanceByTrack[otherTrack] - distanceByTrack[track] - manhattanDistance;
                    if (distancedSaved > 0)
                    {
                        var maxX = Math.Max(otherTrack.X, track.X);
                        var minX = Math.Min(otherTrack.X, track.X);
                        var maxY = Math.Max(otherTrack.Y, track.Y);
                        var minY = Math.Min(otherTrack.Y, track.Y);
                        
                        // Is actually a cheat
                        if (walls.Any(wall => minX <= wall.X && wall.X <= maxX && minY <= wall.Y && wall.Y <= maxY))
                        {
                            cheatDistances.Add(distancedSaved);
                        }
                    }
                }
            }
        }

        return cheatDistances;
    }
}
