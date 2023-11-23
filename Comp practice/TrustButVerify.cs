using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

class TrustButVerify
{
    static void Main3()
    {
        var n = Console.ReadLine();
        var bwt = Console.ReadLine();
        var blocks = FindBlocks(bwt);
        var gcd = FindGcd(blocks.ToArray());
        var permut = GetCanonicalPermutation(bwt);
        var cycles = FindCycles(permut);
        Console.WriteLine(gcd == cycles ? "Yes" : "No");
    }

    static List<int> FindBlocks(string input)
    {
        var blocks = new List<int>();
        var len = input.Length;
        var curLen = 1;
        for (var i = 1; i < len; i++)
        {
            if (i == len - 1)
            {
                blocks.Add(curLen);
                break;
            }

            if (input[i - 1] == input[i])
            {
                curLen++;
                continue;
            }

            blocks.Add(curLen);
            curLen = 1;
        }

        return blocks;
    }

    static int GCD(int a, int b)
    {
        while (b != 0)
        {
            var remainder = a % b;
            a = b;
            b = remainder;
        }

        return a;
    }

    static int FindGcd(int[] numbers)
    {
        if (numbers.Length == 0)
            throw new ArgumentException("Массив не должен быть пустым.");

        var result = numbers[0];
        for (var i = 1; i < numbers.Length; i++)
        {
            result = GCD(result, numbers[i]);
            if (result == 1)
                return 1; // Если найденный НОД уже равен 1, то дальнейший поиск не имеет смысла
        }

        return result;
    }

    static int[] GetCanonicalPermutation(string input)
    {
        var arr = input.Select((t, i) => new Tuple<char, int>(t, i + 1)).OrderBy(t => t.Item1)
            .Select(t => t.Item2).ToArray();
        // foreach (var item in tupleArr)
        // {
        //     Console.Write(item);
        // }

        return arr;
    }

    static void GetDFS(Dictionary<int, int> graph, int node, HashSet<int> visited)
    {
        visited.Add(node);
        if (!graph.ContainsKey(node)) return;
        var nextNode = graph[node];
        if (!visited.Contains(nextNode))
        {
            GetDFS(graph, nextNode, visited);
        }
    }

    static HashSet<int> GetBFS(Dictionary<int, int> graph, int startNode)
    {
        var queue = new Queue<int>();
        var visited = new HashSet<int>();
        queue.Enqueue(startNode);
        while (queue.Count > 0)
        {
            var curNode = queue.Dequeue();
            if (visited.Contains(curNode)) continue;
            visited.Add(curNode);
            if (!graph.ContainsKey(curNode)) continue;
            var neighbour = graph[curNode];
            queue.Enqueue(neighbour);
        }

        return visited;
    }

    static int FindCycles(int[] arr)
    {
        var dict = new Dictionary<int, int>();
        var result = new List<HashSet<int>>();
        var visited = new HashSet<int>();
        for (var i = 1; i <= arr.Length; i++)
        {
            dict[i] = arr[i - 1];
        }

        foreach (var node in dict.Keys)
        {
            if (visited.Contains(node)) continue;
            var cycle = new HashSet<int>();
            GetDFS(dict, node, cycle);
            result.Add(cycle);
            visited.UnionWith(cycle);
        }

        return result.Count;
    }
}