using System;
using System.Numerics;

class CountPaths
{
    static void Main4()
    {
        var graphMatrix = new  long[,] { { 0, 1 }, { 1, 1 } };
        var n = long.Parse(Console.ReadLine());
        var p = 1;
        var answer1 = BinPow(graphMatrix, n);
        Console.WriteLine(answer1[0,1]);
    }

    static long[,] CalculateMatrixProduct(long[,] matrixA,
        long[,] matrixB)
    {
        var rowsA = matrixA.GetLength(0);
        var columnsA = matrixA.GetLength(1);
        var columnsB = matrixB.GetLength(1);

        var result = new long[rowsA, columnsB];

        for (var i = 0; i < rowsA; i++)
        {
            for (var j = 0; j < columnsB; j++)
            {
                for (var k = 0; k < columnsA; k++)
                {
                    result[i, j] = (matrixA[i, k] * matrixB[k, j] + result[i, j]);
                }
            }
        }

        return result;
    }

    static void PrintMatrix(long[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var columns = matrix.GetLength(1);
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }

            Console.WriteLine();
        }
    }

    static long[,] BinPow(long[,] a, long pow)
    {
        if (pow == 0)
            return new long[,] {{1, 0},{0, 1} };
        if (pow % 2 == 1)
            return CalculateMatrixProduct(BinPow(a, pow - 1), a);
        var b = BinPow(a, pow / 2);
        return CalculateMatrixProduct(b, b);
    } 
}