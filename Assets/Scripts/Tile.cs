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
        this.value = Random.Range(1,6);
        Debug.Log(value);
        this.sprite = Resources.Load<Sprite>($"images/fruit{value}");
        Debug.Log(sprite);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite;
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Debug.Log(spriteRenderer.bounds.size.x);
        Debug.Log(spriteRenderer.bounds.size.y);
        this.transform.position = new Vector3(
            this.transform.position.x*spriteRenderer.bounds.size.x,
            this.transform.position.y*spriteRenderer.bounds.size.y,
            0);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
