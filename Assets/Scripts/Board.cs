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
        board = new int[boardSize, boardSize];

        GameObject tile = (GameObject)Resources.Load("prefabs/Tile", typeof(GameObject));

        //Generate Tiles
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Vector3 startingPos = new Vector3(0 + i, 0 + j, 0);
                GameObject newTile = Instantiate(
                    tile,
                    new Vector3(0 + i, 0 + j, 0),
                    Quaternion.identity
                );

                Tile tileComponent = newTile.GetComponent<Tile>();

                tileComponent.addSprite(Random.Range(1, 6));
                board[i, j] = newTile;
                Debug.Log("board state" + board[i, j]);
            }
        }
    }

    //Swaps two passed tiles positions, reallocates neighbors
    public void swapTiles(GameObject tileOne, GameObject tileTwo)
    {
        Vector3 tempPos1 = new Vector3(0, 0, 0);
        tempPos1 = tileOne.transform.position;
        Vector3 tempPos2 = new Vector3(0, 0, 0);
        tempPos2 = tileTwo.transform.position;

        //Change the position of each tile in game space
        //Debug.Log("Before fN: "+ tempPos1);
        tileOne.transform.position = tempPos2;
        tileTwo.transform.position = tempPos1;

        //checkForMatches();
    }

    private checkForMatches()
    {
        HashSet<GameObject> matchedTiles;
        //check each array of board
        (int, int)[] scoredTileIndices;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 1; j < boardSize; j++)
            {
                List<GameObject> matchingTiles = this.checkNeighbors(i, j);
                matchedTiles.UnionWith(matchingTiles);
            }
        }
    }

    private checkNeighbors(int row, int col)
    {
        GameObject currentTile = board[row][col];
        int tileValue;
        bool sameValue;
        List<GameObject> matchingTiles;
        //check southern neighbors
        if (!(row + 1 >= boardSize))
        {
            southTile = board[row + 1][col];

            currentTileValue = currentTile.GetComponent<Tile>().value();
            southTileValue = southTile.GetComponent<Tile>().value();

            if (currentTileValue == southTileValue)
            {
                matchingTiles.Add(currentTile);
                matchingTiles.Add(southTile);
            }
        }

        //check eastern neighbors
        if (!(col + 1 >= boardSize))
        {
            eastTile = board[row + 1][col];

            currentTileValue = currentTile.GetComponent<Tile>().value();
            eastTileValue = eastTile.GetComponent<Tile>().value();

            if (currentTileValue == eastTileValue)
            {
                matchingTiles.Add(currentTile);
                matchingTiles.Add(eastTile);
            }

        }

        return matchingTiles;
    }



}
