using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class day7 : MonoBehaviour
{
    // Define a custom comparer for arrays of integers
    public class IntArrayComparer : IComparer<int[]>
    {
        public int Compare(int[] x, int[] y)
        {
            int minLength = Math.Min(x.Length, y.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (x[i] != y[i])
                {
                    return x[i].CompareTo(y[i]);
                }
            }
            return x.Length.CompareTo(y.Length);
        }
    }
    public class Hand
    {
        public int type;
        public int value;
        public int[] cards;

        public Hand(int[] cards, int value, int type)
        {
            this.cards = cards;
            this.value = value;
            this.type = type;
        }
    }
    [TextArea] public string input;
    private List<Hand> hands;
    private void Start()
    {
        var lines= input.Split("\r\n");
        hands = new List<Hand>();
        for (int i = 0; i < lines.Length; i++)
        {
            var splitLine = lines[i].Split(" ");
            var hand = (TurnIntoHand(splitLine[0]));
            var value = (int.Parse(splitLine[1]));
            var type = GetType(hand);
            hands.Add(new Hand(hand,value, type));
        }
        // Sort hands by type
        hands = hands.OrderBy(h => h.type).ToList();
        var groupedHands = hands
            .GroupBy(h => h.type)
            .Select(group => new
            {
                Type = group.Key,
                Hands = group.OrderBy(h => h.cards, new IntArrayComparer()).ToList()
            })
            .ToList();

        int totalWinnings = 0;
        int rankCounter = 0;
        foreach (var group in groupedHands)
        {
            Debug.Log($"Type: {group.Type} \n");
            foreach (var hand in group.Hands)
            {
                // Output or process each hand
                Debug.Log($"Hand: [{string.Join(", ", hand.cards)}]");
                rankCounter++;
                totalWinnings += rankCounter * hand.value;
            }
        }

        Debug.Log($"total winnings: {totalWinnings}");
    }

    private int GetType(int[] hand)
    {
        if (FiveOfAKind(hand)) return 6;
        if (FourOfAKind(hand)) return 5;
        if (FullHouse(hand)) return 4;
        if (ThreeOfAKind(hand)) return 3;
        if (TwoPair(hand)) return 2;
        if (OnePair(hand)) return 1;
        return 0;
    }

    private bool FiveOfAKind(int[] hand)
    {
        int jokerCount = hand.Count(card => card == 1); 
        var cardCounts = hand.Where(card => card != 1).GroupBy(card => card).ToDictionary(g => g.Key, g => g.Count());
        int maxCount = cardCounts.Any() ? cardCounts.Max(kv => kv.Value) : 0;
        return (jokerCount + maxCount) >= 5;
    }

    private bool FourOfAKind(int[] hand)
    {
        int jokerCount = hand.Count(card => card == 1); 
        var cardCounts = hand.Where(card => card != 1).GroupBy(card => card).ToDictionary(g => g.Key, g => g.Count());
        int maxCount = cardCounts.Any() ? cardCounts.Max(kv => kv.Value) : 0;
        return (jokerCount + maxCount) >= 4;
    }
    
    private bool FullHouse(int[] hand)
    {
        int jokerCount = hand.Count(card => card == 1); // Assuming 0 represents a joker
        var counts = hand.Where(card => card != 1).GroupBy(card => card).ToDictionary(g => g.Key, g => g.Count());
        bool hasThreeOfAKind = counts.Any(kv => kv.Value + jokerCount >= 3);
        bool hasPair = counts.Any(kv => kv.Value + (kv.Value + jokerCount >= 3 ? 0 : jokerCount) >= 2);
        return hasThreeOfAKind && hasPair;
    }
    
    private bool ThreeOfAKind(int[] hand)
    {
        int jokerCount = hand.Count(card => card == 1); 
        var cardCounts = hand.Where(card => card != 1).GroupBy(card => card).ToDictionary(g => g.Key, g => g.Count());
        int maxCount = cardCounts.Any() ? cardCounts.Max(kv => kv.Value) : 0;
        return (jokerCount + maxCount) >= 3;
    }

    private bool TwoPair(int[] hand)
    {
        int jokerCount = hand.Count(card => card == 1); 
        var counts = hand.Where(card => card != 1).GroupBy(card => card).ToDictionary(g => g.Key, g => g.Count());
        int pairs = counts.Count(kv => kv.Value >= 2);
        pairs += jokerCount;
        return pairs >= 2;
    }

    private bool OnePair(int[] hand)
    {
        int jokerCount = hand.Count(card => card == 1); 
        var counts = hand.Where(card => card != 1).GroupBy(card => card).ToDictionary(g => g.Key, g => g.Count());
        int pairs = counts.Count(kv => kv.Value >= 2);
        pairs += Math.Min(jokerCount, 1);
        return pairs >= 1;
    }

    private int[] TurnIntoHand(string inp)
    {
        var hand = new int[5];
        var inpChars = inp.ToCharArray();
        for (int i = 0; i < inpChars.Length; i++)
        {
            hand[i] = inpChars[i] switch
            {
                'T' => 10,
                'Q' => 11,
                'K' => 12,  
                'A' => 13,
                'J' => 1,
                _ => int.Parse(inpChars[i].ToString())
            };
        }

        return hand;
    }
    
}
