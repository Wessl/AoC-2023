using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Day3Part2 : MonoBehaviour
{
    [TextArea]
    public string input;

    private char[][] jaggedLines;

    private string[] lines;
    public TextMeshProUGUI tmpro;

    private int sum = 0;

    private string gearFoundPos = "";
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
        // Handle Gears first of all... 
        for (int row = 0; row < lines.Length; row++)
        {
            var line = lines[row].ToCharArray();
            tmpro.text += line.ToString() + "\n";
            // Look for gears, find any adjacent numbers
            // If you find one, add it, if you find several, multiply them
            // Once a gear has been handled, ERASE the numbers and the gear from the input. 
            HandleGearLine(lines[row], row);
        }
        /*
        for (int row = 0; row < lines.Length; row++)
        {
            var line = lines[row].ToCharArray();
            tmpro.text += line.ToString() + "\n";
            HandleLine(lines[row], row);
        }
        */

        Debug.Log("sum : " + sum);
    }

    private void HandleGearLine(string line, int row)
    {
        for (int col = 0; col < line.Length; col++)
        {
            char c = jaggedLines[row][col];
            if (c == '.') continue;
            if (c == '*')
            {
                int gearResult = CheckGearForAdjacentNumbers(col, row);
                Debug.Log($"Just found a gear, it gave us {gearResult}");
                sum += gearResult;
            }
        }
    }

    void RemoveNumberFromJagged(int row, int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            Debug.Log($"removing from {jaggedLines[row][i]} from jagged!");
            jaggedLines[row][i] = 'x';
        }
    }

    void HandleLine(string line, int row)
    {
        // First handle myself
        // Then if there is another number after me, handle that as well
        for (int col = 0; col < line.Length; col++)
        {
            char c = jaggedLines[row][col];
            if (c == '.') continue;
            if (Char.IsNumber(c) && CheckNumberForAdjacentParts(col, row))
            {
                // Debug.Log($"Ah, {c} is next to a part.");
                sum += GetNumberIamPartOf(col, row, out int nextIndexToCheck, out int startIndexForNumber);
                col = nextIndexToCheck;
            }
        }
        
    }

    class CharPosition
    {
        public char character;
        public int i;
        public int j;

        public CharPosition(char character, int i, int j)
        {
            this.character = character;
            this.i = i;
            this.j = j;
        }
    }

    
    
    int CheckGearForAdjacentNumbers(int i, int j)
    {
        CharPosition[] positions = new CharPosition[8];

        positions[0] = new CharPosition(JaggedPosExists(i-1,j) ? jaggedLines[j][i-1] : '_', i-1, j);
        positions[1] = new CharPosition(JaggedPosExists(i-1,j+1) ? jaggedLines[j+1][i-1] : '_', i-1, j+1);
        positions[2] = new CharPosition(JaggedPosExists(i, j+1) ? jaggedLines[j+1][i] : '_', i, j+1);
        positions[3] = new CharPosition(JaggedPosExists(i+1,j+1) ? jaggedLines[j+1][i+1] : '_', i+1, j+1);
        positions[4] = new CharPosition(JaggedPosExists(i+1,j) ? jaggedLines[j][i+1] : '_', i+1, j);
        positions[5] = new CharPosition(JaggedPosExists(i+1, j-1) ? jaggedLines[j-1][i+1] : '_', i+1, j-1);
        positions[6] = new CharPosition(JaggedPosExists(i,j-1) ? jaggedLines[j-1][i] : '_', i, j-1);
        positions[7] = new CharPosition(JaggedPosExists(i-1,j-1) ? jaggedLines[j-1][i-1] : '_', i-1, j-1);
        List<int> foundNumbers = new List<int>();
        for (int k = 0; k < positions.Length; k++)
        {
            var pos = positions[k];
            if (string.IsNullOrEmpty(pos.ToString())) continue;
            if (Char.IsNumber(pos.character))
            {
                // We just found a number. Handle the whole thing! 
                Debug.Log($"Checking char {pos.character}");
                if (jaggedLines[pos.j][pos.i] == 'x') continue;
                foundNumbers.Add(GetNumberIamPartOf(pos.i, pos.j, out int endIndexForNumber, out int startIndexForNumber));
                RemoveNumberFromJagged(pos.j, startIndexForNumber, endIndexForNumber);
            }
        }
        RemoveNumberFromJagged(j, i, i+1);
        int r = 1;
        for (int k = 0; k < foundNumbers.Count; k++)
        {
            Debug.Log($"multiplying up {foundNumbers[k]}");
            r = r * foundNumbers[k]; // or equivalently r *= mult[i];
        }
        if (r == 1) return 0;
        if (foundNumbers.Count == 1) return 0;
        return r;
    }


    bool CheckNumberForAdjacentParts(int i, int j)
    {
        string symbols = "+£&/#*$@%=-";
        char[] positions = new char[8];
        string[] gearPositions = new string[8];
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
    
    int GetNumberIamPartOf(int col, int row, out int nextIndexToCheck, out int startIndexForNumber)
    {
        // Go all the way to the left
        char[] line = jaggedLines[row];
        startIndexForNumber = 0;
        string numberFound = "";
        for (int i = col; i >= 0; i--)
        {
            if (Char.IsNumber(line[i]))
            {
                startIndexForNumber = i;
            }
            else
                break;
            
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
                break;
            
        }

        Debug.Log($"number found is {numberFound}");
        return int.Parse(numberFound);
    }


}
