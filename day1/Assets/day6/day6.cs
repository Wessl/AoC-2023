using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class day6 : MonoBehaviour
{
    [TextArea] public string input;

    private void Start()
    {
        var times = input.Split("\r\n")[0].Split(":")[1].Replace(" ", "");
        var distances = input.Split("\r\n")[1].Split(":")[1].Replace(" ", "");
        long waysToWin = 1;
        long time = long.Parse(times);
        long goalDistance = long.Parse(distances);
        long waysToWinRace = 0;
        for (int j = 0; j < time; j++)
        {
            long timeAttempt = j;
            long timeLeft = time - timeAttempt;
            long distanceTravelled = timeAttempt * timeLeft;
            if (distanceTravelled > goalDistance) waysToWinRace++;
        }

        waysToWin *= waysToWinRace;
        

        Debug.Log(waysToWin);
    }
}
