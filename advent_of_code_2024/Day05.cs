namespace advent_of_code_2024;

public static class Day05
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        
        // Parsing
        var orderingBeforeRules = new Dictionary<int, HashSet<int>>();
        var orderingAfterRules = new Dictionary<int, HashSet<int>>();
        var updates = new List<List<int>>();
        
        var lineIsMapping = true;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                lineIsMapping = false;
                continue;
            }

            if (lineIsMapping)
            {
                // First part of input with rule ordering
                var split = line.Split("|", StringSplitOptions.RemoveEmptyEntries).ToList();
                var before = int.Parse(split[0]);
                var after = int.Parse(split[1]);

                if (orderingBeforeRules.ContainsKey(before))
                {
                    orderingBeforeRules[before].Add(after);
                }
                else
                {
                    orderingBeforeRules.Add(before, [after]);
                }
                
                if (orderingAfterRules.ContainsKey(after))
                {
                    orderingAfterRules[after].Add(before);
                }
                else
                {
                    orderingAfterRules.Add(after, [before]);
                }
            }
            else
            {
                // Second part of input with updates
                var update = line.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();
                
                updates.Add(update);
            }
        }


        // Part 1
        var validUpdateIndexes = new List<int>();

        for (var i = 0; i < updates.Count; i++)
        {
            var isValidIndex = true;
            var update = updates[i];
            for (var j = 0; j < update.Count; j++)
            {
                var currentNumber = update[j];
                
                if (orderingBeforeRules.TryGetValue(currentNumber, out var beforeRules))
                {
                    for (var k = j + 1; k < update.Count; k++)
                    {
                        var afterNumber = update[k];

                        if (!beforeRules.Contains(afterNumber))
                        {
                            isValidIndex = false;
                        }
                    }
                }

                if (orderingAfterRules.TryGetValue(currentNumber, out var afterRules))
                {
                    for (var k = j - 1; k >= 0; k--)
                    {
                        var beforeNumber = update[k];

                        if (!afterRules.Contains(beforeNumber))
                        {
                            isValidIndex = false;
                        }
                    }
                }
            }

            if (isValidIndex)
            {
                validUpdateIndexes.Add(i);
            }
        }

        var partOneTotal = 0;
        foreach (var validUpdateIndex in validUpdateIndexes)
        {
            var update = updates[validUpdateIndex];
            var mid = update[update.Count / 2];
            partOneTotal += mid;
        }

        // Part Two
        var partTwoTotal = 0;
        
        var invalidUpdateIndexes = Enumerable.Range(0, updates.Count).Except(validUpdateIndexes).ToList();
        foreach (var invalidUpdateIndex in invalidUpdateIndexes)
        {
            var update = updates[invalidUpdateIndex];
            var isSorted = false;
            while (!isSorted)
            {
                isSorted = true;
                
                for (var i = 0; i < update.Count; i++)
                {
                    if (!isSorted) break;
                    var currentNumber = update[i];
                    if (orderingBeforeRules.TryGetValue(currentNumber, out var beforeRules))
                    {
                        for (var j = i + 1; j < update.Count; j++)
                        {
                            var afterNumber = update[j];

                            if (!beforeRules.Contains(afterNumber))
                            {
                                isSorted = false;
                                (update[i], update[j]) = (update[j], update[i]);
                                break;
                            }
                        }
                    }
                
                    if (orderingAfterRules.TryGetValue(currentNumber, out var afterRules))
                    {
                        for (var j = i - 1; j >= 0; j--)
                        {
                            var beforeNumber = update[j];

                            if (!afterRules.Contains(beforeNumber))
                            {
                                isSorted = false;
                                (update[i], update[j]) = (update[j], update[i]);
                                break;
                            }
                        }
                    }
                }
                
            }
            
            var mid = update[update.Count / 2];
            partTwoTotal += mid;
        }
        
        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }
}
