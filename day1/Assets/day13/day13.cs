using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class day13 : MonoBehaviour
{
    [TextArea] public string input;
    // Start is called before the first frame update
    void Start()
    {
        // I am not considering the splitting of each pattern... thats it. 
        var lines = input.Split("\r\n");
        char[,] horizontal = new char[lines.Length, lines[0].Length];

        Debug.Log($"Original Array Dimensions: {lines.Length} x {lines[0].Length}");

        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            for (int j = 0; j < lines[index].Length; j++)
            {
                horizontal[index, j] = line[j];
            }
        }
        PrintJagged(horizontal);
        var vertical = TransposeRowsAndColumns(horizontal);
    
        Debug.Log($"Transposed Array Dimensions: {vertical.GetLength(0)} x {vertical.GetLength(1)}");
        PrintJagged(vertical);
    }


    void PrintJagged(char[,] jagged)
    {
        string str = "";
        for (int i = 0; i < jagged.GetLength(0); i++)
        {
            for (int j = 0; j < jagged.GetLength(1); j++)
            {
                str += jagged[i, j];
                Debug.Log(jagged[i,j]);
            }

            str += "\n";
        }

        Debug.Log(str);
    }

    private char[,] TransposeRowsAndColumns(char[,] arr)
    {
        var newArray = new char[arr.GetLength(1), arr.GetLength(0)];
        string str = "";
        for (int i = 0; i < newArray.GetLength(0); i++)
        {
            for (int j = 0; j < newArray.GetLength(1); j++)
            {
                newArray[i, j] = arr[j, i];
                str += newArray[i, j];
            }
            str += "\n";
        }

        Debug.Log(str);
        return newArray;
    }

}
