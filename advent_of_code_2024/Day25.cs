using System.Linq;

namespace advent_of_code_2024;

public static class Day25
{
 
    public static void Solve(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile).ToList();

        var height = lines.FindIndex(string.IsNullOrWhiteSpace);

        var keys = new List<List<int>>();
        var locks = new List<List<int>>();
        
        for (var i = 0; i < lines.Count; i += 1 + height)
        {
            var isKey = lines[i] == ".....";

            if (isKey)
            {
                var key = lines[i..(i + height)];
                var keyHeights = new List<int>
                {
                    height - 1 - key.FindIndex(x => x[0] == '#'),
                    height - 1 - key.FindIndex(x => x[1] == '#'),
                    height - 1 - key.FindIndex(x => x[2] == '#'),
                    height - 1 - key.FindIndex(x => x[3] == '#'),
                    height - 1 - key.FindIndex(x => x[4] == '#')
                };
                keys.Add(keyHeights);
            }
            else
            {
                
                var @lock = lines[i..(i + height)];
                var lockHeights = new List<int>
                {
                    - 1 + @lock.FindIndex(x => x[0] == '.'),
                    - 1 + @lock.FindIndex(x => x[1] == '.'),
                    - 1 + @lock.FindIndex(x => x[2] == '.'),
                    - 1 + @lock.FindIndex(x => x[3] == '.'),
                    - 1 + @lock.FindIndex(x => x[4] == '.')
                };
                locks.Add(lockHeights);
            }
        }

        // foreach (var key in keys)
        // {
        //     Console.WriteLine($"Key: {string.Join(',', key)}");
        // }
        // foreach (var lock in locks)
        // {
        //     Console.WriteLine($"Lock: {string.Join(',', @lock)}");
        // }

        var partOneTotal = 0L;

        foreach (var @lock in locks)
        {
            foreach (var key in keys)
            {
                if (height - 2 - @lock[0] < key[0]) continue;
                if (height - 2 - @lock[1] < key[1]) continue;
                if (height - 2 - @lock[2] < key[2]) continue;
                if (height - 2 - @lock[3] < key[3]) continue;
                if (height - 2 - @lock[4] < key[4]) continue;
                partOneTotal += 1;
            }
        }
        
        Console.WriteLine($"Part One: {partOneTotal}");
    }
}
