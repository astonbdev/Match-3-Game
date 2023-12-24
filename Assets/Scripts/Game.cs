using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    List<Tile> selectedTiles = new List<Tile>();

    //TODO: Update this so we instantiate the board here rather than in the GUI
    public Board board;

    //TODO: Turn this into a getter setter
    public bool processing = false;
    public int Score = 0;

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

    private void testClickedTiles()
    {
        Tile tileOneComp = selectedTiles[0];
        Tile tileTwoComp = selectedTiles[1];

        //Check that the tiles are adjacent in a cardinal direction
        if (tileOneComp.testNeighbor(tileTwoComp.row, tileTwoComp.col))
        {
            //swap tiles
            this.board.swapTiles(tileOne, tileTwo);
            this.board.checkForMatches();
        }
        ;

        //Done with checks, clear the selected tiles
    }
}
