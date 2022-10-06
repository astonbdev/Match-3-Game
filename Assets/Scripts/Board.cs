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

        StartCoroutine(DebugBoardState(2f));
    }

    IEnumerator DebugBoardState(float waitTime){
        yield return new WaitForSeconds(waitTime);

        this.checkForMatches();
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

    private void checkForMatches()
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

        foreach(GameObject tile in matchedTiles){
            Debug.Log("Tile Value: " + tile.GetComponent<Tile>().value);
            tile.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        Debug.Log("MatchedTiles Length: " + matchedTiles.Count);
    }

    private List<GameObject> checkNeighbors(int row, int col)
    {
        GameObject currentTile = board[row, col];
        int currTileValue = board[row, col].GetComponent<Tile>().value;
        List<GameObject> matchingTiles = new List<GameObject>();

        List<List<GameObject>> neighbors = getTileNeighbors(row, col);

        if(neighbors[0].Count != 0){
            foreach(List<GameObject> neighborSet in neighbors){
                int value1 = neighborSet[0].GetComponent<Tile>().value;
                int value2 = neighborSet[1].GetComponent<Tile>().value;

                if(value1 == currTileValue && value2 == currTileValue){
                    foreach(GameObject neighbor in neighborSet){
                        matchingTiles.Add(neighbor);
                    }
                }
            }
        }

        return matchingTiles;
    }

    /**
        takes int row and col,
        returns List of neighbors in board if within bounds
    */
    private List<List<GameObject>> getTileNeighbors(int row, int col){
        List<List<GameObject>> neighbors = new List<List<GameObject>>
        {
            new List<GameObject>{}, 
            new List<GameObject>{}
        };

        bool neighbor1 = !((row+2 >= boardSize) || (col+2 >= boardSize));
        bool neighbor2 = !((row+1 >= boardSize) || (col+1 >= boardSize));

        if(neighbor1 && neighbor2){
            //add southern neighbors
            Debug.Log("row/col: " + row + " " + col);
            neighbors[0].Add(board[row + 1, col]);
            neighbors[1].Add(board[row + 2, col]);
            //add eastern neighbors
            neighbors[0].Add(board[row, col + 1]);
            neighbors[1].Add(board[row, col + 2]);
        }

        return neighbors;
    }



}
