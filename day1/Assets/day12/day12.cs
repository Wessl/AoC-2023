using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class day12 : MonoBehaviour
{
    // le guide
    /*
     * idk go very well but I think your code is still useable

I'll explain what made the DP part click for me

the space of possible combinations is a tree that splits into two branches on every question mark, right? But if you expand out the tree (I recommend doing it on the sample if you want to see), many of the branches end up being identical. For example with the sequence ??. if you have a group 1 spring long it doesn't matter which question mark becomes a spring and which becomes empty, they both lead to the same state afterwards. So to save time you want to store/cache the results of states and look in that cache before doing another recursion. To do that you can make a hashmap where the key is the sequence + group numbers and the value is the amount of possible combinations associated There's probably much better ways to do it but this worked for me
     */
    [TextArea] public string input;
    private Stopwatch watch = new System.Diagnostics.Stopwatch();
    void Start()
    {
        int theRealTotal = 0;
        foreach (var line in input.Split("\r\n"))
        {
            theRealTotal += HandleConditionRecord(line);
        }
        Debug.Log("the real total: " + theRealTotal);
    }

    private long generateCombinationsTotal = 0;
    private long fitGroupsTotal = 0;
    int HandleConditionRecord(string line)
    {
        var conditionRecord = line.Split(" ")[0];
        var contiguousGroups = line.Split(" ")[1];
        // Generate all combinations
        List<string> combinations = new List<string>();
        GenerateCombinations(conditionRecord, 0, combinations);
        // Try to see if any of them actually work? As in, can "fit" the input
        int myTotal = 0;
        List<int> numbers = contiguousGroups.Split(",").Select(i => int.Parse(i)).ToList();
        foreach (var combination in combinations)
        {
            myTotal += FitGroups(combination, numbers);
        }
        return myTotal;
    }

    private int FitGroups(string combo, List<int> numbers)
    {
        List<int> seqLens = GetSequenceLengths(combo);
        if (seqLens.Count != numbers.Count) return 0; 
        // Debug.Log($"is the sequence {List2Str(numbers)} the same as {List2Str(seqLens)} ? {numbers.SequenceEqual(seqLens)} ");
        return numbers.SequenceEqual(seqLens) ? 1 : 0;
    }

    string List2Str(List<int> str)
    {
        return ("[" + string.Join(", ", str.Select(s => $"'{s}'")) + "]");
    }

    private List<int> GetSequenceLengths(string combo)
    {
        // Debug.Log(combo);
        return combo.Split('.').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Length).ToList();
    }

    void GenerateCombinations(string current, int pos, List<string> combinations)
    {
        // Don't even generate some of the combinations, for instance, 
        if (pos == current.Length)
        {
            combinations.Add(current);
            return;
        }

        if (current[pos] == '?')
        {
            GenerateCombinations(current.Substring(0, pos) + '.' + current.Substring(pos + 1), pos + 1, combinations);
            GenerateCombinations(current.Substring(0,pos) + '#' + current.Substring(pos + 1), pos + 1, combinations);
        }
        else
        {
            GenerateCombinations(current, pos + 1, combinations);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
