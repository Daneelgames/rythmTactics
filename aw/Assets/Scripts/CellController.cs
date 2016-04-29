using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CellController : MonoBehaviour, IPointerClickHandler {

    [HideInInspector]
    public Color cellColor;
    
    public UnitColor cellType;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        CardController card;
        if (cellType == UnitColor.Red)
            card = TurnManager.instance.selectedCardRed;
        else
            card = TurnManager.instance.selectedCardBlue;
        
        if (card == null) return;
        
        card.BuyUnit((Vector2)transform.position);
    }
}
