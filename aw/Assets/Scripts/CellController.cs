using UnityEngine;
using System.Collections;

public class CellController : MonoBehaviour {

    [HideInInspector]
    public Color cellColor;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        cellColor = sprite.color;
    }

    public void ShowColor()
    {
        sprite.color = Color.green;
    }

    public void ReturnColor()
    {
        sprite.color = cellColor;
    }
}
