using System.Text;

namespace advent_of_code_2024;

public static class Day17
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        var registerA = long.Parse(input[0].Split(' ').Last());
        var registerB = long.Parse(input[1].Split(' ').Last());
        var registerC = long.Parse(input[2].Split(' ').Last());
        var program = input[4].Split(' ').Last().Split(',').Select(long.Parse).ToList();

        var partOneTotal = string.Join(',', GetOutput(program, registerA, registerB, registerC));
        
        var changingBaseIndex = 0;
        var currentStringResult = "";
        var stringBuilder = new StringBuilder();
        var invalidBaseIndexValues = new HashSet<(int @index, long value)>();
        
        while (changingBaseIndex < program.Count)
        {
            var proceedToNextBase = false;
            
            foreach (var i in Enumerable.Range(0, 8))
            {
                stringBuilder.Clear();
                stringBuilder.Append(currentStringResult);
                stringBuilder.Append(i);

                while (stringBuilder.Length < program.Count)
                {
                    stringBuilder.Append('0');
                }
                
                registerA = StringToBase8(stringBuilder.ToString());
                var output = GetOutput(program, registerA, registerB, registerC);
                if (output.Count != program.Count)
                {
                    continue;
                }

                if (output[^(changingBaseIndex + 1)] == program[^(changingBaseIndex + 1)]
                    && !invalidBaseIndexValues.Contains((changingBaseIndex, i)))
                {
                    currentStringResult += i.ToString();
                    proceedToNextBase = true;
                    Console.WriteLine($"Search: {currentStringResult}");
                    break;
                }
            }

            if (proceedToNextBase)
            {
                invalidBaseIndexValues.RemoveWhere(x => x.index > changingBaseIndex);
                changingBaseIndex++;
            }
            else
            {
                invalidBaseIndexValues.Add((index: changingBaseIndex - 1, value: long.Parse(currentStringResult[^1].ToString())));
                currentStringResult = currentStringResult.Remove(currentStringResult.Length - 1);
                changingBaseIndex--;
            }
        }

        var partTwoTotal = StringToBase8(currentStringResult);
        
        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }

    private static long StringToBase8(string input)
    {
        long value = 0;
        
        var length = input.Length;
        for (var i = 0; i < length; i++)
        {
            value += (long)Math.Pow(8, i) * long.Parse(input[^(i + 1)].ToString());
        }

        return value;
    }

    private static List<long> GetOutput(List<long> program, long registerA, long registerB, long registerC)
    {
        var output = new List<long>();
        
        var pointer = 0;
        while (pointer < program.Count - 1)
        {
            var opcode = program[pointer];
            var operand = program[pointer + 1];
            
            var literalOperand = operand;
            var comboOperand = (operand) switch
            {
                0 => operand,
                1 => operand,
                2 => operand,
                3 => operand,
                4 => registerA,
                5 => registerB,
                6 => registerC,
                _ => throw new Exception()
            };

            long numerator;
            long denominator;
            
            switch (opcode)
            {
                case 0:
                    numerator = registerA;
                    denominator = (long)Math.Pow(2, comboOperand);
                    registerA = (numerator / denominator);
                    break;
                
                case 1:
                    registerB ^= literalOperand;
                    break;
                
                case 2:
                    registerB = ((comboOperand % 8) + 8) % 8;
                    break;
                
                case 3:
                    if (registerA != 0)
                    {
                        pointer = (int)literalOperand;
                        continue;
                    }
                    break;
                
                case 4:
                    registerB ^= registerC;
                    break;
                
                case 5:
                    output.Add(comboOperand % 8);
                    break;
                
                case 6:
                    numerator = registerA;
                    denominator = (int)Math.Pow(2, comboOperand);
                    registerB = numerator / denominator;
                    break;
                
                case 7:
                    numerator = registerA;
                    denominator = (int)Math.Pow(2, comboOperand);
                    registerC = numerator / denominator;
                    break;
                
            }
            pointer += 2;
        }
        
        return output;
    }
}
