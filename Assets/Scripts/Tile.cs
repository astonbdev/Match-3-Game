using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite;
    public int value;
    void Start()
    {
        this.value = Random.Range(0,6);
        Debug.Log(value);
        this.sprite = Resources.Load<Sprite>($"images/fruit{value}");
        Debug.Log(sprite);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
