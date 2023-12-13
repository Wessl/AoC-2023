using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class day13 : MonoBehaviour
{
    [TextArea] public string input;
    void Start()
    {
        var lines = input.Split("\r\n");

        List<char[,]> horizontals = new List<char[,]>();
        var rectangleDimensions = GetRectangleDimensions(lines);
        int rdIndex = 0;
        char[,] horizontal = new char[rectangleDimensions[rdIndex].height, rectangleDimensions[rdIndex].width];
        
        Debug.Log($"Original Array Dimensions: {lines.Length} x {lines[0].Length}");
        int newStartIndex = 0;
        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            if (string.IsNullOrEmpty(line))
            {
                Debug.Log("adding horizontal.");
                horizontals.Add(horizontal);
                rdIndex++;
                horizontal = new char[rectangleDimensions[rdIndex].height, rectangleDimensions[rdIndex].width]; // Start a new horizontal array
                newStartIndex = index+1;
            }
            else
            {
                for (int j = 0; j < line.Length; j++)
                {
                    horizontal[index - newStartIndex, j] = line[j];
                }
            }
        }
        if (horizontal != null)
        {
            Debug.Log("adding horizontal");
            horizontals.Add(horizontal);
        }

        var verticals = new List<char[,]>();
        foreach (var vertical in horizontals)
        {
            verticals.Add(TransposeRowsAndColumns(vertical));
        }

        AnalyzePatterns(horizontals, verticals);
    }
    
    string List2Str(List<char> str)
    {
        return ("[" + string.Join(", ", str.Select(s => $"'{s}'")) + "]");
    }

    void AnalyzePatterns(List<char[,]> horizontals, List<char[,]> verticals)
    {
        int runningCount = 0;
        for (int i = 0; i < horizontals.Count; i++)
        {
            var horizontal = horizontals[i];
            var vertical = verticals[i];
            //Debug.Log("Checking horizontal...");
            //PrintJagged(horizontal);
            for (int j = 0; j < horizontal.GetLength(0)-1; j++)
            {
                if (GetRow(horizontal, j).SequenceEqual(GetRow(horizontal, j + 1)))
                {
                    if (IsMirrored(horizontal, j))
                    {
                        //Debug.Log($"adding my boi {(j+1)*100} after having checked the following two rows against each other: ");
                        //Debug.Log(List2Str(GetRow(horizontal, j).ToList()));
                        //Debug.Log(List2Str(GetRow(horizontal, j+1).ToList()));
                        runningCount += (j+1)*100;
                    }
                }
            }

            //Debug.Log("Checking vertical...");
            //PrintJagged(vertical);
            for (int j = 0; j < vertical.GetLength(0)-1; j++)
            {
                if (GetRow(vertical, j).SequenceEqual(GetRow(vertical, j + 1)))
                {
                    if (IsMirrored(vertical, j))
                    {
                        //Debug.Log($"adding my boi {j+1} after having checked the following two rows against each other: ");
                        //Debug.Log(List2Str(GetRow(vertical, j).ToList()));
                        //Debug.Log(List2Str(GetRow(vertical, j+1).ToList()));
                        runningCount += j+1;
                    }
                }
            }
        }

        Debug.Log($"here it is: {runningCount}");
    }

    bool IsMirrored(char[,] zals, int j)
    {
        var lim = Math.Min(j, zals.GetLength(0) - j - 2);
        //Debug.Log($"lim is {lim}, j is {j}, and the other part is {zals.GetLength(0) - j - 2}");
        for (int i = 0; i < lim; i++)
        {
            //Debug.Log($"now checking {List2Str(GetRow(zals, j+2+i).ToList())}, which is index {j+2+i} against {List2Str(GetRow(zals, j-1-i).ToList())}, which is index {j-1-i}. , my internal index is {i}, lim is {lim} and zals is {zals.GetLength(0)}");
            if (!GetRow(zals, j + 2 + i).SequenceEqual(GetRow(zals, j - 1 - i))) return false;
        }

        return true;
    }
    
    char[] GetRow(char[,] matrix, int rowIndex)
    {
        int columns = matrix.GetLength(1); // Get the number of columns
        char[] row = new char[columns];

        for (int col = 0; col < columns; col++)
        {
            row[col] = matrix[rowIndex, col];
        }

        return row;
    }

    
    

    List<(int width, int height)> GetRectangleDimensions(string[] lines)
    {
        List<(int width, int height)> dimensions = new List<(int width, int height)>();
        int currentHeight = 0;
        int maxWidth = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) // Check for an empty line
            {
                if (currentHeight > 0)
                {
                    dimensions.Add((maxWidth, currentHeight));
                    currentHeight = 0;
                    maxWidth = 0;
                }
            }
            else
            {
                int currentWidth = line.Length;
                maxWidth = Math.Max(maxWidth, currentWidth);
                currentHeight++;
            }
        }

        // Add the last rectangle if the last line isn't an empty line
        if (currentHeight > 0)
        {
            dimensions.Add((maxWidth, currentHeight));
        }

        return dimensions;
    }


    void PrintJagged(char[,] jagged)
    {
        string str = "";
        for (int i = 0; i < jagged.GetLength(0); i++)
        {
            for (int j = 0; j < jagged.GetLength(1); j++)
            {
                str += jagged[i, j];
            }

            str += "\n";
        }

        Debug.Log(str);
    }

    private char[,] TransposeRowsAndColumns(char[,] arr)
    {
        var newArray = new char[arr.GetLength(1), arr.GetLength(0)];
        string str = "";
        for (int i = 0; i < newArray.GetLength(0); i++)
        {
            for (int j = 0; j < newArray.GetLength(1); j++)
            {
                newArray[i, j] = arr[j, i];
                str += newArray[i, j];
            }
            str += "\n";
        }
        return newArray;
    }

}
