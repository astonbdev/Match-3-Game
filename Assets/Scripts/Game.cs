using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    List<GameObject> selectedTiles = new List<GameObject>();

    //TODO: Update this so we instantiate the board here rather than in the GUI
    public GameObject board;

    //TODO: Turn this into a getter setter
    public bool processing = false;
    public int Score = 0;

    public void ScoreTile()
    {
        Score += 10;
    }

    public void AddClickedTile(GameObject tile)
    {
        if (processing)
            return;

        selectedTiles.Add(tile);

        // Debug.Log("selectedTiles Count: " + selectedTiles.Count);

        if (selectedTiles.Count == 2)
        {
            testClickedTiles();
            selectedTiles.Clear();
        }
    }

    private void testClickedTiles()
    {
        // Debug.Log("testClickedTiles");
        GameObject tileOne = selectedTiles[0];
        GameObject tileTwo = selectedTiles[1];

        Tile tileOneComp = selectedTiles[0].GetComponent<Tile>();
        Tile tileTwoComp = selectedTiles[1].GetComponent<Tile>();

        //Check that the tiles are adjacent in a cardinal direction
        if (tileOneComp.testNeighbor(tileTwoComp.row, tileTwoComp.col))
        {
            //swap tiles
            this.board.GetComponent<Board>().swapTiles(tileOne, tileTwo);
            this.board.GetComponent<Board>().checkForMatches();
        }
        ;
        //Done with checks, clear the selected tiles
    }
}
