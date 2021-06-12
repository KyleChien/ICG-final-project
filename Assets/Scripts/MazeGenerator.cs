using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    // instantiate wall and floor based on the RecursiveVacktrack results

    public int rows;
    public int columns;
    public float blockHeight;
    public float blockWidth;
    public GameObject floor;
    public GameObject wall;
    public GameObject pillar;

    private void Start()
    {
        RecursiveBacktrack RB =  new RecursiveBacktrack(rows, columns);
        RB.exec(0,0);

        for(int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = col * blockWidth;
                float z = row * blockHeight;
                MazeBlock block = RB.m_MazeBlocks[row, col];

                // floors
                Instantiate(floor, new Vector3(x, 0, z), Quaternion.identity);

                // walls
                if (block.WallLeft)
                    Instantiate(wall, new Vector3(x - blockWidth / 2, 0, z) + wall.transform.position, Quaternion.Euler(0, 270, 0));
                if (block.WallUp)
                    Instantiate(wall, new Vector3(x, 0, z + blockHeight / 2) + wall.transform.position, Quaternion.Euler(0, 0, 0));
                if (block.WallRight)
                    Instantiate(wall, new Vector3(x + blockWidth / 2, 0, z) + wall.transform.position, Quaternion.Euler(0, 90, 0));
                if (block.WallDown)
                    Instantiate(wall, new Vector3(x, 0, z - blockHeight / 2) + wall.transform.position, Quaternion.Euler(0, 180, 0));

                // pillars
                Instantiate(pillar, new Vector3(x - blockWidth / 2, 0, z - blockWidth / 2), Quaternion.identity);
                if (row + 1 >= rows)
                    Instantiate(pillar, new Vector3(x - blockWidth / 2, 0, z + blockWidth / 2), Quaternion.identity);
                if (col + 1 >= columns)
                    Instantiate(pillar, new Vector3(x + blockWidth / 2, 0, z - blockWidth / 2), Quaternion.identity);
                if (row + 1 >= rows && col + 1 >= columns)
                    Instantiate(pillar, new Vector3(x + blockWidth / 2, 0, z + blockWidth / 2), Quaternion.identity);
            }
        }
    }
}
