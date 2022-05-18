using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject [,] board;
    void Start()
    {
        board = new GameObject [6,6];

        GameObject tile = (GameObject)Resources.Load("prefabs/Tile", typeof(GameObject));

        for(int i=0; i < 6; i++){
            for(int j=0; j < 6; j++){
                board[i,j] = Instantiate(tile, new Vector3(0+i,0+j,0), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
