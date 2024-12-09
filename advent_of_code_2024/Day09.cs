using System.Text;

namespace advent_of_code_2024;

public static class Day09
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllText(inputFile).Trim();

        var diskLayout = new List<int>();
        
        var isEmptySpace = false;

        var id = 0;
        
        foreach (var number in input.Select(@char => int.Parse(@char.ToString())))
        {
            if (isEmptySpace)
            {
                diskLayout.AddRange(Enumerable.Repeat(-1, number));
            }
            else
            {
                diskLayout.AddRange(Enumerable.Repeat(id, number));
                id++;
            }
            
            isEmptySpace = !isEmptySpace;
        }

        var partOneTotal = GetPartOneTotal(diskLayout);
        var partTwoTotal = GetPartTwoTotal(diskLayout);

        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static long GetPartOneTotal(List<int> diskLayout)
    {
        var compactDisk = diskLayout.ToList();
        
        var startIndex = 0;
        var endIndex = compactDisk.Count - 1;

        while (endIndex > startIndex)
        {
            if (compactDisk[startIndex] != -1)
            {
                startIndex++;
                continue;
            }

            if (compactDisk[endIndex] == -1)
            {
                endIndex--;
                continue;
            }
            
            (compactDisk[startIndex], compactDisk[endIndex]) = (compactDisk[endIndex], compactDisk[startIndex]);
            startIndex++;
            endIndex--;
        }
        
        return compactDisk
            .TakeWhile(t => t != -1)
            .Select((t, i) => i * (long)t)
            .Sum();
    }
    
    private static long GetPartTwoTotal(List<int> diskLayout)
    {
        var compactDisk = diskLayout.ToList();
        
        // This could certainly be made easier with a better data structure but whatever brute force
        // This could also definitely be made better by not relying on Linq but it's easier this way
        var highestId = compactDisk.Max();

        while (highestId > 0)
        {
            var highestIdStartIndex = compactDisk.FindIndex(x => x == highestId);
            var highestIdEndIndex = compactDisk.FindLastIndex(x => x == highestId);
            
            var highestIdLength = 1 + highestIdEndIndex - highestIdStartIndex;

            var isEmptySpaceFound = false;
            var emptySpaceIndex = 0;
            while (!isEmptySpaceFound && emptySpaceIndex <= (highestIdStartIndex - highestIdLength))
            {
                var isAllEmpty = Enumerable.Range(emptySpaceIndex, highestIdLength)
                    .Aggregate(true, (isAllEmpty, i) => isAllEmpty & compactDisk[i] == -1);

                if (isAllEmpty)
                {
                    isEmptySpaceFound = true;
                    break;
                }

                emptySpaceIndex++;
            }

            if (isEmptySpaceFound)
            {
                foreach (var i in Enumerable.Range(0, highestIdLength))
                {
                    (compactDisk[emptySpaceIndex + i], compactDisk[highestIdStartIndex + i]) = (
                        compactDisk[highestIdStartIndex + i], compactDisk[emptySpaceIndex + i]);
                }
            }
            
            highestId--;
        }
        
        return compactDisk
            .Select((t, i) => t == -1 ? 0 : i * (long)t)
            .Sum();
    }

    private static void DebugPrint(List<int> diskLayout)
    {
        var sb = new StringBuilder();
        foreach (var number in diskLayout)
        {
            if (number == -1)
            {
                sb.Append('.');
            }
            else
            {
                sb.Append(number);
            }
        }
        
        Console.WriteLine(sb.ToString());
    }
}
