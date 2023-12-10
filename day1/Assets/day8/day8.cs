using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class day8 : MonoBehaviour
{
    [TextArea] public string input;

    private Dictionary<string, string> path;

    private char[] instructionsArr;
    private int instructionCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        var lines = input.Split("\r\n");
        var instructions = lines[0];
        instructionsArr = instructions.ToCharArray();
        path = new Dictionary<string, string>();
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i];
            var lefthandside = line.Split("=")[0].Trim();
            var righthandside = line.Split("=")[1].Trim();
            Debug.Log($"adding {lefthandside}, {righthandside}");
            path.Add(lefthandside,righthandside);
        }
        // Find starting positions 
        List<string> startPositions = new List<string>();
        foreach (var kvp in path)
        {
            if (kvp.Key.EndsWith('A'))
            {
                startPositions.Add(kvp.Key);
                Debug.Log("start pos: " + kvp.Key);
            }
        }
        
        // move one step with each input, if last char of all is not z, continue
        int counter = 0;
        List<string> results = new List<string>();
        // not like this. go through each one. find the cycle length. then find the LCM of them all. duh
        /*
        while (!AllEndOnZ(results) && counter < 10000)
        {
            results = new List<string>();
            for (var index = 0; index < startPositions.Count; index++)
            {
                var startPos = startPositions[index];
                var res = MoveOneElement((startPos),
                    instructionsArr[instructionCounter % (instructionsArr.Length)]);
                results.Add(res);
                startPositions[index] = res;
            }

            instructionCounter++;
            counter++;
        }
        */
        List<long> cycleLengthForMe = new List<long>();
        for (var index = 0; index < startPositions.Count; index++)
        {
            instructionCounter = 0;
            while (!startPositions[index].EndsWith('Z'))
            {
                var startPos = startPositions[index];
                var res = MoveOneElement((startPos),
                    instructionsArr[instructionCounter % (instructionsArr.Length)]);
                instructionCounter++;
                startPositions[index] = res;
            }
            cycleLengthForMe.Add(instructionCounter);
        }
        Debug.Log("[" + string.Join(", ", cycleLengthForMe.Select(s => $"'{s}'")) + "]");
        print($"lcm: {LCM(cycleLengthForMe.ToArray())}");
        
        //var res = HandleInstructions(instructions);
        Debug.Log(counter);
    }
    static long LCM(long[] numbers)
    {
        return numbers.Aggregate(lcm);
    }
    static long lcm(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
    static long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
    


    bool AllEndOnZ(List<string> results)
    {
        if (results.Count == 0) return false; // first time
        Debug.Log("[" + string.Join(", ", results.Select(s => $"'{s}'")) + "]");
        foreach (var res in results)
        {
            if (!res.EndsWith('Z')) return false;
        }
        return true;
    }

    // for part 1
    int HandleInstructions(string instructions)
    {
        string currElement = "AAA";
        int counter = 0;
        while (currElement != "ZZZ")
        {
            foreach (var c in instructions.ToCharArray())
            {
                var left = path[currElement].Split(",")[0].Trim().Substring(1);
                var right = path[currElement].Split(",")[1].Trim().Substring(0,3);
            
                if (c == 'L')
                {
                    currElement = left;
                }
                else
                {
                    currElement = right;
                }
                counter++;
            }
        }
        return counter;
    }

    string MoveOneElement(string currElement, char currDir)
    {
        // returns the result of moving one element, given an input
        var left = path[currElement].Split(",")[0].Trim().Substring(1);
        var right = path[currElement].Split(",")[1].Trim().Substring(0,3);
        if (currDir == 'L')
        {
            currElement = left;
        }
        else
        {
            currElement = right;
        }

        return currElement;
    }

    
}
