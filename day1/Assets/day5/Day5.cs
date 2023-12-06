using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day5 : MonoBehaviour
{
    Dictionary<int,int> seedToSoil = new Dictionary<int, int>();
    Dictionary<int,int> soilToFertilizer = new Dictionary<int, int>();
    Dictionary<int,int> fertilizerToWater = new Dictionary<int, int>();
    Dictionary<int,int> waterToLight = new Dictionary<int, int>();
    Dictionary<int,int> lightToTemperature = new Dictionary<int, int>();
    Dictionary<int,int> temperatureToHumidity = new Dictionary<int, int>();
    Dictionary<int,int> humidityToLocation = new Dictionary<int, int>();
    [TextArea] public string input;
    private void Start()
    {
        string[] lines = input.Split("\r\n");
        int[] seeds = ExtractSeeds(lines);
        ExtractMaps(lines);
        for (int a = 0; a < seedToSoil.Count; a++)
        {
            Debug.Log(a + ", "+ seedToSoil[a]);
        }
    }

    private int[] ExtractSeeds(string[] lines)
    {
        return lines[0].Split(":")[1].Trim().Split(" ").Select(int.Parse).ToArray();
    }

    private void ExtractMaps(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("seed-to-soil")) seedToSoil = ExtractMap(lines, i);
            if (lines[i].Contains("soil-to-fertilizer")) soilToFertilizer = ExtractMap(lines, i);
            if (lines[i].Contains("fertilizer-to-water")) fertilizerToWater = ExtractMap(lines, i);
            if (lines[i].Contains("water-to-light")) waterToLight = ExtractMap(lines, i);
            if (lines[i].Contains("light-to-temperature")) lightToTemperature = ExtractMap(lines, i);
            if (lines[i].Contains("temperature-to-humidity")) temperatureToHumidity = ExtractMap(lines, i);
            if (lines[i].Contains("humidity-to-location")) humidityToLocation = ExtractMap(lines, i);
        }
    }

    private Dictionary<int, int> ExtractMap(string[] lines, int index)
    {
        Dictionary<int, int> myDict = new Dictionary<int, int>();
        for (int i = index+1; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
            {
                for (int j = 0; j < 100; j++)
                {
                    myDict.TryAdd(j, j);
                }

                return myDict;
            }
            string[] line = lines[i].Split(" ");
            int source = int.Parse(line[0]);
            int dest = int.Parse(line[1]);
            int length = int.Parse(line[2]);
            for (int j = 0; j < length; j++)
            {
                myDict.Add(j+source,j+dest);
            }
        }

        for (int i = 0; i < 100; i++)
        {
            myDict.TryAdd(i, i);
        }

        return myDict;
    }
}
