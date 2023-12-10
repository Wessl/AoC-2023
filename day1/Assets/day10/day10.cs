using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class day10 : MonoBehaviour
{
    [TextArea] public string input;

    private char[,] field;
    private char[,] covered;
    // Start is called before the first frame update
    void Start()
    {
        field = new char[input.Split("\r\n").Length, input.Split("\r\n")[0].Length];
        covered = new char[input.Split("\r\n").Length, input.Split("\r\n")[0].Length];
        var inps = input.Split("\r\n");
        for (var row = 0; row < inps.Length; row++)
        {
            var inp = inps[row].ToCharArray();
            for (int column = 0; column < inp.Length; column++)
            {
                field[row, column] = inp[column];
                covered[row,column] = '.';
            }

            var str = "";
            for (int column = 0; column < field.GetLength(1); column++)
            {
                str += field[row, column];
            }
            Debug.Log(str);
        }
        // Find start! 
        Tuple<int, int> pos = new Tuple<int, int>(0,0);
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == 'S') pos = new Tuple<int, int>(i, j);
            }
        }

        ProcessConnections(pos);
        // for part two: you have to always say which side is on "the inside" of the pipe
        // the pipe has a direction, depending on the circularity of it - if its left circular, take each tile to 
        // the left of my current direction (so not necessarily left.. could be up, down, right, depending on the way i am facing)
        // and then "flood fill" any adjacent tiles until you can't flood fill anymore. 
        
        // pick each position, flood fill, see if am on the "right or wrong" side of the pipes. 
    }
    
    private int currConnections = 0;
    
    void ProcessConnections(Tuple<int, int> initialPos)
    {
        Queue<Tuple<int, int>> positionsToProcess = new Queue<Tuple<int, int>>();
        positionsToProcess.Enqueue(initialPos);

        while (positionsToProcess.Count > 0)
        {
            var pos = positionsToProcess.Dequeue();
            var me = field[pos.Item1, pos.Item2];

            if (covered[pos.Item1, pos.Item2] != '.') continue;
            if (me == 'S' && currConnections != 0) continue;

            covered[pos.Item1, pos.Item2] = (char)currConnections;
            currConnections++;
            Debug.Log(currConnections);

            // Check and enqueue adjacent positions
            EnqueueIfConnected(pos.Item1, pos.Item2 - 1, me, 'l', positionsToProcess); // Left
            EnqueueIfConnected(pos.Item1 - 1, pos.Item2, me, 'u', positionsToProcess); // Up
            EnqueueIfConnected(pos.Item1, pos.Item2 + 1, me, 'r', positionsToProcess); // Right
            EnqueueIfConnected(pos.Item1 + 1, pos.Item2, me, 'd', positionsToProcess); // Down
        }
    }

    void EnqueueIfConnected(int x, int y, char me, char direction, Queue<Tuple<int, int>> queue)
    {
        Tuple<int, int> newPos = new Tuple<int, int>(x, y);
        char adjacent = GetAdjacent(newPos);

        if (IsConnected(me, adjacent, direction))
        {
            queue.Enqueue(newPos);
        }
    }

    
    

    bool IsConnected(char me, char you, char relationship)
    {
        // each char has four potential connecting points. up, down, left right. 
        // in order for two to be connected, the position and connecting points need to be considered
        // so for instance. you is up, then i need to have top, and you need to have bottom. 
        // or you is left, then i need to have left, and you need to have right. 
        if (relationship == 'u' && HasTop(me) && HasDown(you))
        {
            return true;
        }

        if (relationship == 'l' && HasLeft(me) && HasRight(you))
        {
            return true;
        }

        if (relationship == 'd' && HasDown(me) && HasTop(you))
        {
            return true;
        }

        if (relationship == 'r' && HasRight(me) && HasLeft(you))
        {
            return true;
        }
        return false;
    }

    bool HasTop(char c)
    {
        return (c == 'L' || c == '|' || c == 'J' || c == 'S');
    }
    bool HasLeft(char c)
    {
        return (c == '7' || c == '-' || c == 'J' || c == 'S');
    }
    bool HasDown(char c)
    {
        return (c == '7' || c == '|' || c == 'F' || c == 'S');
    }
    bool HasRight(char c)
    {
        return (c == 'L' || c == '-' || c == 'F' || c == 'S');
    }

    char GetAdjacent(Tuple<int,int> pos)
    {
        int row = pos.Item1;
        int col = pos.Item2;
        if (row >= 0 && row < field.GetLength(0) && col >= 0 && col < field.GetLength(1))
        {
            return field[row, col];
        }

        return 'X';
    }
}
