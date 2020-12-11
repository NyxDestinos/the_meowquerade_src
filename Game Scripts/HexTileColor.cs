using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileColor : MonoBehaviour
{
    private SpriteRenderer rend;
    public Color hoverColor;
    public Color moveableColor;
    private Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        HexTile _parent = transform.parent.GetComponent<HexTile>();
        if (!_parent.isTurn) rend.material.color = startColor;
        else rend.material.color = moveableColor;
    }

    private void OnMouseDown()
    {
        HexTile _parent = transform.parent.GetComponent<HexTile>();


        if (GameManager.isCurrentTurn && !GameManager.isMove && _parent.isTurn)
        {
            GameManager.players[Client.instance.myId].getNextPoint(_parent.x, _parent.y, transform.position);
            ClientSend.SendPlayerPosition(_parent.x, _parent.y);
            ArenaUIManager.instance.SetTileEndTurn();
            GameManager.isMove = true;
        }

    }

    public void setMoveableColor()
    {
        rend.material.color = moveableColor;
    }

    public void setStartColor()
    {
        rend.material.color = startColor;
    }
}
