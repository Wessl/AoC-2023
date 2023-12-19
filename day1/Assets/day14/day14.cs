using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class day14 : MonoBehaviour
{
    [TextArea] public string input;
    private List<List<char>> rockField;
    public int loopdiloops = 100000;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("le tap");
            RunIt();
        }
    }

    private void RunIt()
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

        List<int> results = new List<int>();
        // Tilt in some direction
        for (int i = 0; i < loopdiloops; i++)
        {
            TiltNorth();
            TiltWest();
            TiltSouth();
            TiltEast();
            results.Add(CalculateRockValue());
        }
        // Realize that the result becomes cyclic after a while - for me it was a cycle length of 19
        Debug.Log(List2Str(results));
    }
    
    string List2Str(List<int> str)
    {
        return ("[" + string.Join(", ", str.Select(s => $"{s}")) + "]");
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
    
    #region EAST
    
    private void TiltEast()
    {
        for (int j = rockField[0].Count-1; j >= 0; j--)
        {
            for (int i = rockField.Count - 1; i >= 0; i--)
            {
                // this index - try to move it up
                // how far north can we go until we hit a wall or another stone?
                if (rockField[i][j] == 'O')
                {
                    ShiftRight(i,j);
                    //PrintRocks();
                }
                
            }
        }
    }

    void ShiftRight(int i, int j)
    {
        for (int k = j; k < rockField[0].Count-1 ; k++)
        {
            if (CanGoFurtherEast(i, k))
            {
                //Debug.Log($"End of the line: From {i},{j}, we went all the way to {k},{j}!");
                rockField[i][k+1] = 'O';
                rockField[i][k] = '.';
            }
            else
            {
                break;
            }
        }
    }

    bool CanGoFurtherEast(int i, int j)
    {
        if (j >= rockField[0].Count-1) return false;
        if (rockField[i][j+1] == '#') return false;
        else if (rockField[i][j+1] == 'O') return false;
        return true;
    }
    #endregion EAST
    
    #region WEST
    
    private void TiltWest()
    {
        for (int j = 0; j < rockField[0].Count; j++)
        {
            for (int i = rockField.Count - 1; i >= 0; i--)
            {
                // this index - try to move it up
                // how far north can we go until we hit a wall or another stone?
                if (rockField[i][j] == 'O')
                {
                    ShiftLeft(i,j);
                    //PrintRocks();
                }
                
            }
        }
    }

    void ShiftLeft(int i, int j)
    {
        for (int k = j; k >= 0 ; k--)
        {
            if (CanGoFurtherWest(i, k))
            {
                //Debug.Log($"End of the line: From {i},{j}, we went all the way to {k},{j}!");
                rockField[i][k-1] = 'O';
                rockField[i][k] = '.';
            }
            else
            {
                break;
            }
        }
    }

    bool CanGoFurtherWest(int i, int j)
    {
        if (j <= 0) return false;
        if (rockField[i][j-1] == '#') return false;
        else if (rockField[i][j-1] == 'O') return false;
        return true;
    }
    #endregion WEST
    
    
}
