using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[6];

    private void Start()
    {
        for(int i = 2; i<=7; i++){
            sprites[i-2] = Resources.Load(
                "images/free-fruit-vector_0{i}") as Sprite;
        }


        foreach(Sprite sprite in sprites){
            Debug.Log(sprite);
        }
    }
}
