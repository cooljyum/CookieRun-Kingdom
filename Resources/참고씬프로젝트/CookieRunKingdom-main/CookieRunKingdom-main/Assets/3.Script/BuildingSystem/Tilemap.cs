using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    // 위치
    public float x { get; private set; }
    public float y { get; private set; }

    // Order in Layer
    public int order { get; private set; }

    // 비어있는지
    public bool isEmpty = true;


    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // tilemap 초기화
    public void SetInfo(float x, float y, int order)
    {
        this.x = x;
        this.y = y;
        this.order = order;
        isEmpty = true;

        transform.localPosition = new Vector3(x, y, 0);
        transform.localRotation = Quaternion.identity;
        spriteRenderer.sortingOrder = -401;
    }

    // sprite 초기화
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void Visualize()
    {
        if (isEmpty)
            spriteRenderer.color = Color.blue;
        else
            spriteRenderer.color = Color.red;
    }
}
