using System.Text;

namespace advent_of_code_2024;

public static class Day17
{
    public static void Solve(string inputFile)
    {
        var input = File.ReadAllLines(inputFile);
        var registerA = int.Parse(input[0].Split(' ').Last());
        var registerB = int.Parse(input[1].Split(' ').Last());
        var registerC = int.Parse(input[2].Split(' ').Last());
        var program = input[4].Split(' ').Last().Split(',').Select(int.Parse).ToList();
        
        var output = new List<int>();
        
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
            
            int numerator, denominator;
            
            switch (opcode)
            {
                case 0:
                    numerator = registerA;
                    denominator = (int)Math.Pow(2, comboOperand);
                    registerA = numerator / denominator;
                    break;
                
                case 1:
                    registerB ^= literalOperand;
                    break;
                
                case 2:
                    registerB = comboOperand % 8;
                    break;
                
                case 3:
                    if (registerA != 0)
                    {
                        pointer = literalOperand;
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
        
        Console.WriteLine($"Part One: {string.Join(',', output)}");
        // Console.WriteLine($"Part Two: {partTwoTotal}");
    }
}
