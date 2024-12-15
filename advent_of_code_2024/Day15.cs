using System.Text;

namespace advent_of_code_2024;

// This was rough
public static class Day15
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private record struct Coordinate(int X, int Y);
    
    private static Dictionary<Coordinate, char> Tiles = new();
    private static Dictionary<Coordinate, char> ScaledTiles = new();
    private static int XSize, YSize;
        
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        
        XSize = input[0].Length;
        YSize = input.Count(line => !string.IsNullOrWhiteSpace(line) && line[0] == '#');
            
        var directions = new List<Direction>();
        
        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            if (line[0] == '#')
            {
                for (var j = 0; j < line.Length; j++)
                {
                    var position = new Coordinate(j, i);
                    Tiles.Add(position, line[j]);
                    
                    var scaledPosition = new Coordinate(2 * j, i);
                    var rightScaledPosition = new Coordinate(2 * j + 1, i);
                    switch (line[j])
                    {
                        case '@':
                            ScaledTiles.Add(scaledPosition, line[j]);
                            ScaledTiles.Add(rightScaledPosition, '.');
                            break;
                        case 'O':
                            ScaledTiles.Add(scaledPosition, '[');
                            ScaledTiles.Add(rightScaledPosition, ']');
                            break;
                        default:
                            ScaledTiles.Add(scaledPosition, line[j]);
                            ScaledTiles.Add(rightScaledPosition, line[j]);
                            break;
                    }
                    
                }
            }
            else
            {
                foreach (var direction in line)
                {
                    switch (direction)
                    {
                        case '^':
                            directions.Add(Direction.Up);
                            break;
                        case 'v':
                            directions.Add(Direction.Down);
                            break;
                        case '<':
                            directions.Add(Direction.Left);
                            break;
                        case '>':
                            directions.Add(Direction.Right);
                            break;
                    }
                }
            }

        }
        
        var robotPosition = Tiles.First(tile => tile.Value == '@').Key;
        foreach (var direction in directions)
        {
            var nextEmptySpace = FindValidEmptySpace(direction, robotPosition);
            if (nextEmptySpace != null)
            {
                Tiles[nextEmptySpace.Value] = 'O';
                Tiles[robotPosition] = '.';
                robotPosition = direction switch
                {
                    Direction.Up => robotPosition with { Y = robotPosition.Y - 1 },
                    Direction.Down => robotPosition with { Y = robotPosition.Y + 1 },
                    Direction.Left => robotPosition with { X = robotPosition.X - 1 },
                    Direction.Right => robotPosition with { X = robotPosition.X + 1 },
                };
                Tiles[robotPosition] = '@';
            }
        }
        
        PrintTiles();
        
        Console.WriteLine();
        
        foreach (var direction in directions)
        {
            robotPosition = ScaledTiles.First(tile => tile.Value == '@').Key;
            AttemptMoveRobot(robotPosition, direction);
            if (ScaledTiles.Count(tile => tile.Value == '@') > 1)
            {
                ScaledTiles[robotPosition] = '.';
            }
        }

        PrintScaledTiles();

        var partOneTotal = Tiles
            .Where(tile => tile.Value == 'O')
            .Sum(tile => tile.Key.X + tile.Key.Y * 100L);
        
        var partTwoTotal = ScaledTiles
            .Where(tile => tile.Value == '[')
            .Sum(tile => tile.Key.X + tile.Key.Y * 100L);

        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static void AttemptMoveRobot(Coordinate robotPosition, Direction direction)
    {
        List<Coordinate> tiles;
        switch (direction)
        {
            case Direction.Up:
                tiles = GetTilesToPushVertical(robotPosition, true);
                AttemptMoveTilesVertical(tiles, true);
                break;
            case Direction.Down:
                tiles = GetTilesToPushVertical(robotPosition, false);
                AttemptMoveTilesVertical(tiles, false);
                break;
            case Direction.Left:
                tiles = GetTilesToPushHorizontal(robotPosition, true);
                AttemptMoveTilesHorizontal(tiles, true);
                break;
            case Direction.Right:
                tiles = GetTilesToPushHorizontal(robotPosition, false);
                AttemptMoveTilesHorizontal(tiles, false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    
    private static void AttemptMoveTilesHorizontal(List<Coordinate> tiles, bool isLeft)
    {
        var movement = isLeft ? -1 : 1;
        
        if (tiles.All(tile => ScaledTiles[tile with { X = tile.X + movement }] != '#'))
        {
            var cloneDict = ScaledTiles.ToDictionary(x => x.Key, x => x.Value);
            foreach (var tile in tiles)
            {
                cloneDict[tile with { X = tile.X + movement }] = ScaledTiles[tile];
            }
            
            ScaledTiles = cloneDict;
        }
    }

    private static void AttemptMoveTilesVertical(List<Coordinate> tiles, bool isUp)
    {
        var movement = isUp ? -1 : 1;
        
        if (tiles.All(tile => ScaledTiles[tile with { Y = tile.Y + movement }] != '#'))
        {
            var cloneDict = ScaledTiles.ToDictionary(x => x.Key, x => x.Value);
            foreach (var tile in isUp ? tiles.OrderBy(tile => tile.Y) : tiles.OrderByDescending(tile => tile.Y) )
            {
                cloneDict[tile with { Y = tile.Y + movement }] = ScaledTiles[tile];
                cloneDict[tile] = '.';
            }
            
            ScaledTiles = cloneDict;
        }
    }
    

    private static List<Coordinate> GetTilesToPushVertical(Coordinate position, bool isUp)
    {
        var tiles = new List<Coordinate> { position };
        
        var movement = isUp ? -1 : 1;
        var movePosition = position with { Y = position.Y + movement };

        if (ScaledTiles[movePosition] == '[')
        {
            var leftTileUp = GetTilesToPushVertical(movePosition, isUp);
            var rightTileUp = GetTilesToPushVertical(movePosition with { X = position.X + 1 }, isUp);

            tiles.AddRange(leftTileUp);
            tiles.AddRange(rightTileUp);
        }
        
        if (ScaledTiles[movePosition] == ']')
        {
            var leftTileUp = GetTilesToPushVertical(movePosition with { X = position.X - 1 }, isUp);
            var rightTileUp = GetTilesToPushVertical(movePosition, isUp);
            
            tiles.AddRange(leftTileUp);
            tiles.AddRange(rightTileUp);
        }

        return tiles.Distinct().ToList();
    }
    
    private static List<Coordinate> GetTilesToPushHorizontal(Coordinate position, bool isLeft)
    {
        var tiles = new List<Coordinate> { position };
        
        var movement = isLeft ? -1 : 1;
        var movePosition = position with { X = position.X + movement };

        if (ScaledTiles[movePosition] != '.' && ScaledTiles[movePosition] != '#')
        {
            var leftTileUp = GetTilesToPushHorizontal(movePosition, isLeft);
            tiles.AddRange(leftTileUp);
        }
        
        return tiles.Distinct().ToList();
    }
    
    private static Coordinate? FindValidEmptySpace(Direction direction, Coordinate robotPosition)
    {
        var movePosition = robotPosition;

        while (Tiles.TryGetValue(movePosition, out var tile) && (tile != '#' && tile != '.'))
        {
            movePosition = direction switch
            {
                Direction.Up => movePosition with { Y = movePosition.Y - 1 },
                Direction.Down => movePosition with { Y = movePosition.Y + 1 },
                Direction.Left => movePosition with { X = movePosition.X - 1 },
                Direction.Right => movePosition with { X = movePosition.X + 1 },
            };
        }
        
        return Tiles[movePosition] == '.' ? movePosition : null;
    }
    
    private static void PrintTiles()
    {
        var stringBuilder = new StringBuilder();
        for (var y = 0; y < YSize; y++)
        {
            for (var x = 0; x < XSize; x++)
            {

                stringBuilder.Append(Tiles[new Coordinate(x, y)]);
            }
            stringBuilder.AppendLine();
        }

        Console.WriteLine(stringBuilder.ToString());
    }
    
    private static void PrintScaledTiles()
    {
        var stringBuilder = new StringBuilder();
        for (var y = 0; y < YSize; y++)
        {
            for (var x = 0; x < 2 * XSize; x++)
            {

                stringBuilder.Append(ScaledTiles[new Coordinate(x, y)]);
            }
            stringBuilder.AppendLine();
        }

        Console.WriteLine(stringBuilder.ToString());
    }
}
