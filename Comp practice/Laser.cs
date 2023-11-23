using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

class Laser
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

    public static void Main2()
    {
        var dict = new Dictionary<(string, string), HashSet<(string, string)>>();
        var n = int.Parse(Console.ReadLine());
        for (var i = 0; i < n; i++)
        {
            var ni = int.Parse(Console.ReadLine()) * 2;
            var cut = Console.ReadLine().Split(' ');
            for (var j = 0; j < cut.Length - 2; j += 2)
            {
                if (dict.ContainsKey((cut[j], cut[j + 1])))
                {
                    dict[(cut[j], cut[j + 1])].Add((cut[j + 2], cut[j + 3]));
                    if (!dict.ContainsKey((cut[j + 2], cut[j + 3])))
                        dict[(cut[j + 2], cut[j + 3])] = new HashSet<(string, string)>();
                    dict[(cut[j + 2], cut[j + 3])].Add((cut[j], cut[j + 1]));
                }
                else
                {
                    dict[(cut[j], cut[j + 1])] = new HashSet<(string, string)> { (cut[j + 2], cut[j + 3]) };
                    if (!dict.ContainsKey((cut[j + 2], cut[j + 3])))
                        dict[(cut[j + 2], cut[j + 3])] = new HashSet<(string, string)>();
                    dict[(cut[j + 2], cut[j + 3])].Add((cut[j], cut[j + 1]));
                }
            }
        }

        var components = FindComponents(dict);
        var sum = components.Count - 1;
        foreach (var component in components)
        {
            var degrees = FindDegrees(component);
            var oddDegrees = degrees.Values.Count(x => x % 2 != 0);
            if (oddDegrees > 2)
            {
                sum += (oddDegrees - 2) / 2;
            }
        }

        Console.WriteLine(sum);
    }

    private static void PrintDict(Dictionary<(string, string), HashSet<(string, string)>> dict)
    {
        foreach (var kvp in dict)
        {
            Console.WriteLine($"dict[{kvp.Key.ToString()}] = {kvp.Value.Count}");
        }
    }

    private static List<Dictionary<(string, string), HashSet<(string, string)>>> FindComponents(
        Dictionary<(string, string), HashSet<(string, string)>> dict)
    {
        var components = new List<Dictionary<(string, string), HashSet<(string, string)>>>();
        var visited = new HashSet<(string, string)>();

        foreach (var startPoint in dict.Keys)
        {
            if (!visited.Contains(startPoint))
            {
                var component = new Dictionary<(string, string), HashSet<(string, string)>>();
                DFS(dict, startPoint, visited, component);
                components.Add(component);
            }
        }

        return components;
    }

    private static void DFS(Dictionary<(string, string), HashSet<(string, string)>> dict, (string, string) currentPoint,
        HashSet<(string, string)> visited, Dictionary<(string, string), HashSet<(string, string)>> component)
    {
        visited.Add(currentPoint);
        component.Add(currentPoint, dict[currentPoint]);

        foreach (var neighbor in dict[currentPoint])
        {
            if (!visited.Contains(neighbor))
            {
                DFS(dict, neighbor, visited, component);
            }
        }
    }

    private static Dictionary<(string, string), int> FindDegrees(
        Dictionary<(string, string), HashSet<(string, string)>> dict)
    {
        var degrees = new Dictionary<(string, string), int>();

        foreach (var vertex in dict.Keys)
        {
            var degree = dict[vertex].Count;
            degrees[vertex] = degree;
        }

        return degrees;
    }
}