using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Details1
{
    public struct Point
    {
        public string x;
        public string y;

        public Point(string x, string y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"x = {x}; y = {y}";
        }
    }

    class Graph
    {
        public List<List<int>> graph { get; private set; }
        public List<int> graphPowers { get; private set; }
        public Graph CreateGraph()
        {
            (graph, graphPowers) = GetGraphAndNodes();
            return this;
        }
        
        private static (List<List<int>>, List<int>) GetGraphAndNodes()
        {
            var graph = new List<List<int>>();
            var nodePowers = new List<int>();
            var n = int.Parse(Console.ReadLine());
            for (var i = 0; i < n; i++)
            {
                var line = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
                nodePowers.Add(line[0]);
                graph.Add(new List<int>());
                for (var j = 0; j < line[0]; j++)
                {
                    graph[i].Add(line[j+1]-1);
                }
            }

            return (graph, nodePowers);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < graph.Count; i++)
            {
                builder.AppendLine($"Node {i+1}: linked nodes:{string.Join(' ', graph[i])}  NodePower{graphPowers[i]}");
            }

            return builder.ToString();
        }
    }
    

    public static void Main1()
    {
        var graph = new Graph().CreateGraph();
        var orderSorted = GetSortedOrder(graph.graph, graph.graphPowers);
        orderSorted.Reverse();
        var maybeColours = new int[6];
        for (var i = 0; i < 6; i++)
        {
            maybeColours[i] = i + 1;
        }
        var nodesColours = new Dictionary<int, int>();
        var _ = graph.graph.Select((_, index) => nodesColours[index] = 0).ToArray();

        foreach (var node in orderSorted)
        {
            var neighborsColours = graph.graph[node].Select(n => nodesColours[n]).ToArray(); 
            var nodeColour = maybeColours.Where(color => !neighborsColours.Contains(color)).Min();
            nodesColours[node] = nodeColour;
        }
        Console.WriteLine(string.Join('\n', nodesColours.Values));
    }
    
    
    private static List<HashSet<int>> Sort(List<List<int>> graph, List<int> power)
    {
        var sorted = new List<HashSet<int>>();
        for (var i = 0; i < graph.Count; i++)
            sorted.Add(new HashSet<int>());
        for (var i = 0; i < graph.Count; i++)
            sorted[power[i]].Add(i);
        return sorted;
    }

    private static int GetNextMinPower(int minPower, List<HashSet<int>> sorted)
    {
        while (sorted[minPower].Count == 0)
        {
            minPower++;
        }

        return minPower;
    }

    private static List<int> GetSortedOrder(List<List<int>> graph, List<int> power)
    {
        var sorted = Sort(graph, power);
        var order = new List<int>();
        var minPower = power.Min();
        foreach (var t in graph)
        {
            minPower = GetNextMinPower(minPower, sorted);
            var v = sorted[minPower].First();
            order.Add(v);
            sorted[power[v]].Remove(v);
            power[v] = int.MaxValue;
            for (var j = 0; j < graph[v].Count; j++)
            {
                var w = graph[v][j];
                if (power[w] == int.MaxValue || !sorted[power[w]].Contains(w)) continue;
                sorted[power[w]].Remove(w);
                power[w]--;
                if (power[w] < minPower)
                {
                    minPower = power[w];
                }
                sorted[power[w]].Add(w);
            }
        }
        return order;
    }
}



