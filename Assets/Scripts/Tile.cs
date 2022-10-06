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
        
        addSprite(value);

        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.transform.position = new Vector3(
            this.transform.position.x * spriteRenderer.bounds.size.x,
            this.transform.position.y * spriteRenderer.bounds.size.y,
            0
        );
    }

    public void addSprite(int spriteValue)
    {
        this.sprite = Resources.Load<Sprite>($"images/fruit{value}");
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.sprite;
    }
}
