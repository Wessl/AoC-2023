using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class day11 : MonoBehaviour
{
    [TextArea] public string input;
    private char[,] starMap;

    public int expandStarMapBy;
    // How to solve part 2: realize that the relationship in terms of power does not change when the stars move by multiples of 10
    // if you know how big the difference is between 1 space and 10, you can figure out the difference between the next ones too
    /* For me it was like this:
     my input: 
1 -> 9414682		diff: 0
10 -> 14367832		diff: 4953150
100 -> 63899332		diff: 49531500
1000 -> 559214332   diff: 495315000
then you keep going... 

     * 1: 9414682 + 550349449650, which was gotten from:
expansion   difference from previous
10          4953150
100         49531500
1000        495315000
10000       4953150000
100000      49531500000
1000000     495315000000
add up the 1, and then each difference, and you got your answer :)
     */
    void Start()
    {
        starMap = new char[input.Split("\r\n").Length, input.Split("\r\n")[0].Length];
        var inps = input.Split("\r\n");
        for (var row = 0; row < inps.Length; row++)
        {
            var inp = inps[row].ToCharArray();
            for (int column = 0; column < inp.Length; column++)
            {
                starMap[row, column] = inp[column];
            }
            
        }
        Print2DArr(starMap);
        var expandedStarMap = ExpandStarMap(starMap);
        //Print2DArr(expandedStarMap);
        List<Vector2Int> galaxyPositions = FindGalaxyPositions(expandedStarMap);
        int total = 0;
        for (int i = 0; i < galaxyPositions.Count; i++)
        {
            total += FindShortestPathBetweenPairs(i, galaxyPositions[i], galaxyPositions);
        }


        Debug.Log("total: " + total);
    }

    int FindShortestPathBetweenPairs(int thisStar, Vector2Int glxp, List<Vector2Int> galaxyPositions)
    {
        int thisGalaxysDistances = 0;
        for (int i = thisStar + 1; i < galaxyPositions.Count; i++) // Start from the next galaxy
        {
            var other = galaxyPositions[i];
            var dist = Math.Abs(glxp.x - other.x) + Math.Abs(glxp.y - other.y);
            // Debug.Log($"dist between star number {thisStar + 1} and {i + 1} is {dist}");
            thisGalaxysDistances += dist;
        }
        return thisGalaxysDistances;
    }


    List<Vector2Int> FindGalaxyPositions(char[,] starMapBoi)
    {
        List<Vector2Int> galaxyPositions = new List<Vector2Int>();
        for (var row = 0; row < starMapBoi.GetLength(0); row++)
        {
            for (int column = 0; column < starMapBoi.GetLength(1); column++)
            {
                if (starMapBoi[row, column] == '#')
                {
                    galaxyPositions.Add(new Vector2Int(row,column));
                } 
            }
        }
        return galaxyPositions;
    }

    void Print2DArr(char[,] twodmap)
    {
        // terminology... 
        string str = "";
        for (var row = 0; row < twodmap.GetLength(0); row++)
        {
            
            for (int column = 0; column < twodmap.GetLength(1); column++)
            {
                str += twodmap[row, column];
            }

            str += "\n";
        }
        Debug.Log(str);
    }

    char[,] ExpandStarMap(char[,] starMap)
    {
        int internalIterator = expandStarMapBy - 1;
        List<List<char>> tempStarMap = new List<List<char>>();
        for (int i = 0; i < starMap.GetLength(0); i++)
        {
            var row = GetRow(starMap, i);
            tempStarMap.Add(row.ToList());
            if (IsLineGalaxyFree(row))
            {
                for (int j = 0; j < internalIterator; j++)
                {
                    tempStarMap.Add(row.ToList());
                }
            }
        }

        int colsAdded = 0;
        for (int i = 0; i < starMap.GetLength(1); i++)
        {
            var column = GetColumn(starMap, i);
            if (IsLineGalaxyFree(column))
            {
                for (int k = 0; k < internalIterator; k++)
                {
                    for (int j = 0; j < tempStarMap.Count; j++)
                    {
                        tempStarMap[j].Insert(i+colsAdded, '.');
                    }
                    colsAdded++;
                }
            }
        }
        
        char[,] expandedMap = new char[tempStarMap.Count, tempStarMap[0].Count];
        for (int i = 0; i < tempStarMap.Count; i++)
        {
            for (int j = 0; j < tempStarMap[i].Count; j++)
            {
                expandedMap[i, j] = tempStarMap[i][j];
            }
        }

        return expandedMap;
    }

    bool IsLineGalaxyFree(char[] line)
    {
        foreach (var c in line)
        {
            if (c == '#') return false;
        }
        return true;
    }
    
    public char[] GetColumn(char[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
            .Select(x => matrix[x, columnNumber])
            .ToArray();
    }

    public char[] GetRow(char[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
            .Select(x => matrix[rowNumber, x])
            .ToArray();
    }
}
