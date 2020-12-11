using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSprite : MonoBehaviour
{
    public Color hoverColor = new Color(1f, 0.5f, 0.5f, 1);
    public Color targetableColor = new Color(1f, 0.5f, 0.5f, 1);
    public Color startColor = new Color(1, 1, 1, 1);
    public GameObject prefabSkin;

    public void OnMouseEnter()
    {
        int _targetID = transform.parent.GetComponent<PlayerManager>().id;
        ArenaUIManager.instance.targetText.text = GameManager.players[_targetID].username;
        for (int i =0; i < prefabSkin.transform.childCount; i++)
        {
            GameObject _part = prefabSkin.transform.GetChild(i).gameObject;
            if (_part.GetComponent<SpriteRenderer>() != null) _part.GetComponent<SpriteRenderer>().color = hoverColor;
        }
    }

    public void OnMouseExit()
    {
        ArenaUIManager.instance.targetText.text = "";
        for (int i = 0; i < prefabSkin.transform.childCount; i++)
        {
            GameObject _part = prefabSkin.transform.GetChild(i).gameObject;
            if (_part.GetComponent<SpriteRenderer>() != null)
            {
                if (gameObject.transform.parent.GetComponent<PlayerManager>().isTargetable)
                {
                    _part.GetComponent<SpriteRenderer>().color = targetableColor;
                }
                else
                {
                    _part.GetComponent<SpriteRenderer>().color = startColor;
                }

            }
        }
    }

    public void OnMouseDown()
    {
        int _targetID = transform.parent.GetComponent<PlayerManager>().id;
        if (transform.parent.GetComponent<PlayerManager>().isTargetable)
        {
            GameManager.instance.UseCard(_targetID);
        }

        for (int i = 1; i <= GameManager.players.Count; i++)
        {
            GameObject _prefab = GameManager.players[i].playerPrefab.GetComponent<PlayerSprite>().prefabSkin;
            for (int j = 0; j < _prefab.transform.childCount; j++)
            {
                GameObject _part = _prefab.transform.GetChild(j).gameObject;
                if (_part.GetComponent<SpriteRenderer>() != null)
                {
                    _part.GetComponent<SpriteRenderer>().color = startColor;
                }
            }
        }
    }

    public void setTargetable()
    {
        for (int i = 0; i < prefabSkin.transform.childCount; i++)
        {
            GameObject _part = prefabSkin.transform.GetChild(i).gameObject;
            if (_part.GetComponent<SpriteRenderer>() != null)
            {
                _part.GetComponent<SpriteRenderer>().color = targetableColor;
            }

        }
    }

    public void setStartColor()
    {
        for (int i = 0; i < prefabSkin.transform.childCount; i++)
        {
            GameObject _part = prefabSkin.transform.GetChild(i).gameObject;
            if (_part.GetComponent<SpriteRenderer>() != null)
            {
                _part.GetComponent<SpriteRenderer>().color = startColor;
            }

        }
    }
}
