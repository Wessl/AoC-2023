using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day5 : MonoBehaviour
{
    List<RangeMapper> seedToSoil;
    List<RangeMapper> soilToFertilizer;
    List<RangeMapper> fertilizerToWater;
    List<RangeMapper> waterToLight;
    List<RangeMapper> lightToTemperature;
    List<RangeMapper> temperatureToHumidity;
    List<RangeMapper> humidityToLocation;
    [TextArea] public string input;
    private List<RangeMapper>[] maps;
    private long lowestLocationYet;
    private void Start()
    {
        var time = DateTime.Now;
        string[] lines = input.Split("\r\n");
        long[] seeds = ExtractSeeds(lines);
        ExtractMaps(lines);
        
        lowestLocationYet = int.MaxValue;
        maps = new List<RangeMapper>[]{
            seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature,
            temperatureToHumidity, humidityToLocation
        };
        Hijacker();
        long lowestEver = long.MaxValue;
        for (int i = 0; i < seeds.Length; i+=2)
        {
            var seedRange = seeds[i+1];
            long lowestThisSeed = HandleSeeds(seedRange, seeds[i]);
            if (lowestThisSeed < lowestEver) lowestEver = lowestThisSeed;
        }

        Debug.Log($"lowest: {lowestEver}");
        Debug.Log("okay. " + (DateTime.Now.Millisecond - time.Millisecond).ToString());
    }

    void Hijacker()
    {
        // this is so ugly lmao
        long seed = 1950498166;
        long val = long.MaxValue;
        for (int i = 0; i < 1000; i++)
        {
            long newVal = SampleSeed(seed);
            if (newVal < val) val = newVal;
            seed--;
        }

        Debug.Log($"seed: {seed}, value: {val}");
    }

    private long HandleSeeds(long seedRange, long seedStart, int divisions = 2000)
    {
        if (seedRange <= 1)
        {
            return SampleSeed(seedStart);
        }

        long bestSeedValue = long.MaxValue;
        long bestSeed = seedStart;
        long rangeDivision = seedRange / divisions;

        for (long i = 0; i <= divisions; i++)
        {
            long currentSeed = seedStart + i * rangeDivision;
            long currentSeedValue = SampleSeed(currentSeed);

            if (currentSeedValue < bestSeedValue)
            {
                bestSeedValue = currentSeedValue;
                bestSeed = currentSeed;
            }
        }

        // Narrow the search range around the best seed found
        long newRangeStart = Math.Max(seedStart, bestSeed - rangeDivision);
        long newRangeEnd = Math.Min(seedStart + seedRange, bestSeed + rangeDivision);
        long newRange = newRangeEnd - newRangeStart;
        Debug.Log($"best seed: {bestSeed}, which results in {SampleSeed(bestSeed)}" );

        // Recursively refine the search in the narrowed range
        long recursiveResult = HandleSeeds(newRange, newRangeStart, divisions);

        // Return the smaller of the two values
        return Math.Min(bestSeedValue, recursiveResult);
    }

    long SampleSeed(long seed)
    {
        long runningValue = seed;
        for (int j = 0; j < maps.Length; j++)
        {
            foreach (var rm in maps[j])
            {
                if (runningValue >= rm.startSource && runningValue < (rm.startSource + rm.rangeLength))
                {
                    runningValue += (rm.startDest - rm.startSource);
                    break;  // Don't find something again in the same map if we already did... lol
                }
            }
        }

        return runningValue;
    }

    private long[] ExtractSeeds(string[] lines)
    {
        return lines[0].Split(":")[1].Trim().Split(" ").Select(long.Parse).ToArray();
    }

    private void ExtractMaps(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("seed-to-soil")) seedToSoil = ExtractMap(lines, i, "seed to soil");
            if (lines[i].Contains("soil-to-fertilizer")) soilToFertilizer = ExtractMap(lines, i, "soil to fertizlier");
            if (lines[i].Contains("fertilizer-to-water")) fertilizerToWater = ExtractMap(lines, i, "fertilizer to water");
            if (lines[i].Contains("water-to-light")) waterToLight = ExtractMap(lines, i, "water to light");
            if (lines[i].Contains("light-to-temperature")) lightToTemperature = ExtractMap(lines, i, "light to temp");
            if (lines[i].Contains("temperature-to-humidity")) temperatureToHumidity = ExtractMap(lines, i, "temp to humidity");
            if (lines[i].Contains("humidity-to-location")) humidityToLocation = ExtractMap(lines, i, "humidity to location");
        }
    }
    
    private List<RangeMapper> ExtractMap(string[] lines, int index, string debugWho)
    {
        List<RangeMapper> rms = new List<RangeMapper>();
        Debug.Log($"now adding {debugWho} mappings.");
        for (int i = index+1; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
            {
                return rms;
            }
            string[] line = lines[i].Split(" ");
            long dest = long.Parse(line[0]);
            long source = long.Parse(line[1]);
            long length = long.Parse(line[2]);
            rms.Add(new RangeMapper(source, dest, length));
        }

        return rms;
    }

    class RangeMapper
    {
        public long startSource;
        public long startDest;
        public long rangeLength;

        public RangeMapper(long startSource, long startDest, long rangeLength)
        {
            this.startSource = startSource;
            this.startDest = startDest;
            this.rangeLength = rangeLength;
        }
    }
}
