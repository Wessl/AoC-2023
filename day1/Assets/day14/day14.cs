using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class day14 : MonoBehaviour
{
    [TextArea] public string input;
    private List<List<char>> rockField;
    private void Start()
    {
        string[] lines = input.Split("\n");
        rockField = new List<List<char>>();
        for (int i = 0; i < lines.Length; i++)
        {
            rockField.Add(new List<char>());
            for (int j = 0; j < lines[0].Length; j++)
            {
                rockField[i].Add(lines[i][j]);
            }
        }
        // Tilt in some direction
        // TiltNorth();
        TiltSouth();
        PrintRocks();
        int res = CalculateRockValue();
        Debug.Log(res);
    }

    int CalculateRockValue()
    {
        int runningCounter = 0;
        for (int i = 0; i < rockField.Count; i++)
        {
            int multiplier = rockField.Count - i;
            runningCounter += (multiplier * (rockField[i].Count(r => r == 'O')));
        }

        return runningCounter;
    }

    private void PrintRocks()
    {
        string str = "";
        foreach (var rockRow in rockField)
        {
            foreach (var rock in rockRow)
            {
                str += rock;
            }

            str += "\n";
        }

        Debug.Log(str);
    }

    #region NORTH
    
    private void TiltNorth()
    {
        for (int i = 0; i < rockField.Count; i++)
        {
            for (int j = 0; j < rockField[i].Count; j++)
            {
                // this index - try to move it up
                // how far north can we go until we hit a wall or another stone?
                if (rockField[i][j] == 'O')
                {
                    ShiftUpwards(i,j);
                    //PrintRocks();
                }
                
            }
        }
    }

    void ShiftUpwards(int i, int j)
    {
        for (int k = i; k > 0 ; k--)
        {
            if (CanGoFurtherNorth(k, j))
            {
                //Debug.Log($"End of the line: From {i},{j}, we went all the way to {k},{j}!");
                rockField[k-1][j] = 'O';
                rockField[k][j] = '.';
            }
            else
            {
                break;
            }
        }
    }

    bool CanGoFurtherNorth(int i, int j)
    {
        if (i == 0) return false;
        if (rockField[i-1][j] == '#') return false;
        else if (rockField[i - 1][j] == 'O') return false;
        return true;
    }
    #endregion NORTH
    
    #region SOUTH
    
    private void TiltSouth()
    {
        for (int i = rockField.Count-1; i >= 0; i--)
        {
            for (int j = rockField[i].Count - 1; j >= 0; j--)
            {
                // this index - try to move it up
                // how far north can we go until we hit a wall or another stone?
                if (rockField[i][j] == 'O')
                {
                    ShiftDownwards(i,j);
                    //PrintRocks();
                }
                
            }
        }
    }

    void ShiftDownwards(int i, int j)
    {
        for (int k = i; k < rockField.Count ; k++)
        {
            if (CanGoFurtherSouth(k, j))
            {
                //Debug.Log($"End of the line: From {i},{j}, we went all the way to {k},{j}!");
                rockField[k+1][j] = 'O';
                rockField[k][j] = '.';
            }
            else
            {
                break;
            }
        }
    }

    bool CanGoFurtherSouth(int i, int j)
    {
        if (i >= rockField.Count-1) return false;
        if (rockField[i+1][j] == '#') return false;
        else if (rockField[i + 1][j] == 'O') return false;
        return true;
    }
    #endregion SOUTH
    
    
    
    
    
}
