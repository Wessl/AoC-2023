using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Day1Solver : MonoBehaviour
{
    [SerializeField] string input;
    [SerializeField] TextMeshProUGUI tmpro;
    [SerializeField] private TextMeshProUGUI runningTotal;
    private int total;

    private void Start()
    {
        string[] lines = input.Split();
        StartCoroutine(TextUpdater(lines));
    }

    IEnumerator TextUpdater(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            int first = FindFirstNumber(lines[i], out int firstIndex);
            int second = FindLastNumber(lines[i], out int secondIndex);
            total += int.Parse(first.ToString() + second.ToString());
            char[] currLineChars = lines[i].ToCharArray();
            string newString = GiveStringColor(lines[i], firstIndex, secondIndex);
            tmpro.text += newString + "\n";
            // total += int.Parse(currLineChars[first].ToString() + currLineChars[second].ToString());
            runningTotal.text = total.ToString();
            if (tmpro.text.Length > 1000) tmpro.text = tmpro.text.Substring(tmpro.text.Length - 700);
            yield return new WaitForEndOfFrame();
        }
    }

    private string GiveStringColor(string s, int first, int second)
    {
        string startTag = "<color=#E0E300>";
        string endTag = "</color>";
        
        s = s.Insert(first, startTag);
        s = s.Insert(first + startTag.Length + 1, endTag);

        if (first == second) return s;
        second += startTag.Length + endTag.Length;

        s = s.Insert(second, startTag);
        s = s.Insert(second + startTag.Length + 1, endTag);
        

        return s;
    }


    private int FindFirstNumber(string line, out int index)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (Char.IsNumber(line[i]))
            {
                index = i;
                return int.Parse(line[i].ToString());
            }
            if (ANumberStartsHere(line, i, out int number))
            {
                index = i;
                return number;
            }
        }

        index = 0;
        return 0;
    }

    private bool ANumberStartsHere(string line, int index, out int number)
    {
        // Map of number words to their numeric values
        var numberWords = new Dictionary<string, int>
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9},
        };

        number = 0;
        if (index >= line.Length) return false; // Check for valid index

        foreach (var pair in numberWords)
        {
            // Check if the substring starting at index matches a number word
            if (line.Length >= index + pair.Key.Length && line.Substring(index, pair.Key.Length) == pair.Key)
            {
                number = pair.Value;
                return true;
            }
        }

        return false; // Return false if no number word is found
    }


    private int FindLastNumber(string line, out int index)
    {
        for (int i = line.Length-1; i >= 0; i--)
        {
            if (Char.IsNumber(line[i]))
            {
                index = i;
                return int.Parse(line[i].ToString());
            }

            if (ANumberStartsHere(line, i, out int number))
            {
                index = i;
                return number;
            }
        }

        index = 0;
        return 0;
    }
}
