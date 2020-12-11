using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    public Animator anim;
    public int cardIndex;
    public int handIndex;
    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<RectTransform>().localPosition = targetPosition + new Vector3(0f,-450f);
    }

    public void SetCard(int _index)
    {
        cardIndex = _index;
        gameObject.GetComponent<Image>().sprite = GameManager.instance.cardFaceList[_index];
        gameObject.transform.SetParent(ArenaUIManager.instance.handPrefab.transform);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }

    public void SetEffectCard(int _index)
    {
        cardIndex = _index;
        gameObject.GetComponent<Image>().sprite = GameManager.instance.cardFaceList[_index];
    }

    public void OnMouseEnter()
    {
        anim.SetBool("isSelect", true);
        HighlightCard();
    }

    public void OnMouseExit()
    {
        anim.SetBool("isSelect", false);
        Dehighlight();
    }

    public void OnMouseDown()
    {

        if (GameManager.isCurrentTurn)
        {
            GameManager.instance.currentCardHand = handIndex;
            GameManager.instance.currentCardIndex = cardIndex;
            if (cardIndex == 1)
            {
                ArenaUIManager.instance.SelfCastRange();
            }
            else if (cardIndex == 0)
            {
                ArenaUIManager.instance.GetPlayerInRange(2, false);
            }
            else
            {
                ArenaUIManager.instance.GetPlayerInRange(2, true);
            }
        }
    }

    public void HighlightCard()
    {
        handIndex = gameObject.transform.GetSiblingIndex();
        gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
    }

    public void Dehighlight()
    {
        gameObject.transform.SetSiblingIndex(handIndex);
    }
}
