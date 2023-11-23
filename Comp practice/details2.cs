using System;
using System.Numerics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Details2
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

    public class Graph
    {
        public List<List<int>> graph { get; private set; }
        public List<int> graphPowers { get; private set; }

        public Graph CreateGraph()
        {
            (graph, graphPowers) = GetGraphAndNodesPowers();
            return this;
        }

        private static (List<List<int>>, List<int>) GetGraphAndNodesPowers()
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
                    graph[i].Add(line[j + 1]-1);
                }
            }

            return (graph, nodePowers);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < graph.Count; i++)
            {
                builder.AppendLine(
                    $"Node {i + 1}: linked nodes:{string.Join(' ', graph[i])}  NodePower{graphPowers[i]}");
            }

            return builder.ToString();
        }
    }


    public static void Main()
    {
        var graph = new Graph().CreateGraph();
        var orderSorted = GetSortedOrder(graph.graph, graph.graphPowers);
        orderSorted.Reverse();
        var maybeColours = new int[5];
        for (var i = 0; i < 5; i++)
        {
            maybeColours[i] = i + 1;
        }

        var nodesColours = new Dictionary<int, int>();
        graph.graph.Select((_, index) => nodesColours[index] = 0).ToArray();

        foreach (var node in orderSorted)
        {
            foreach (var colour in maybeColours)
            {
                var hardToColour = false;
                foreach (var __ in graph.graph[node].Where(neighbour => colour == nodesColours[neighbour]))
                {
                    hardToColour = true;
                }

                if (!hardToColour)
                {
                    nodesColours[node] = colour;
                    break;
                }
            }

            if (nodesColours[node] == 0)
            {
                ColourProblemNode(node, nodesColours, graph.graph);
            }
        }

        Console.WriteLine(string.Join('\n', nodesColours.Values));
    }

    private static void ColourProblemNode(int node, Dictionary<int, int> nodesColours, List<List<int>> graph)
    {
        foreach (var neighbour in graph[node])
        {
            var neighbourColour = nodesColours[neighbour];
            foreach (var anotherNeighbour in graph[node])
            {
                if (neighbour == anotherNeighbour) continue;
                var anotherNeighbourColour = nodesColours[anotherNeighbour];
                if (anotherNeighbourColour == 0 || neighbourColour == 0 || neighbourColour==anotherNeighbourColour) continue;
                var component = GetComponentStartWithCurNodeAndFinishWithAnotherNeighbourColour(neighbour,
                    neighbourColour, anotherNeighbourColour, graph, nodesColours);
                if (!component.Contains(anotherNeighbour))
                {
                    foreach (var n0de in component)
                    {
                        if (nodesColours[n0de] == anotherNeighbourColour)
                            nodesColours[n0de] = neighbourColour;
                        else if (nodesColours[n0de] == neighbourColour)
                            nodesColours[n0de] = anotherNeighbourColour;
                    }

                    nodesColours[node] = neighbourColour;
                    return;
                }
            }
        }
    }

    private static HashSet<int> GetComponentStartWithCurNodeAndFinishWithAnotherNeighbourColour(int startNode,
        int startColour, int endColour, List<List<int>> graph, Dictionary<int, int> nodesColours)
    {
        var comp = new HashSet<int>();
        var visited = new HashSet<int>();
        var queue = new Queue<int>();
        queue.Enqueue(startNode);
        while (queue.Count != 0)
        {
            var curNode = queue.Dequeue();
            if (visited.Contains(curNode) ||
                (nodesColours[curNode] != startColour && nodesColours[curNode] != endColour))
            {
                continue;
            }

            visited.Add(curNode);
            comp.Add(curNode);
            graph[curNode].Select(x =>
            {
                queue.Enqueue(x);
                return x;
            }).ToArray();
        }

        return comp;
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
        var sortedByPowerNodes = Sort(graph, power);
        var orderToColour = new List<int>();
        var minPower = power.Min();

        for (var _ = 0; _ < graph.Count; _++)
        {
            minPower = GetNextMinPower(minPower, sortedByPowerNodes);

            var nodeWithMinPower = sortedByPowerNodes[minPower].First();
            orderToColour.Add(nodeWithMinPower);
            sortedByPowerNodes[power[nodeWithMinPower]].Remove(nodeWithMinPower);
            power[nodeWithMinPower] = int.MaxValue;
            for (var j = 0; j < graph[nodeWithMinPower].Count; j++)
            {
                var incidentWithMinPowerNode = graph[nodeWithMinPower][j];
                if (power[incidentWithMinPowerNode] == int.MaxValue ||
                    !sortedByPowerNodes[power[incidentWithMinPowerNode]].Contains(incidentWithMinPowerNode)) continue;

                sortedByPowerNodes[power[incidentWithMinPowerNode]].Remove(incidentWithMinPowerNode);
                power[incidentWithMinPowerNode]--;
                if (power[incidentWithMinPowerNode] < minPower)
                {
                    minPower = power[incidentWithMinPowerNode];
                }

                sortedByPowerNodes[power[incidentWithMinPowerNode]].Add(incidentWithMinPowerNode);
            }
        }

        return orderToColour;
    }
}