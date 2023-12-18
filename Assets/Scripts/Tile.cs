using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using DG.Tweening;
//TODO: This should probably just be a presentational component. The core logic
//ought to live as a multidimentional array in game, and let game handle swapping
//these presentational game pieces. Too much abstraction about the tile into
//one class

public class Tile : MonoBehaviour
{
    public struct Directions
    {
        public int north;
        public int south;
        public int east;
        public int west;
        public Directions(int row, int col)
        {
            north = col + 1;
            south = col - 1;
            east = row + 1;
            west = row - 1;
        }

        public override string ToString() => $@"
            north: {north}
            south: {south}
            east: {east}
            west: {west}";
    }
    public Sprite sprite; // tile(Tile).direcrtions
    public int value;
    public int row;
    public int col;
    public Directions directions;
    GameObject game;
    public Vector3 GetTilePosition()
    {
        return this.transform.position;
    }
    /**
        initializes the tile state, giving it a random fruit sprite and 
        setting it's neighbors
*/
    public void initializeTile(int row, int col)
    {
        // Debug.Log("initializeTile" + " " + row + col);
        this.row = row;
        this.col = col;
        this.directions = new Directions(this.row, this.col);

        game = GameObject.Find("Game");

        this.value = Random.Range(1, 4);
        addSprite(this.value);

        SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
        this.transform.position = new Vector3(
            this.transform.position.x * sprite.bounds.size.x,
            this.transform.position.y * sprite.bounds.size.y
        );
        // this.transform.DOMoveX(100, 1);
    }

    /**
        Updates the directions of the current tile and the corresponding row
        and col.
    */
    public void updateDirections(int row, int col)
    {
        this.directions = new Directions(row, col);
    }

    /**
    *   retrieves sprite resource and adds it to this components SpriteRenderer
    **/
    public void addSprite(int spriteFruitIcon)
    {
        this.sprite = Resources.Load<Sprite>($"images/fruit{spriteFruitIcon}");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite;
    }


    /** 
        On mouse down, passes this gameObject instance to the Game class for
        processing
    */
    void OnMouseDown()
    {
        Debug.Log("Clicked");
        Debug.Log($"row:{row} col:{col}");
        game.GetComponent<Game>().addClickedTile(this.gameObject);
    }

    /** returns bool if given coords are neighboring this tile horizontally or 
    *   vertically
*/
    public bool testNeighbor(int targetRow, int targetCol)
    {
        Debug.Log($"targetCoords:{targetRow}-{targetCol}");
        Debug.Log($"{this.directions}");
        // fail fast, cannot be the same tile
        if (this.row == targetRow && this.col == targetCol)
        {
            return false;
        }

        //since we have tested that the tile is NOT the same tile, we can now test
        //that the row or column is any of the associated cardinal directions
        bool inVertRange = (
            (targetCol == this.directions.south)
            || (targetCol == this.directions.north)
        );

        bool inHorizRange = (
            (targetRow == this.directions.west)
            || (targetRow == this.directions.east)
        );

        //If both cases are true, it's a corner, which is an invalid move
        if (inVertRange && inHorizRange) return false;
        return inVertRange || inHorizRange;

    }

    private void RunScoreAnimation()
    {
        //TODO: Implement this, get the tiles to wiggle and then delete
    }
}
