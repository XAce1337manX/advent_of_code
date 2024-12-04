namespace advent_of_code_2024;

public static class Day04
{
    public static void Solve(string inputFile)
    {        
        var input = File.ReadAllLines(inputFile);

        // Input parsing
        var xTiles = new List<(int row, int col)>();
        var mTiles = new List<(int row, int col)>();
        var aTiles = new List<(int row, int col)>();
        var sTiles = new List<(int row, int col)>();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                var tile = input[row][col];
                switch (tile)
                {
                    case 'X':
                        xTiles.Add((row, col));
                        break;
                    case 'M':
                        mTiles.Add((row, col));
                        break;
                    case 'A':
                        aTiles.Add((row, col));
                        break;
                    case 'S':
                        sTiles.Add((row, col));
                        break;
                }
            }
        }

        // Part 1
        var partOneTotal = 0;

        var directionFactors = new List<(int rowFactor, int colFactor)>
        {
            (1, 1),
            (1, 0),
            (1, -1),
            (0, -1),
            // (0, 0),
            (0, 1),
            (-1, 1),
            (-1, 0),
            (-1, -1),
        };

        foreach (var xTile in xTiles)
        {
            foreach (var direction in directionFactors)
            {
                if (mTiles.Contains((xTile.row + 1 * direction.rowFactor, xTile.col + 1 * direction.colFactor))
                    && aTiles.Contains((xTile.row + 2 * direction.rowFactor, xTile.col + 2 * direction.colFactor))
                    && sTiles.Contains((xTile.row + 3 * direction.rowFactor, xTile.col + 3 * direction.colFactor)))
                {
                    partOneTotal++;
                }
            }
        }
        
        // Part 2
        var partTwoTotal = 0;

        var directions = new List<(int row1, int col1, int row2, int col2)>
        {
            (1, 1, -1, 1),
            (1, 1, 1, -1),
            (-1, -1, -1, 1),
            (-1, -1, 1, -1),
        };

        foreach (var aTile in aTiles)
        {
            foreach (var direction in directions)
            {
                if (sTiles.Contains((aTile.row + direction.row1, aTile.col + direction.col1))
                    && sTiles.Contains((aTile.row + direction.row2, aTile.col + direction.col2))
                    && mTiles.Contains((aTile.row - direction.row1, aTile.col - direction.col1)) // invert s-tile direction
                    && mTiles.Contains((aTile.row - direction.row2, aTile.col - direction.col2))
                    )
                {
                    partTwoTotal++;
                }
            }
        }

        Console.WriteLine(partOneTotal);
        Console.WriteLine(partTwoTotal);
    }
}
