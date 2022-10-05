using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[,] board;
    public int boardSize = 6;

    void Start()
    {
        board = new GameObject[boardSize, boardSize];

        GameObject tile = (GameObject)Resources.Load("prefabs/Tile", typeof(GameObject));

        //Generate Tiles
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Instantiate(
                    tile, 
                    new Vector3(0 + i, 0 + j, 0), 
                    Quaternion.identity
                );
            }
        }
    }

}
