using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
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
            north = row + 1;
            south = row - 1;
            east = col + 1;
            west = col - 1;
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

    /**
        initializes the tile state, giving it a random fruit sprite and 
        setting it's neighbors
*/
    public void initializeTile(int row, int col)
    {
        Debug.Log("initializeTile" + " " + row + col);
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
    }

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


    void OnMouseDown()
    {
        Debug.Log("Clicked");
        Debug.Log($"{directions}");
        game.GetComponent<Game>().addClickedTile(this.gameObject);
    }

    /** returns bool if given coords are neighboring this tile horizontally or 
    *   vertically
*/
    public bool testNeighbor(int targetRow, int targetCol)
    {
        Debug.Log($"targets: row-{targetRow}, col-{targetCol}");
        Debug.Log($"this tile: row-{this.row}, col-{this.col}");
        Debug.Log($"directions: vert-{this.directions.south}-{this.directions.north}");
        Debug.Log($"directions: horiz-{this.directions.west}-{this.directions.east}");



        // fail fast, cannot be the same tile
        if (this.row == targetRow && this.col == targetCol)
        {
            return false;
        }

        //since we have tested that the tile is NOT the same tile, we can now test
        //that the row or column is either of the associated cardinal directions
        bool inVertRange = (
            (targetCol == this.directions.south)
            || (targetCol == this.directions.north)
        );

        bool inHorizRange = (
            (targetRow == this.directions.west)
            || (targetCol == this.directions.east)
        );

        return inVertRange || inHorizRange;

    }
}
