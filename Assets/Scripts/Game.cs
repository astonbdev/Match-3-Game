using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Board board;
    public int Score = 0;

    //TODO: Turn this into a getter setter
    public bool processing = false;
    List<Tile> selectedTiles = new List<Tile>();

    public void Start()
    {
        this.board = GameObject.Find("Board").GetComponent<Board>();
    }

    public void ScoreTile()
    {
        Score += 10;
    }

    /**
        Adds selected tiles to the tile tracker for controlling game inputs
    */
    public void AddClickedTile(GameObject tile)
    {
        if (processing)
            return;

        selectedTiles.Add(tile.GetComponent<Tile>());

        // Debug.Log("selectedTiles Count: " + selectedTiles.Count);

        if (selectedTiles.Count == 2)
        {
            testClickedTiles();
            selectedTiles.Clear();
        }
    }

    /**
        Tests for valid tile selection. If valid, swaps tiles and then
        checks for matches on the board.
    */
    private void testClickedTiles()
    {
        Tile tileOne = selectedTiles[0];
        Tile tileTwo = selectedTiles[1];

        //Check that the tiles are adjacent in a cardinal direction
        if (tileOne.testNeighbor(tileTwo.row, tileTwo.col))
        {
            //swap tiles
            this.board.swapTiles(tileOne, tileTwo);
            this.board.checkForMatches();
        }
        ;

        //Done with checks, clear the selected tiles
    }
}
