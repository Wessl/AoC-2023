using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Day3Solver : MonoBehaviour
{
    [TextArea]
    public string input;

    private char[][] jaggedLines;

    private string[] lines;
    public TextMeshProUGUI tmpro;

    private int sum = 0;
    // Start is called before the first frame update
    void Start()
    {
        lines = input.Split("\r\n");
        jaggedLines = new char[lines.Length][];
        // Assign stuff to jaggedLines
        for (int row = 0; row < lines.Length; row++)
        {
            jaggedLines[row] = lines[row].ToCharArray();
        }

        for (int row = 0; row < lines.Length; row++)
        {
            var line = lines[row].ToCharArray();
            tmpro.text += line.ToString() + "\n";
            HandleLine(lines[row], row);
        }

        Debug.Log("sum : " + sum);
    }

    void HandleLine(string line, int row)
    {
        // First handle myself
        // Then if there is another number after me, handle that as well
        for (int col = 0; col < line.Length; col++)
        {
            char c = jaggedLines[row][col];
            if (c == '.') continue;
            if (Char.IsNumber(c) && CheckAdjacent(col, row))
            {
                // Debug.Log($"Ah, {c} is next to a part.");
                sum += GetNumberIamPartOf(col, row, out int nextIndexToCheck);
                col = nextIndexToCheck;
            }
        }
        
    }

    int GetNumberIamPartOf(int col, int row, out int nextIndexToCheck)
    {
        // Go all the way to the left
        char[] line = jaggedLines[row];
        int startIndexForNumber = 0;
        string numberFound = "";
        for (int i = col; i >= 0; i--)
        {
            if (Char.IsNumber(line[i]))
            {
                startIndexForNumber = i;
            }
            else
            {
                break;
            }
        }

        nextIndexToCheck = 0;
        for (int i = startIndexForNumber; i < line.Length; i++)
        {
            if (Char.IsNumber(line[i]))
            {
                numberFound += line[i];
                nextIndexToCheck = i;
            }
            else
            {
                break;
            }
        }

        // Debug.Log($"We just found a number that is probably what we want: {numberFound}");
        return int.Parse(numberFound);
    }

    bool CheckAdjacent(int i, int j)
    {
        string symbols = "+£&/#*$@%=-";
        char[] positions = new char[8];
        positions[0] = JaggedPosExists(i-1,j) ? jaggedLines[j][i-1] : '_';
        positions[1] = JaggedPosExists(i-1,j+1) ? jaggedLines[j+1][i-1] : '_';
        positions[2] = JaggedPosExists(i, j+1) ? jaggedLines[j+1][i] : '_';
        positions[3] = JaggedPosExists(i+1,j+1) ? jaggedLines[j+1][i+1] : '_';
        positions[4] = JaggedPosExists(i+1,j) ? jaggedLines[j][i+1] : '_';
        positions[5] = JaggedPosExists(i+1, j-1) ? jaggedLines[j - 1][i+1] : '_';
        positions[6] = JaggedPosExists(i,j-1) ? jaggedLines[j-1][i] : '_';
        positions[7] = JaggedPosExists(i-1,j-1) ? jaggedLines[j-1][i-1] : '_';
        for (int k = 0; k < positions.Length; k++)
        {
            var pos = positions[k];
            if (string.IsNullOrEmpty(pos.ToString())) continue;
            if (!Char.IsNumber(jaggedLines[j][i])) continue;
            if (symbols.Contains(pos))
            {
                // Debug.Log($"We just checked {pos} for {jaggedLines[j][i]}");
                return true;
            }
        }

        return false;
    }

    bool JaggedPosExists(int i, int j)
    {
        if (i >= 0 && i < jaggedLines[0].Length && j >= 0 && j < jaggedLines.Length) return true;
        return false;
    }

    
}
