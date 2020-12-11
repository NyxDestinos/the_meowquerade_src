using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusBar : MonoBehaviour
{
    public int id;
    public TextMeshProUGUI playerName;
    public Image playerHead;
    public GameObject heartCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void heartDisplay(int _life)
    {
        for (int i = 0; i < heartCounter.transform.childCount; i++)
        {
            heartCounter.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < _life; i++)
        {
            heartCounter.transform.GetChild(i).gameObject.SetActive(true);
        }
        Debug.Log("Already Display : " + _life);
    }
}
