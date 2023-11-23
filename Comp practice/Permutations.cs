using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

class Permutations
{
    static void Main3()
    {
        var n = int.Parse(Console.ReadLine());
        var t = BigInteger.Parse(Console.ReadLine());
        var permut = GetPermutation(n, t);
        var cycles = FindCycles(permut);
            // foreach (var cycle in cycles)
            // {
            //     foreach (var item in cycle)
            //     {
            //         Console.WriteLine(item + ' ');
            //     }
            //     Console.WriteLine();
            // }
        Console.WriteLine(cycles);
    }

    static int[] GetPermutation(int n, BigInteger t)
    {
        var nums = Enumerable.Range(1, n).ToList();
        var result = new List<int>();

        for (var i = n; i > 0; i--)
        {
            var fact = Factorial(i - 1);
            var index = (t - 1) / fact;
            result.Add(nums[(int)index]);
            nums.RemoveAt((int)index);
            t -= index * fact;
        }

        return result.ToArray();
    }
    
    static BigInteger Factorial(BigInteger n)
    {
        if (n == 0)
            return 1;
        else
            return n * Factorial(n - 1);
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

    static void MakeCyclicShift(int[] arr)
    {
        int temp = arr[arr.Length - 1];

        for (int i = arr.Length - 1; i > 0; i--)
        {
            arr[i] = arr[i - 1];
        }

        arr[0] = temp;
    }
    static string GetCanonicView(int[] arr)
    {
        if (arr.Length == 1) return $"( {arr[0]} )";
        var max = arr.Max();
        while(arr[^1]!=max)
            MakeCyclicShift(arr);
        MakeCyclicShift(arr);
        return $"( {string.Join(' ', arr)} )";
    }
    static string FindCycles(int[] arr)
    {
        var dict = new Dictionary<int, int>();
        var result = new List<int[]>();
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
            result.Add(cycle.ToArray());
            visited.UnionWith(cycle);
        }

        return string.Join(' ', result.OrderBy(x=>x.Max()).Select(GetCanonicView));
    }
}