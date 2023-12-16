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
        //Starts at bottom left, then adds new item to each row flowing up, then new column.
        for (int i = 0; i < 1; i++)
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

                //tileComponent.addSprite(Random.Range(1, 1));
                tileComponent.initializeTile(j, i);
                board[i, j] = newTile;
                // Debug.Log("board state" + board[i, j]);
            }
        }
        board[0, 0].GetComponent<SpriteRenderer>().color = Color.blue;


        //StartCoroutine(DebugBoardState(2f));
    }

    IEnumerator DebugBoardState(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        this.checkForMatches();
    }
    //Swaps two passed tiles positions, reallocates neighbors
    public void swapTiles(GameObject tileOne, GameObject tileTwo)
    {
        Vector3 tileOnePos = tileOne.transform.position;
        Vector3 tileTwoPos = tileTwo.transform.position;

        tileOne.transform.position = tileTwoPos;
        tileTwo.transform.position = tileOnePos;

        Tile tileOneComp = tileOne.GetComponent<Tile>();
        Tile tileTwoComp = tileTwo.GetComponent<Tile>();

        // Debug.Log(string.Format("tileOne coords: {0} {1}", tileOneComp.row, tileOneComp.col));
        // Debug.Log(string.Format("tileTwo coords: {0} {1}", tileTwoComp.row, tileTwoComp.col));

        //update tiles
        (tileOneComp.row, tileTwoComp.row) = (tileTwoComp.row, tileOneComp.row);
        (tileOneComp.col, tileTwoComp.col) = (tileTwoComp.col, tileOneComp.col);

        //update board state
        this.board[tileOneComp.row, tileOneComp.col] = tileOne;
        this.board[tileTwoComp.row, tileTwoComp.col] = tileTwo;

        // Debug.Log(string.Format("Post Swap tileOne coords: {0} {1}", tileOneComp.row, tileOneComp.col));
        // Debug.Log(string.Format("Post Swap tileTwo coords: {0} {1}", tileTwoComp.row, tileTwoComp.col));
    }

    /** Controller function to check for tiles matches on 3 or more in horizontal 
        and vertical directions
    */
    public void checkForMatches()
    {
        HashSet<GameObject> matchedTiles = new HashSet<GameObject>();

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                List<GameObject> matchingTiles = this.checkNeighbors(i, j);
                matchedTiles.UnionWith(matchingTiles);
            }
        }

        foreach (GameObject tile in matchedTiles)
        {
            // Debug.Log("Tile Value: " + tile.GetComponent<Tile>().value);
            tile.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        // Debug.Log("MatchedTiles Length: " + matchedTiles.Count);
    }

    /**
        Given set of 2 dimensional array indices, test that the next 2 neighboring tiles
        have the same value. Append matching tiles to list, and return the list
    */
    private List<GameObject> checkNeighbors(int row, int col)
    {
        GameObject currentTile = board[row, col];
        int currTileValue = board[row, col].GetComponent<Tile>().value;
        List<GameObject> matchingTiles = new List<GameObject>();

        List<List<GameObject>> neighbors = getTileNeighbors(row, col);

        foreach (List<GameObject> neighborSet in neighbors)
        {
            if (neighborSet.Count >= 2)
            {
                int value1 = neighborSet[0].GetComponent<Tile>().value;
                int value2 = neighborSet[1].GetComponent<Tile>().value;

                // Debug.Log("1,2,3 " + value1 + " " + value2 + " " + currTileValue);

                if (value1 == currTileValue && value2 == currTileValue)
                {
                    foreach (GameObject neighbor in neighborSet)
                    {
                        // Debug.Log("num neighbors " + neighborSet.Count);
                        matchingTiles.Add(neighbor);
                    }
                    matchingTiles.Add(currentTile);
                }
            }
        }
        return matchingTiles;
    }

    /**
        takes int row and col,
        returns List of neighbors up to 2 spaces away and within bounds
    */
    private List<List<GameObject>> getTileNeighbors(int row, int col)
    {
        List<List<GameObject>> neighbors = new List<List<GameObject>>
        {
            new List<GameObject>{},
            new List<GameObject>{}
        };

        //test rows
        if (!(row + 1 >= boardSize))
        {
            neighbors[0].Add(board[row + 1, col]);
        }
        if (!(row + 2 >= boardSize))
        {
            neighbors[0].Add(board[row + 2, col]);
        }

        //test cols
        if (!(col + 1 >= boardSize))
        {
            neighbors[1].Add(board[row, col + 1]);

        }
        if (!(col + 2 >= boardSize))
        {
            neighbors[1].Add(board[row, col + 2]);
        }

        return neighbors;
    }
}
