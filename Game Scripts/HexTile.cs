using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public int x;
    public int y;
    public int z;

    public bool isTurn;

    public TextMesh gridText;
    public GameObject centerPoint;
    public HexTileColor hexPrefab;

    public Color hoverColor;
    private Color startColor;
    public SpriteRenderer rend;

    void Start()
    {
        startColor = rend.material.color;
        gridText.text = x + "," + y;
        //TextMesh text = Instantiate(gridText, gameObject.transform.position, gameObject.transform.rotation);
        //text.gameObject.transform.SetParent(gameObject.transform);
    }

    void OnMouseEnter()
    {
        Debug.Log(x + "," + y);
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }



}
