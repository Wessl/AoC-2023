using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class day10part2 : MonoBehaviour
{
    // i dont know what im doing anymore :D
    // my brain right now: https://www.youtube.com/watch?v=dvhhYjwjUGk&list=PLBO2h-GzDvIYkZ0-nDMJ9WnN3Qrr5ZZeB&index=14
    public class Marker
    {
        public char dir;
        
        public Tuple<int, int> pos;
        public bool partOfPath;
        public bool insideSnake;

        public Marker(char dir, Tuple<int,int> pos, bool partOfPath, bool insideSnake)
        {
            this.dir = dir;
            this.pos = pos;
            this.partOfPath = partOfPath;
            this.insideSnake = insideSnake;
        }
    }
    [TextArea] public string input;

    private char[,] field;
    private Marker[,] covered;

    private List<Marker> snakepositions = new List<Marker>();
    void Start()
    {
        field = new char[input.Split("\r\n").Length, input.Split("\r\n")[0].Length];
        covered = new Marker[input.Split("\r\n").Length, input.Split("\r\n")[0].Length];
        var inps = input.Split("\r\n");
        for (var row = 0; row < inps.Length; row++)
        {
            var inp = inps[row].ToCharArray();
            for (int column = 0; column < inp.Length; column++)
            {
                field[row, column] = inp[column];
                covered[row,column] = new Marker('_', new Tuple<int,int>(row,column), false, false);
            }
        }
        // Find start! 
        Tuple<int, int> pos = new Tuple<int, int>(0,0);
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (field[i, j] == 'S') pos = new Tuple<int, int>(i+1, j);
            }
        }

        ProcessConnections(pos);
        
        // okay so we have a good thingy. now lets through each marked one, and mark those to its side. 
        int snakePosOddMod = 1;
        foreach (var m in snakepositions)
        {
            Debug.Log($"now trying position {m.pos.ToString()}");
            if (m.dir == 'u')
            {
                LookRight(m);
                // we have a direction, now, if we are a corner, we should find out what we cover. 
                if (field[m.pos.Item1, m.pos.Item2] == 'J')
                {
                    LookDown(m);
                }
                if (field[m.pos.Item1, m.pos.Item2] == '7')
                {
                    LookUp(m);
                }
            } else if (m.dir == 'r')
            {
                LookDown(m);
                if (field[m.pos.Item1, m.pos.Item2] == 'L')
                {
                    LookLeft(m);
                }
                if (field[m.pos.Item1, m.pos.Item2] == 'J')
                {
                    LookRight(m);
                }
            }
            else if (m.dir == 'd')
            {
                LookLeft(m);
                if (field[m.pos.Item1, m.pos.Item2] == 'F')
                {
                    LookUp(m);
                }
                if (field[m.pos.Item1, m.pos.Item2] == 'L')
                {
                    LookDown(m);
                }
            }else if (m.dir == 'l')
            {
                LookUp(m);
                if (field[m.pos.Item1, m.pos.Item2] == 'F')
                {
                    LookLeft(m);
                }
                if (field[m.pos.Item1, m.pos.Item2] == '7')
                {
                    LookRight(m);
                }
            }
        }
        //dir
        for (int i = 0; i < covered.GetLength(0); i++)
        {
            string str1 = "";
            for (int j = 0; j < covered.GetLength(1); j++)
            {
                str1 += covered[i, j].dir;
            }
            Debug.Log(str1);
        }

        Debug.Log(" ");
        
        //now we have the edges of interiors filled! yay. now we just have to, keep filling. 
        for (int i = 0; i < coveredInitialPos.Count; i++)
        {
            // if my neighbour is... blank, that is, neither path nor filled already, fill it! 
            LookDown(coveredInitialPos[i]);
            LookRight(coveredInitialPos[i]);
            LookLeft(coveredInitialPos[i]);
            LookUp(coveredInitialPos[i]);
        }
        string str = "";
        for (int i = 0; i < covered.GetLength(0); i++)
        {
            for (int j = 0; j < covered.GetLength(1); j++)
            {
                if (field[i, j] == 'S')
                {
                    str += 'S';
                }
                else if (covered[i, j].insideSnake)
                {
                    str += 'I';
                } 
                else
                    str += covered[i, j].partOfPath ? 'o' : '.';
            }

            str += "\n";
        }

        Debug.Log(str);
        Debug.Log(counter);
        
    }

    private List<Marker> coveredInitialPos = new List<Marker>();
    private int counter = 0;
    void LookRight(Marker m)
    {
        if (GetAdjacentCovered(new Tuple<int, int>(m.pos.Item1, m.pos.Item2 + 1)))
        {
            covered[m.pos.Item1, m.pos.Item2 + 1].insideSnake = true;
            coveredInitialPos.Add(covered[m.pos.Item1, m.pos.Item2 + 1]);
            counter++;
        }
    }

    void LookDown(Marker m)
    {
        if (GetAdjacentCovered(new Tuple<int, int>(m.pos.Item1 + 1, m.pos.Item2)))
        {
            covered[m.pos.Item1 + 1, m.pos.Item2].insideSnake = true;
            coveredInitialPos.Add(covered[m.pos.Item1 + 1, m.pos.Item2]);
            counter++;

        }
    }

    void LookLeft(Marker m)
    {
        if (GetAdjacentCovered(new Tuple<int, int>(m.pos.Item1, m.pos.Item2 + -1)))
        {
            covered[m.pos.Item1, m.pos.Item2 + -1 ].insideSnake = true;
            coveredInitialPos.Add(covered[m.pos.Item1, m.pos.Item2 + -1 ]);
            counter++;
        }
    }

    void LookUp(Marker m)
    {
        if (GetAdjacentCovered(new Tuple<int, int>(m.pos.Item1 + -1, m.pos.Item2)))
        {
            covered[m.pos.Item1 + -1, m.pos.Item2].insideSnake = true;
            coveredInitialPos.Add(covered[m.pos.Item1 + -1, m.pos.Item2]);
            counter++;
        }
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

            if (covered[pos.Item1, pos.Item2].partOfPath == true) continue;
            if (me == 'S' && currConnections != 0) continue;

           
            currConnections++;
            Debug.Log(currConnections);

            // Check and enqueue adjacent positions
            EnqueueIfConnected(pos.Item1, pos.Item2 - 1, me, 'l', positionsToProcess, pos.Item1, pos.Item2); // Left
            EnqueueIfConnected(pos.Item1 - 1, pos.Item2, me, 'u', positionsToProcess, pos.Item1, pos.Item2); // Up
            EnqueueIfConnected(pos.Item1, pos.Item2 + 1, me, 'r', positionsToProcess, pos.Item1, pos.Item2); // Right
            EnqueueIfConnected(pos.Item1 + 1, pos.Item2, me, 'd', positionsToProcess, pos.Item1, pos.Item2); // Down
        }
    }

    void EnqueueIfConnected(int row, int col, char me, char direction, Queue<Tuple<int, int>> queue, int oldRow, int oldCol)
    {
        Tuple<int, int> newPos = new Tuple<int, int>(row, col);
        char adjacent = GetAdjacent(newPos);
        if (IsConnected(me, adjacent, direction) && adjacent != 'S' && !covered[newPos.Item1, newPos.Item2].partOfPath)
        {
            Marker marker = new Marker(direction, newPos, true, false);
            covered[oldRow, oldCol] = marker;
            snakepositions.Add(marker);
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
    bool GetAdjacentCovered(Tuple<int,int> pos)
    {
        int row = pos.Item1;
        int col = pos.Item2;
        if (row >= 0 && row < covered.GetLength(0) && col >= 0 && col < covered.GetLength(1))
        {
            return (!covered[row, col].partOfPath ) && (!covered[row,col].insideSnake && (field[row,col] != 'S'));
        }
        return false;
    }
}
