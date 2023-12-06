using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Day4 : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string input;

    private int[] cardAmounts;

    private int total = 0;
    private void Start()
    {
        var cards = input.Split("\r\n");
        cardAmounts = new int[cards.Length];
        for (var index = 0; index < cardAmounts.Length; index++)
        {
            cardAmounts[index] = 1;
        }

        for (int i = 0; i < cards.Length; i++)
        {
            var card = cards[i];
            int matches = HandleCard(card);
            for (int k = 0; k < cardAmounts[i]; k++)
            {
                for (int j = 0; j < matches; j++)
                {
                    cardAmounts[i + j + 1]++;
                }
            }
            
        }

        for (var index = 0; index < cardAmounts.Length; index++)
        {
            var card = cardAmounts[index];
            Debug.Log($"card number {index+1} is {card} ");
            total += card;
        }

        Debug.Log("total: " + total);
    }

    private int HandleCard(string card)
    {
        int[] winningNumbers = ExtractWinningNumbers(card);
        int[] myNumbers = ExtractMyNumbers(card);
        int matches = 0;
        for (int i = 0; i < winningNumbers.Length; i++)
        {
            if (myNumbers.Contains(winningNumbers[i]))
            {
                matches++;
            }
        }

        return matches;
    }

    private int[] ExtractWinningNumbers(string card)
    {
        Debug.Log("card: " + card);
        var beforeSeparator= card.Split("|")[0];
        var numberString = beforeSeparator.Split(":")[1].Trim();
        var numbers = numberString.Split(" ");
        List<int> numbersList = new List<int>();
        foreach (var number in numbers)
        {
            if (number.Length == 0) continue;
            numbersList.Add(int.Parse(number));
        }

        return numbersList.ToArray();

    }

    private int[] ExtractMyNumbers(string card)
    {
        var afterSeparator = card.Split("|")[1].Trim();
        var numbers = afterSeparator.Split(" ");
        List<int> numbersList = new List<int>();
        foreach (var number in numbers)
        {
            if (number.Length == 0) continue;
            numbersList.Add(int.Parse(number));
        }

        return numbersList.ToArray();
    }
}
