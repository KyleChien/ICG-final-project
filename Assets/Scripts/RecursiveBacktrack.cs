using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    left = 0,
    up = 1,
    right = 2,
    down = 3,
}

public class RecursiveBacktrack : MonoBehaviour
{
    // recursive backtrack, create a boolean matrix for each maze block

    public int maxRows;
    public int maxColumns;
    public MazeBlock[,] m_MazeBlocks;

    public RecursiveBacktrack(int rows, int columns)
    {
        maxRows = rows;
        maxColumns = columns;

        if (rows<=0 || columns <= 0)
        {
            Debug.Log("rows or columns out of range !");
            throw new System.ArgumentOutOfRangeException();
        }

        // generate MazeBlocks
        m_MazeBlocks = new MazeBlock[rows, columns];
        for(int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                m_MazeBlocks[row, col] = new MazeBlock();
            }
        }
    }

    public void exec(int row, int col)
    {
        // recursive backtrack
        m_MazeBlocks[row, col].IsVisted = true;
        bool DONE = false;
        while (!DONE)
        {
            // get ambient status
            List<int> AvailableMove = new List<int>();

            if (col - 1 >= 0 && !m_MazeBlocks[row, col - 1].IsVisted)
                AvailableMove.Add((int)Direction.left);
            if (row + 1 < maxRows && !m_MazeBlocks[row + 1, col].IsVisted)
                AvailableMove.Add((int)Direction.up);
            if (col + 1 < maxColumns && !m_MazeBlocks[row, col + 1].IsVisted)
                AvailableMove.Add((int)Direction.right);
            if (row - 1 >= 0 && !m_MazeBlocks[row - 1, col].IsVisted)
                AvailableMove.Add((int)Direction.down);

            if (AvailableMove.Count != 0)
            {
                var move = AvailableMove[Random.Range(0, AvailableMove.Count)];
                switch (move)
                {
                    case (int)Direction.left:
                        m_MazeBlocks[row, col].WallLeft = false;
                        m_MazeBlocks[row, col-1].WallRight = false;
                        exec(row, col - 1);
                        break;
                    case (int)Direction.up:
                        m_MazeBlocks[row, col].WallUp = false;
                        m_MazeBlocks[row+1, col].WallDown = false;
                        exec(row + 1, col);
                        break;
                    case (int)Direction.right:
                        m_MazeBlocks[row, col].WallRight = false;
                        m_MazeBlocks[row, col+1].WallLeft = false;
                        exec(row, col + 1);
                        break;
                    case (int)Direction.down:
                        m_MazeBlocks[row, col].WallDown = false;
                        m_MazeBlocks[row-1, col].WallUp = false;
                        exec(row - 1, col);
                        break;
                }
            }
            else
            {
                DONE = true;
                //Debug.Log(string.Format("coordination ({0}, {1}) DONE !", coord.x, coord.y));
            }
            
        }

    }
}
