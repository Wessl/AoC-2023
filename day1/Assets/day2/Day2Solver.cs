using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Day2Solver : MonoBehaviour
{
    private const int RED = 12;
    private const int GREEN = 13;
    private const int BLUE = 14;
    
    [TextArea]
    [SerializeField] private string input;

    [SerializeField] private TextMeshProUGUI tmproLines;
    [SerializeField] private TextMeshProUGUI tmproTotal;
    
    // Start is called before the first frame update
    void Start()
    {
        var lines = input.Split("\n");
        Debug.Log(input);
        Debug.Log(lines);
        Debug.Log(lines[0]);
        StartCoroutine(CheckGames(lines));
    }

    IEnumerator CheckGames(string[] lines)
    {
        int gamesWonIdSum = 0;
        int power = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            int gameIndex = int.Parse(line.Split(" ")[1].Split(":")[0]);
            string[] rounds = line.Split(":")[1].Split(";");
            int maxRed = 0;
            int maxGreen = 0;
            int maxBlue = 0;
            bool possible = true;
            for (int j = 0; j < rounds.Length; j++)
            {
                
                int red = 0;
                int green = 0;
                int blue = 0;
                
                var roundColors = rounds[j].Split(",");
                foreach (var roundColor in roundColors)
                {
                    GetColorAndAmount(roundColor, out string color, out int amount);
                    switch (color)
                    {
                        case "red":
                            red += amount;
                            break;
                        case "green":
                            green += amount;
                            break;
                        case  "blue":
                            blue += amount;
                            break;
                        default:
                            Debug.Log("oopsie");
                            break;
                    }
                }

                if (red >= maxRed) maxRed = red;
                if (green >= maxGreen) maxGreen = green;
                if (blue >= maxBlue) maxBlue = blue;
                
            }

            power += maxRed * maxGreen * maxBlue;
            {
                tmproLines.text += lines[i] + "\n";
                gamesWonIdSum += gameIndex;
                //tmproTotal.text = gamesWonIdSum.ToString();
                tmproTotal.text = power.ToString();
            }

            
            
            yield return new WaitForEndOfFrame();
        }

        Debug.Log(gamesWonIdSum);
    }

    void GetColorAndAmount(string elem, out string color, out int amount)
    {
        var s = elem.Trim().Split(" ");
        color = s[1];
        Debug.Log(color);
        Debug.Log(s[0]);
        amount = int.Parse(s[0]);
    }
}
