using System.Text;

namespace advent_of_code_2024;

public static class Day07
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);

        var equations = new List<(long target, List<long> values)>();

        foreach (var line in input)
        {
            var split = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var target = long.Parse(split[0]);
            var values = new List<long>(split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));
            
            equations.Add((target, values));
        }

        var partOneTotal = GetTotal(equations, 2);
        var partTwoTotal = GetTotal(equations, 3);

        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }
    
    private static long GetTotal(List<(long target, List<long> values)> equations, int numberBase)
    {
        var total = 0L;

        var chars = Enumerable.Range(0, numberBase)
            .Select(x => x.ToString().ToCharArray()[0])
            .ToArray();

        foreach (var equation in equations)
        {
            for (var i = 0; i < Math.Pow(numberBase, equation.values.Count - 1); i++)
            {
                var equationResult = equation.values[0];
                
                for (var j = 1; j < equation.values.Count; j++)
                {
                    var value = equation.values[j];

                    // Ditch any performance benefit for part 2 because base 3 mask is string garbage brrrrr
                    var binaryString = LongToStringWithCharBase(i, chars);

                    switch (binaryString[^j])
                    {
                        case '0':
                            equationResult += value;
                            break;
                        case '1':
                            equationResult *= value;
                            break;
                        case '2':
                            equationResult = long.Parse(equationResult.ToString() + value.ToString());
                            break;
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            throw new NotImplementedException();
                    }
                }
                
                if (equationResult == equation.target)
                {
                    total += equationResult;
                    break;
                }
            }
        }

        return total;
    }

    private static readonly StringBuilder _stringBuilder = new();
    
    // Praise stack exchange for base
    private static string LongToStringWithCharBase(long value, char[] baseChars)
    {
        _stringBuilder.Clear();
        var targetBase = baseChars.Length;

        do
        {
            _stringBuilder.Insert(0, baseChars[value % targetBase]);
            value /= targetBase;
        } 
        while (value > 0);

        // I can't be bothered to deal with index out of range exceptions
        _stringBuilder.Insert(0, "0000000000000000000000000");
        
        return _stringBuilder.ToString();
    }
}
