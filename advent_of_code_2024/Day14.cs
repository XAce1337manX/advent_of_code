using System.Text;
using System.Text.RegularExpressions;

namespace advent_of_code_2024;

public static class Day14
{
    private class Robot
    {
        public (int x, int y) Position { get; set; }
        public (int x, int y) Velocity { get; set; }
    }
    
    private const int XSize = 101;
    private const int YSize = 103;
    private const int Seconds = 100;
    
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        
        var regex = new Regex(@"p=(-*\d+),(-*\d+) v=(-*\d+),(-*\d+)$");
        
        var robots = new List<Robot>();
        foreach (var line in input)
        {
            var match = regex.Match(line);
            var robot = new Robot
            {
                Position = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                Velocity = (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
            };
            robots.Add(robot);
        }

        var partOneTotal = 0;
        var partTwoTotal = 0;
        
        var i = 1;
        while (i < 10000)
        {
            MoveRobots(robots);
            
            // Part 1
            if (i == 100)
            {
                // Calculate quadrant boundary
                var xQuadrantLine = XSize / 2;
                var yQuadrantLine = YSize / 2;

                partOneTotal = robots.Count(robot => robot.Position.x < xQuadrantLine && robot.Position.y < yQuadrantLine)
                               * robots.Count(robot => robot.Position.x < xQuadrantLine && robot.Position.y > yQuadrantLine)
                               * robots.Count(robot => robot.Position.x > xQuadrantLine && robot.Position.y < yQuadrantLine)
                               * robots.Count(robot => robot.Position.x > xQuadrantLine && robot.Position.y > yQuadrantLine);
            }
         
            // Part 2
            var robotPositions = robots.Select(robot => robot.Position).ToList();
            var averageX = robotPositions.Average(pos => pos.x);
            var averageY = robotPositions.Average(pos => pos.y);
            var standardDeviationX = Math.Sqrt(robotPositions.Average(pos => Math.Pow(pos.x - averageX, 2)));
            var standardDeviationY = Math.Sqrt(robotPositions.Average(pos => Math.Pow(pos.y - averageY, 2)));

            var combinedStandardDeviation = standardDeviationX * standardDeviationY;

            Console.WriteLine($"{i} => {combinedStandardDeviation}");
            if (combinedStandardDeviation < 500) // Hard coded threshold that worked
            {
                PrintRobotPositions(robotPositions);
                partTwoTotal = i;
                break;
            }
            
            i++;
        }

        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static void MoveRobots(List<Robot> robots)
    {
        foreach (var robot in robots)
        {
            robot.Position = (
                ((robot.Position.x + robot.Velocity.x * 1) % XSize + XSize) % XSize,
                ((robot.Position.y + robot.Velocity.y * 1) % YSize + YSize) % YSize
            );
        }
    }

    private static void PrintRobotPositions(List<(int x, int y)> robotPositions)
    {
        var stringBuilder = new StringBuilder();
        for (var y = 0; y < YSize; y++)
        {
            for (var x = 0; x < XSize; x++)
            {
                if (robotPositions.Contains((x, y)))
                {
                    stringBuilder.Append('\u2593');
                }
                else
                {
                    stringBuilder.Append('\u2591');
                }
            }
        
            stringBuilder.AppendLine();
        }
        
        Console.WriteLine(stringBuilder.ToString());
    }
}
