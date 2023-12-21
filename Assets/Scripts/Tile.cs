using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using DG.Tweening;
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
            north = col + 1;
            south = col - 1;
            east = row + 1;
            west = row - 1;
        }

        public override string ToString() =>
            $@"
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
        this.row = row;
        this.col = col;
        Debug.Log(col + "=" + row);

        this.directions = new Directions(this.row, this.col);
        this.value = Random.Range(1, 4);

        game = GameObject.Find("Game");

        addSprite(this.value);

        Debug.Log("new tile position: " + this.transform.position);

        SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
        this.transform.position = new Vector3(
            this.transform.position.x * sprite.bounds.size.x,
            this.transform.position.y * sprite.bounds.size.y
        );
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
        game.GetComponent<Game>().AddClickedTile(this.gameObject);
    }

    /** returns bool if given coords are neighboring this tile horizontally or
    *   vertically
    */
    public bool testNeighbor(int targetRow, int targetCol)
    {
        // fail fast, cannot be the same tile
        if (this.row == targetRow && this.col == targetCol)
        {
            return false;
        }

        //since we have tested that the tile is NOT the same tile, we can now test
        //that the row or column is any of the associated cardinal directions
        bool inVertRange = (
            (targetCol == this.directions.south) || (targetCol == this.directions.north)
        );

        bool inHorizRange = (
            (targetRow == this.directions.west) || (targetRow == this.directions.east)
        );

        //If both cases are true, it's a corner, which is an invalid move
        if (inVertRange && inHorizRange)
            return false;
        return inVertRange || inHorizRange;
    }

    /**
        runs the tweens to wiggle the Tile before it is destroyed.
    */
    public void RunScoreAnimation()
    {
        float animTime = .175f;

        Tween[] tweens =
        {
            this.transform.DORotate(new Vector3(0, 0, -45), animTime),
            this.transform.DORotate(new Vector3(0, 0, 0), animTime),
            this.transform.DORotate(new Vector3(0, 0, 45), animTime),
            this.transform.DORotate(new Vector3(0, 0, 0), animTime)
        };

        Sequence wiggleSequence = DOTween.Sequence();
        wiggleSequence.OnComplete(removeAndFillTile);

        foreach (Tween tween in tweens)
        {
            wiggleSequence.Append(tween);
        }

        wiggleSequence.SetLoops(2, LoopType.Restart);
    }

    /**
        Controller callback that destroys the current tile and recreates a new tile
        for the game board
    */
    private void removeAndFillTile()
    {
        Tile.GenerateFillTile(this.row, this.col);
        Destroy(gameObject);
    }

    /**
        generates a new tile for the gameboard, then removes the current tile
        from the scene
    */
    private static void GenerateFillTile(int row, int col)
    {
        Board gameBoard = GameObject.Find("Board").GetComponent<Board>();
        Vector3 startingPos = new Vector3(col, row, 0);

        GameObject tile = (GameObject)Resources.Load("prefabs/Tile", typeof(GameObject));
        gameBoard.board[row, col] = Instantiate(tile, startingPos, Quaternion.identity);
        gameBoard.board[row, col].transform.position = startingPos;
        tile.GetComponent<Tile>().initializeTile(row, col);
    }
}
