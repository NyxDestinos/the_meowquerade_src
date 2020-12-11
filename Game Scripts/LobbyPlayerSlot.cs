using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyPlayerSlot : MonoBehaviour
{
    public int id;
    public string username;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI idText;
    public TextMeshProUGUI ReadyText;
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = username;
        idText.text = id.ToString();
        ReadyText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSlotData(int _id, string _username)
    {
        id = _id;
        username = _username;
        nameText.text = username;
        idText.text = id.ToString();
    }

    public void clearSlot()
    {
        nameText.text = "";
        idText.text = "";
    }

    public void setIsReadyText(bool isActive)
    {
        ReadyText.gameObject.SetActive(isActive);
    }
}
