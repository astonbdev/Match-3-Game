using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    struct Directions
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
    }
    public Sprite sprite; // tile(Tile).direcrtions
    public int value;
    public int row;
    public int col;
    Directions directions;
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

        SpriteRenderer sprite= this.gameObject.GetComponent<SpriteRenderer>();
        this.transform.position = new Vector3(
            this.transform.position.x * sprite.bounds.size.x,
            this.transform.position.y * sprite.bounds.size.y
        );
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
        game.GetComponent<Game>().addClickedTile(this.gameObject);
    }

    /** returns bool if given coords are neighboring this tile horizontally or 
    *    vertically
    */
    public bool testNeighbor(int targetRow, int targetCol)
    {
        Debug.Log("testNeighbor: " + targetRow + " " + targetCol);
        Debug.Log("thisTile: " + this.row + " " + this.col);
        Debug.Log(
            string.Format(
                "directions: south {0} north {1} west {2} east {3} ",
                this.directions.south,
                this.directions.north,
                this.directions.west,
                this.directions.east
            )
        );
        // fail fast, cannot be the same tile
        if(this.row == targetRow && this.col == targetCol){
            return false;
        }

        bool inVertRange = (
            (targetCol >= this.directions.south) && (targetCol <= this.directions.north)
        );

        bool inHorizRange = (
            (targetRow >= this.directions.west) && (targetCol <= this.directions.east)
        );

        if (inVertRange && inHorizRange)
        {
            if (inVertRange && (this.row == targetRow))
            {
                return inVertRange;
            }
            if(inHorizRange && (this.col == targetCol))
            {
                return inHorizRange;
            }
        }

        return false;
    }
}
