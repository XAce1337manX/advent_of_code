namespace advent_of_code_2024;

public static class Day23
{
    private class Vertex
    {
        public required string Name;
        public readonly HashSet<Vertex> Neighbours = [];
    }
    
    public static void Solve(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);

        var graph = new Dictionary<string, Vertex>();
        
        foreach (var line in lines)
        {
            var split = line.Split("-");

            graph.TryAdd(split[0], new Vertex{ Name = split[0] });
            graph.TryAdd(split[1], new Vertex{ Name = split[1] });

            graph[split[0]].Neighbours.Add(graph[split[1]]);
            graph[split[1]].Neighbours.Add(graph[split[0]]);
        }

        var triCycles = new List<HashSet<Vertex>>();
        
        foreach (var vertex in graph.Values)
        {
            foreach (var neighbour in vertex.Neighbours)
            {
                foreach (var deepNeighbour in neighbour.Neighbours)
                {
                    if (deepNeighbour.Neighbours.Contains(vertex))
                    {
                        if (!triCycles.Any(tc =>
                                tc.Contains(vertex) && tc.Contains(neighbour) && tc.Contains(deepNeighbour)))
                        {
                            triCycles.Add([vertex, neighbour, deepNeighbour]);
                        }
                    }
                }
            }
        }
        
        foreach (var cycle in triCycles)
        {
            Console.WriteLine(string.Join(",", cycle.Select(x => x.Name)));
        }
        
        var partOneTotal = triCycles.Count(x => x.Any(v => v.Name.StartsWith($"t")));

        var maximalCliques = new List<HashSet<Vertex>>();
        foreach (var vertex in graph.Values)
        {
            var clique = new HashSet<Vertex> { vertex };

            foreach (var neighbour in vertex.Neighbours)
            {
                if (clique.All(c => neighbour.Neighbours.Contains(c)))
                {
                    clique.Add(neighbour);
                }
            }
            
            maximalCliques.Add(clique);
        }

        var maximumClique = maximalCliques.MaxBy(c => c.Count);
        var partTwoTotal = string.Join(',', maximumClique!.Select(v => v.Name).OrderBy(n => n));
        
        Console.WriteLine($"Part One: {partOneTotal}");
        Console.WriteLine($"Part Two: {partTwoTotal}");
    }
}
