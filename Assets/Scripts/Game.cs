using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    List<GameObject> selectedTiles = new List<GameObject>();
    public GameObject board;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addClickedTile(GameObject tile){
        selectedTiles.Add(tile);

        Debug.Log("selectedTiles Count: " + selectedTiles.Count);

        if(selectedTiles.Count == 2){
            testClickedTiles();
            selectedTiles.Clear();
        }
    }

    void testClickedTiles(){
        Debug.Log("testClickedTiles");
        GameObject tileOne = selectedTiles[0];
        GameObject tileTwo = selectedTiles[1];
        
        Tile tileOneComp = selectedTiles[0].GetComponent<Tile>();
        Tile tileTwoComp = selectedTiles[1].GetComponent<Tile>();

        if(tileOneComp.testNeighbor(tileTwoComp.row, tileTwoComp.col)){
            this.board.GetComponent<Board>().swapTiles(tileOne, tileTwo);
            this.board.GetComponent<Board>().checkForMatches();
        };
    }
}
