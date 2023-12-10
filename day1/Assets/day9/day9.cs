using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class day9 : MonoBehaviour
{
    [TextArea] public string input;
    private List<int[]> sequencesLinear;

    private void Start()
    {
        var lines = input.Split("\r\n");
        int sum = 0;
        foreach (var line in lines)
        {
            ProduceOasis(line);
            //sum += Extrapolate();
            sum += ExtrapolateBackwards();
        }

        Debug.Log("sum: " + sum);
    }

    void ProduceOasis(string line)
    {
        int[] numbers = line.Split(" ").Select(int.Parse).ToArray();
        sequencesLinear = new List<int[]>();
        StepDown(numbers);
        Debug.Log("we've just used step down what do we get back? ");
        foreach (var sequnec in sequencesLinear)
        {
            Debug.Log($": [{string.Join(", ", sequnec)}]");
        }
    }
    
    int ExtrapolateBackwards()
    {
        for (int i = sequencesLinear.Count - 1; i >= 1; i--)
        {
            var sequence = sequencesLinear[i].ToList();
            var first = sequence.First();
            var newList = sequencesLinear[i - 1].ToList();
            newList.Insert(0,sequencesLinear[i - 1].First() - first);

            // Assign the new list back to sequencesLinear[i - 1]
            sequencesLinear[i-1] = newList.ToArray();            
            //Debug.Log($"Modified Sequence: [{string.Join(", ", sequencesLinear[i-1])}]");
        }
        Debug.Log($"Final Sequence: [{string.Join(", ", sequencesLinear.First())}]");
        return sequencesLinear[0].First();
    }

    int Extrapolate()
    {
        for (int i = sequencesLinear.Count - 1; i >= 1; i--)
        {
            var sequence = sequencesLinear[i].ToList();
            var last = sequence.Last();
            var newList = sequencesLinear[i - 1].ToList();
            newList.Add(sequencesLinear[i - 1].Last() + last);

            // Assign the new list back to sequencesLinear[i - 1]
            sequencesLinear[i-1] = newList.ToArray();            
            //Debug.Log($"Modified Sequence: [{string.Join(", ", sequencesLinear[i-1])}]");
        }
        Debug.Log($"Final Sequence: [{string.Join(", ", sequencesLinear.First())}]");
        return sequencesLinear[0].Last();
    }

    void StepDown(int[] numbers)
    {
        int[] oneStepDownNumbers = new int[numbers.Length - 1];
        for (var index = 0; index < numbers.Length-1; index++)
        {
            var n = numbers[index];
            var diff = numbers[index + 1] - n;
            oneStepDownNumbers[index] = diff;
        }
        sequencesLinear.Add(numbers);
        //Debug.Log($"Added Sequence: [{string.Join(", ", numbers)}]");
        if (AllZero(numbers)) return;
        StepDown(oneStepDownNumbers);
    }

    private bool AllZero(int[] arr)
    {
        bool isZero = true;
        foreach (var a in arr)
        {
            if (a != 0) isZero = false;
        }
        return isZero;
    }
}
