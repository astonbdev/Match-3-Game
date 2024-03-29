using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject[,] board;
    public int boardSize = 6;
    public float tileFillOffset = 0;
    private Game game;

    // private float animTime = .175f;

    //TODO: Should make this private but I just want it to work right now
    public HashSet<GameObject> processingTiles;

    /**
        Renders the intial board state
    */
    void Start()
    {
        board = new GameObject[boardSize, boardSize];
        game = GameObject.Find("Game").GetComponent<Game>();

        GameObject tile = (GameObject)Resources.Load("prefabs/Tile", typeof(GameObject));

        //Generate Tiles
        //Starts at bottom left, then adds new item to each row flowing up, then new column.
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                Vector3 startingPos = new Vector3(col, row, 0);
                GameObject newTile = Instantiate(tile, startingPos, Quaternion.identity);

                Tile tileComponent = newTile.GetComponent<Tile>();

                tileComponent.initializeTile(row, col);
                board[row, col] = newTile;
            }
        }
    }

    /**
        For debugging
    */
    IEnumerator DebugBoardState(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        this.checkForMatches();
    }

    //Swaps two passed tiles positions, reallocates neighbors
    public void swapTiles(Tile tileOne, Tile tileTwo)
    {
        Vector3 tileOnePos = tileOne.gameObject.transform.position;
        Vector3 tileTwoPos = tileTwo.gameObject.transform.position;

        tileOne.gameObject.transform.position = tileTwoPos;
        tileTwo.gameObject.transform.position = tileOnePos;

        //update tiles
        (tileOne.row, tileTwo.row) = (tileTwo.row, tileOne.row);
        (tileOne.col, tileTwo.col) = (tileTwo.col, tileOne.col);

        tileOne.updateDirections(tileOne.row, tileOne.col);
        tileTwo.updateDirections(tileTwo.row, tileTwo.col);

        //update board state
        this.board[tileOne.row, tileOne.col] = tileOne.gameObject;
        this.board[tileTwo.row, tileTwo.col] = tileTwo.gameObject;
    }

    /**
        Controller function to check for tiles matches on 3 or more in horizontal
        and vertical directions
    */
    public void checkForMatches()
    {
        game.processing = true;
        HashSet<GameObject> matchedTiles = new HashSet<GameObject>();

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i, j] == null)
                    continue;
                List<GameObject> matchingTiles = this.checkNeighbors(i, j);
                matchedTiles.UnionWith(matchingTiles);
            }
        }

        this.processingTiles = matchedTiles;

        //ensure their are no matching tiles before
        if (matchedTiles.Count == 0)
        {
            this.game.processing = false;
            return;
        }

        //animate the tiles, score them, and then repopulate the board
        foreach (GameObject tile in matchedTiles)
        {
            tile.GetComponent<Tile>().RunScoreAnimation();
            game.ScoreTile();
        }

        Debug.Log("Done with processing scored tiles");
        StartCoroutine(this._waitThenCheckMatches());
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

        //grabs the vertical and horizontal neighbors, then compares their values
        //with respect to each direction
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
        returns List of neighbors up to 2 spaces away to the right and above the
        current tile
    */
    private List<List<GameObject>> getTileNeighbors(int row, int col)
    {
        List<List<GameObject>> neighbors = new List<List<GameObject>>
        {
            new List<GameObject>(),
            new List<GameObject>()
        };

        //test rows
        if (!(row + 1 >= boardSize) && (board[row + 1, col] != null))
        {
            neighbors[0].Add(board[row + 1, col]);
        }
        if (!(row + 2 >= boardSize) && (board[row + 2, col] != null))
        {
            neighbors[0].Add(board[row + 2, col]);
        }

        //test cols
        if (!(col + 1 >= boardSize) && (board[row, col + 1] != null))
        {
            neighbors[1].Add(board[row, col + 1]);
        }
        if (!(col + 2 >= boardSize) && (board[row, col + 2] != null))
        {
            neighbors[1].Add(board[row, col + 2]);
        }

        return neighbors;
    }

    private IEnumerator _waitThenCheckMatches()
    {
        while (game.processing)
        {
            yield return new WaitForSeconds(.5f);
            if (this.processingTiles.Count == 0)
            {
                game.processing = false;
            }
        }
        this.checkForMatches();
    }

    //FIXME: I want to keep this code for future reference, but it's unnecessary because I can just
    //create a lambda statement as a callback in the tile itself after the death animation runs
    // private Action GetTileFillCallback(int row, int col)
    // {
    //     return () =>
    //     {
    //         float offset = this.boardSize;
    //         Vector3 startingPos = new Vector3(col + offset, row + offset, 0);
    //         GameObject tile = (GameObject)Resources.Load("prefabs/Tile", typeof(GameObject));
    //         tile.GetComponent<Tile>().initializeTile(row, col);

    //         this.board[row, col] = Instantiate(tile, startingPos, Quaternion.identity);
    //     };
    // }
}
