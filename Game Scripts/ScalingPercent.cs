using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScalingPercent : MonoBehaviour
{
    TMPro.TextMeshProUGUI percentageText;

    void Start()
    {
        percentageText = GetComponent<TMPro.TextMeshProUGUI> ();
    }

    public void textUpdate (float value)
    {
        percentageText.text = Mathf.RoundToInt(value) + "%";
    }
}
